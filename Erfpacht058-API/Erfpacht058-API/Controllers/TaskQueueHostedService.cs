using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Rapport;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.Dynamic;
using Erfpacht058_API.Controllers.Rapport;
using Erfpacht058_API.Models.Facturen;
using System.Configuration;
using System.Text;
using System.Xml.Linq;
using System.IO.Compression;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;

namespace Erfpacht058_API.Controllers
{
    public class TaskQueueHostedService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly ConcurrentQueue<TaskQueue> _taskQueue = new ConcurrentQueue<TaskQueue>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);
        private readonly IServiceScopeFactory _scopeFactory;
        private int _invoiceNrCounter = 0;
        public TaskCompletionSource<bool> _taskCompletionSource { get; } = new TaskCompletionSource<bool>();

        public TaskQueueHostedService(ILogger<TaskQueueHostedService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public void EnqueueTask(TaskQueue task)
        {
            _taskQueue.Enqueue(task);
            _signal.Release();
        }

        public async Task ExecuteBackgroundTask(CancellationToken stoppingToken)
        {
            await ExecuteAsync(stoppingToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await _signal.WaitAsync(stoppingToken); // Wacht tot nieuw signaal voor het uitvoeren van een taak

                while (_taskQueue.TryDequeue(out var task))
                {
                    // Zet task in Progress
                    _logger.LogInformation("Achtergrondtaak in behandeling");
                    await using var scope = _scopeFactory.CreateAsyncScope();
                    var context = scope.ServiceProvider.GetRequiredService<Erfpacht058_APIContext>();
                    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                    task.Status = Status.InBehandeling;
                    context.Entry(task).State = EntityState.Modified;
                    await context.SaveChangesAsync();

                    // Async Task uitvoeren
                    try
                    {
                        // Task async uitvoeren adhv het soort taak
                        switch (task.SoortTaak)
                        {
                            case SoortTaak.Import:
                                await Import(task.Import, context);
                                break;
                            case SoortTaak.Export:
                                await Export(task.Export, "", context, config);
                                break;
                            case SoortTaak.Facturen:
                                getLatestInvNr(context);
                                await GenerateInvoices(task.FactuurJob, context, config);
                                break;
                        }

                        // Zet status op succesvol ivm geen fouten
                        task.Status = Status.Succesvol;
                        _logger.LogInformation("Taak succesvol afgerond");

                        _taskCompletionSource.SetResult(true); // Geef succes terug
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Fout bij uitvoeren taak: " + ex.Message);
                        task.Fout = ex.Message;
                        task.Status = Status.Mislukt;

                        _taskCompletionSource.SetException(ex); // Geef exception terug
                    }
                    finally
                    {
                        // Wijzig taak naar afgehandeld in database
                        task.AfgerondDatum = DateTime.Now;
                        context.Entry(task).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                    }
                }
            }
        }

        // Background Tasks

        /// <summary>
        /// Behandel een export verzoek naar gewenst formaat van een model gespecificeerd
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task Export(Export export, string outputFormat, Erfpacht058_APIContext context, IConfiguration configuration)
        {
            // Verkrijg Export details
            var template = await context.Template
                .Include(e => e.RapportData)
                .Include(e => e.Filters)
                .FirstOrDefaultAsync(e => e.Id == export.Template.Id);

            // Verkrijg het betreffende model Type
            var modelType = Assembly.GetExecutingAssembly().GetType(template.Model);
            if (modelType == null)
                throw new InvalidOperationException("Model niet gevonden");

            // Zet de entiteit van de DbContext dynamisch a.d.h.v. modelType
            var setMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set), 1, new Type[] { });
            setMethod = setMethod.MakeGenericMethod(modelType);
            var dbSet = (IQueryable)setMethod.Invoke(context, null);

            // Pas eventuele filters toe aan de DbSet object
            foreach (var filter in template.Filters)
            {
                var parameter = Expression.Parameter(modelType, "x");
                var property = Expression.Property(parameter, filter.Key);
                var convertedValue = Convert.ChangeType(filter.Value, property.Type);
                var value = Expression.Constant(convertedValue, property.Type);
                BinaryExpression binaryExpression = null;

                // Op basis van de Enum operator een actie uitvoeren
                switch (filter.Operation)
                {
                    case Operator.Equal:
                        binaryExpression = Expression.Equal(property, value);
                        break;
                    case Operator.NotEqual:
                        binaryExpression = Expression.NotEqual(property, value);
                        break;
                    case Operator.GreaterThen:
                        binaryExpression = Expression.GreaterThan(property, value);
                        break;
                    case Operator.LessThen:
                        binaryExpression = Expression.LessThan(property, value);
                        break;
                    case Operator.GreaterThanEqual:
                        binaryExpression = Expression.GreaterThanOrEqual(property, value);
                        break;
                    case Operator.LessThanEqual:
                        binaryExpression = Expression.LessThanOrEqual(property, value);
                        break;
                }

                // Filter toepassen
                if (binaryExpression != null)
                {
                    var lambda = Expression.Lambda(binaryExpression, parameter);
                    // Apply the filter using the 'Where' method on the IQueryable
                    dbSet = dbSet.Provider.CreateQuery(
                        Expression.Call(
                            typeof(Queryable),
                            "Where",
                            new Type[] { modelType },
                            dbSet.Expression,
                            Expression.Quote(lambda)
                        ));
                }
            }

            /* Stel een Dictionary samen met als sleutel kolomnaam en waarde als rijen
             * Voorbeeld:
             * Naam => [de Vries, de Jong, Atsma]
             * Email => [test@test.nl, tester@hallo.nl]
             */
            Dictionary<string, List<object>> exportData = new Dictionary<string, List<object>>();

            var dbData = await dbSet
                .Cast<object>()
                .ToListAsync(); // verkrijg de (gefilterde) resultaten van de database asynchroon

            foreach (var column in template.RapportData)
            {
                var columnData = dbData
                    .Select(item => item.GetType().GetProperty(column.Key).GetValue(item, null))
                    .ToList();

                exportData.Add(column.Key, columnData);
            }

            // Genereer een export bestand adhv de keuze van de gebruiker
            string exportFile = "";
            var exportContext = new ExportStrategyContext(new CSVExportStrategy()); // default strategy
            switch (export.Formaat)
            {
                case Formaat.PDF:
                    exportContext.SetExportStrategy(new PDFExportStrategy());
                    exportFile = exportContext.GenerateFile(exportData, configuration, export);
                    break;
                case Formaat.Excel:
                    exportContext.SetExportStrategy(new ExcelExportStrategy());
                    exportFile = exportContext.GenerateFile(exportData, configuration, export);
                    break;
                case Formaat.CSV:
                    exportContext.SetExportStrategy(new CSVExportStrategy());
                    exportFile = exportContext.GenerateFile(exportData, configuration, export);
                    break;
            }

            // Wijzig het pad in de Export
            export.ExportPad = exportFile;
            context.Entry(export).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Behandel een import verzoek
        /// </summary>
        /// <param name="import"></param>
        /// <returns></returns>
        public async Task Import(Import import, Erfpacht058_APIContext context)
        {
            // Verkrijg vertaaltabel
            var translateModel = await context.TranslateModel
                .Include(e => e.Translations)
                .FirstOrDefaultAsync(e => e.Id == import.TranslateModel.Id);
            if (translateModel == null) throw new Exception("Geen correcte vertaaltabel gevonden");

            // Verkrijg het betreffende model Type
            var modelType = Assembly.GetExecutingAssembly().GetType(translateModel.Model);
            if (modelType == null)
                throw new InvalidOperationException("Model niet gevonden");

            // Zet de entiteit van de DbContext dynamisch a.d.h.v. modelType
            var setMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set), 1, new Type[] { });
            setMethod = setMethod.MakeGenericMethod(modelType);
            var dbSet = (IQueryable)setMethod.Invoke(context, null);
            var addMethod = dbSet.GetType().GetMethod(nameof(DbSet<object>.Add));

            // CSV Reader configuratie samenstellen
            var csvReaderConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
            };

            // CSV uitlezen
            using (var stream = new StreamReader(import.importPad))
            // CSV Reader initialiseren
            using (var csvReader = new CsvReader(stream, csvReaderConfig))
            {
                // Lees de Headers van de CSV en initialiseer een vertaaltabel dict.
                csvReader.Read();
                csvReader.ReadHeader();
                var csvHeaders = csvReader.Context.Reader.HeaderRecord;
                var headerToProperyMap = new Dictionary<string, string>();

                // Stel de vertaaltabel samen adhv de translations uit de database
                foreach (var translation in translateModel.Translations)
                {
                    var csvHeader = translation.CSVColummnName;
                    var modelPropertyName = translation.ModelColumnName;
                    if (!string.IsNullOrEmpty(csvHeader) && !string.IsNullOrEmpty(modelPropertyName))
                    {
                        headerToProperyMap[csvHeader] = modelPropertyName;
                    }
                }

                // Loop door de CSV Regels heen met een while loop
                while (csvReader.Read())
                {
                    // Maak een nieuwe instantie aan van het toe te voegen object aan het model
                    var entityToAdd = Activator.CreateInstance(modelType);

                    // Doorloop iedere header en stel het toe te voegen object samen adhv de vertaaltabel
                    foreach (var header in csvHeaders)
                    {
                        if (headerToProperyMap.TryGetValue(header, out var propertyName))
                        {
                            // Verkrijg model kolomnaam en waarde
                            var value = csvReader.GetField(header);
                            var property = modelType.GetProperty(propertyName);
                            if (property != null)
                            {
                                // Controleer of Enum waarden en Parse deze indien nodig
                                if (property.PropertyType.IsEnum)
                                {
                                    var enumValue = Enum.Parse(property.PropertyType, value);
                                    property.SetValue(entityToAdd, enumValue);
                                }
                                else
                                {
                                    // Reguliere waarden worden direct direct aan het object toegekend
                                    property.SetValue(entityToAdd, Convert.ChangeType(value, property.PropertyType));
                                }
                            }
                        }
                    }

                    // Voeg het object toe aan de context
                    addMethod.Invoke(dbSet, new[] { entityToAdd });
                }

                // Sla de nieuwe objecten op in de database context
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Achtergrond taak die facturen genereert
        /// </summary>
        /// <returns></returns>
        public async Task GenerateInvoices(FactuurJob factuurJob, Erfpacht058_APIContext context, IConfiguration configuration)
        {
            // verkrijg alle overeenkomsten die in de facturatieperiode vallen
            var overeenkomsten = await context.Overeenkomst
                .Include(e => e.Financien)
                .Include(e => e.Eigendom)
                    .ThenInclude(rel => rel.Eigenaar)
                .Include(e => e.Eigendom)
                    .ThenInclude(rel => rel.Adres)
                .Where(f => f.Financien.FactureringsPeriode == factuurJob.FactureringsPeriode)
                .ToListAsync();

            // Facturen storage output gereed maken
            var outputDir = configuration["Bestanden:Facturen"] + "/" + factuurJob.Id.ToString() + "/";
            var csvPad = outputDir + "/FacturenJob-" + factuurJob.Id.ToString() + ".csv";
            Directory.CreateDirectory(outputDir); // map aanmaken
            Directory.CreateDirectory(outputDir + "/XML"); // XMl output

            // CSV genereren
            StringBuilder csvContent = new StringBuilder();
            string[] csvHeaders = { "Nummer", "Datum", "Bedrag" };
            string headerLine = string.Join(",", csvHeaders); // CSV kopregel genereren en wegschrijven naar bestand
            csvContent.AppendLine(headerLine);
            File.WriteAllText(csvPad, csvContent.ToString());
            csvContent.Clear();

            // Genereer per overeenkomst een nieuwe factuur
            foreach (var overeenkomst in overeenkomsten)
            {
                // Nieuwe factuur aanmaaken
                var factuur = new Factuur
                {
                    Eigenaar = overeenkomst.Eigendom.Eigenaar[0], // altijd op naam van eerste eigenaar
                    Datum = DateTime.Now,
                    Nummer = generateFactuurnr(context),
                    Financien = overeenkomst.Financien,
                    Bedrag = 0,
                };

                // Factuurregel opgestellen
                var factuurregels = new FactuurRegels
                {
                    Aantal = 1,
                    Beschrijving = "Canon erfpacht: " + overeenkomst.Eigendom.Adres.Straatnaam + " " + overeenkomst.Eigendom.Adres.Huisnummer,
                    Prijs = overeenkomst.Financien.Bedrag,
                    Totaal = overeenkomst.Financien.Bedrag
                };

                // Factuurregels toevoegen aan context
                context.FactuurRegels.Add(factuurregels);
                factuur.Regels.Add(factuurregels);

                // Factuurbedrag berekenen adhv de regels
                foreach (var regel in factuur.Regels)
                {
                    factuur.Bedrag += regel.Totaal;
                }

                // Relaties leggen en toevoegen aan context
                context.Factuur.Add(factuur);
                factuurJob.Facturen.Add(factuur);
                context.Entry(factuurJob).State = EntityState.Modified; 

                await context.SaveChangesAsync();

                // Stap 2: XML factuur maken en toevoegen aan CSV
                var xmlPad = outputDir + "/XML/" + factuur.Nummer + ".xml";
                XDocument xmldoc = new XDocument(
                        new XElement("Factuur",
                            new XElement("Nummer", factuur.Nummer),
                            new XElement("Datum", factuur.Datum.ToString("dd-MM-yyyy")),
                            new XElement("Bedrag", factuur.Bedrag.ToString()),
                            new XElement("Regels",
                                from regel in factuur.Regels
                                select new XElement("Regel",
                                    new XElement("Aantal", regel.Aantal),
                                    new XElement("Beschrijving", regel.Beschrijving),
                                    new XElement("Prijs", regel.Prijs),
                                    new XElement("Totaal", regel.Totaal)
                            )
                        )
                    ));

                xmldoc.Save(xmlPad); // wegschrijven naar XML

                // CSV regel wegschrijven
                string[] csvData = { factuur.Nummer, factuur.Datum.ToString("dd-MM-yyyy"), factuur.Bedrag.ToString() };
                string csvRegel = string.Join(",", csvData);
                File.AppendAllText(csvPad, csvRegel + Environment.NewLine);
            }

            // ZIP file genereren
            var zipFile = configuration["Bestanden:Facturen"] + "/Facturen-" + factuurJob.Id.ToString() + ".zip";
            ZipFile.CreateFromDirectory(outputDir, zipFile);

            // Factuur Job storage pad bijwerken
            factuurJob.StoragePad = zipFile;
            context.Entry(factuurJob).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        private void getLatestInvNr(Erfpacht058_APIContext context)
        {
            // verkrijg laatste factuurnummer van dit jaar
            var laatsteFactNr = context.Factuur
                .Where(i => i.Nummer.StartsWith(DateTime.Now.Year.ToString()))
                .OrderByDescending(i => i.Nummer)
                .FirstOrDefault();

            if (laatsteFactNr == null)
            {
                _invoiceNrCounter = 0;
            }
            else
            {
                _invoiceNrCounter = Convert.ToInt32(laatsteFactNr.Nummer.Split("-")[1]); // update glob int
            }
        }

        // Functie om een nieuw factuurnummer te genereren
        private string generateFactuurnr(Erfpacht058_APIContext context)
        {
            var year = DateTime.Now.Year.ToString(); // verkrijg huidige jaar

            // Verhoog de teller mbv Interlocked
            Interlocked.Increment(ref _invoiceNrCounter);

            return year + "-" + _invoiceNrCounter.ToString("D9");
        }
    }
}
