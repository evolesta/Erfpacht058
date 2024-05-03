using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Rapport;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.AspNetCore.Http;
using Erfpacht058_API.Models;
using System.Reflection;
using System.Linq.Dynamic.Core;

namespace Erfpacht058_API.Controllers.Rapport
{
    public class TaskQueueHostedService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly ConcurrentQueue<TaskQueue> _taskQueue = new ConcurrentQueue<TaskQueue>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);
        private readonly IServiceScopeFactory _scopeFactory;

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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {           
            while (!stoppingToken.IsCancellationRequested)
            {
                await _signal.WaitAsync(stoppingToken); // Wacht tot nieuw signaal voor het uitvoeren van een taak

                while(_taskQueue.TryDequeue(out var task))
                {
                    // Zet task in Progress
                    await using var scope = _scopeFactory.CreateAsyncScope();
                    var context = scope.ServiceProvider.GetRequiredService<Erfpacht058_APIContext>();
                    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                    task.Status = Status.InBehandeling;
                    context.Entry(task).State = EntityState.Modified;
                    await context.SaveChangesAsync();

                    // Async Task uitvoeren
                    try
                    {
                        // Task async uitvoeren
                        await Export(task.Export, "", context, config);

                        // Zet status op succesvol ivm geen fouten
                        task.Status = Status.Succesvol;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error executing Task: " + ex.Message);
                        task.Fout = ex.Message;
                        task.Status = Status.Mislukt;
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
        public async Task Export(Export export, string outputFormat, Erfpacht058_APIContext context, IConfiguration configuration)
        {
            // Verkrijg Export details
            var template = await context.Template
                .Include(e => e.RapportData)
                .FirstOrDefaultAsync(e => e.Id == export.Template.Id);

            // Verkrijg het betreffende model Type
            var modelType = Assembly.GetExecutingAssembly().GetType(template.Model);
            if (modelType == null)
                throw new InvalidOperationException("Model niet gevonden");
            
            // Zet de entiteit van de DbContext dynamisch a.d.h.v. modelType
            var setMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set), 1, new Type[] { });
            setMethod = setMethod.MakeGenericMethod(modelType);
            var dbSet = setMethod.Invoke(context, null);

            /* Stel een Dictionary samen met als sleutel kolomnaam en waarde als rijen
             * Voorbeeld:
             * Naam => [de Vries, de Jong, Atsma]
             * Email => [test@test.nl, tester@hallo.nl]
             */
            Dictionary<string, List<object>> exportData = new Dictionary<string, List<object>>();

            foreach (var column in template.RapportData)
            {
                var propertyInfo = modelType.GetProperty(column.Key);
                var columnData = await ((IQueryable)dbSet)
                    .Select($"{column.Key}") // selecteer de kolom
                    .Cast<object>()
                    .ToListAsync();

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
    }
}
