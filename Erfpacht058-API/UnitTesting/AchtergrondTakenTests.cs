using DocumentFormat.OpenXml.Wordprocessing;
using Erfpacht058_API.Controllers;
using Erfpacht058_API.Controllers.Facturen;
using Erfpacht058_API.Controllers.Rapport;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models;
using Erfpacht058_API.Models.Eigendom;
using Erfpacht058_API.Models.Facturen;
using Erfpacht058_API.Models.OvereenkomstNS;
using Erfpacht058_API.Models.Rapport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using System.Text;

namespace UnitTesting
{
    public class AchtergrondTakenTests
    {
        /*
         * Deze klasse bevat de tests voor het uitvoeren van import, export en facturatietaken op de achtergrond
         */

        private readonly ImportController _importController;
        private readonly ExportController _exportController;
        private readonly FactuurJobController _factuurJobController;

        private readonly Erfpacht058_APIContext _context;
        private readonly Mock<IConfiguration> _configuration;

        private readonly TaskQueueHostedService _taskQueue;
        private readonly Mock<ILogger<TaskQueueHostedService>> _logger;
        private readonly Mock<IServiceScopeFactory> _scopeFactory;

        public AchtergrondTakenTests()
        {
            // creer InMemory DB Context
            var options = new DbContextOptionsBuilder<Erfpacht058_APIContext>()
                .UseInMemoryDatabase(databaseName: "Erfpacht058")
                .Options;
            _context = new Erfpacht058_APIContext(options);

            _configuration = new Mock<IConfiguration>();
            var dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            _configuration.Setup(config => config["Bestanden:ExportPad"]).Returns(dir + "/temp/export");
            _configuration.Setup(config => config["Bestanden:ImportPad"]).Returns(dir + "/temp/import");
            _configuration.Setup(config => config["Bestanden:Facturen"]).Returns(dir + "/temp/facturen");

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(x => x.GetService(typeof(Erfpacht058_APIContext))).Returns(_context);
            serviceProvider.Setup(x => x.GetService(typeof(IConfiguration))).Returns(_configuration?.Object);

            var serviceScope = new Mock<IServiceScope>();
            serviceScope.Setup(x => x.ServiceProvider).Returns(serviceProvider.Object);

            _scopeFactory = new Mock<IServiceScopeFactory>();
            _scopeFactory.Setup(x => x.CreateScope()).Returns(serviceScope.Object);

            _logger = new Mock<ILogger<TaskQueueHostedService>>();

            _taskQueue = new TaskQueueHostedService(_logger.Object, _scopeFactory.Object);
            _importController = new ImportController(_context, _configuration.Object, _taskQueue);
            _exportController = new ExportController(_context, _taskQueue);
            _factuurJobController = new FactuurJobController(_context, _taskQueue);
        }

        [Fact]
        public async Task RunExportTask()
        {
            // Arrange
            ClearDatabase();

            // Creeer een nieuw template voor de export
            var exportTemplate = new Template
            {
                Id = 1,
                Naam = "Tester",
                Maker = "De grote Tester",
                Model = "Erfpacht058_API.Models.Gebruiker",
                AanmaakDatum = DateTime.Now
            };
            exportTemplate.Filters.Add(new Filter { Id = 1, Key = "Actief", Operation = Operator.Equal, Value = "True", TemplateId = 1 });
            exportTemplate.RapportData.Add(new RapportData { Id = 1, Key = "Emailadres", Naam = "Email", TemplateId = 1 });
            _context.Template.Add(exportTemplate);
            _context.SaveChanges();

            // creeer een exportDto voor de actie
            var exportDto = new ExportDto
            {
                Start = true,
                Formaat = Formaat.CSV,
                TemplateId = 1
            };

            // Mock de Controller ivm de gebruiker in de claim en voer een gebruiker op in de context
            var claimsPrincipal = GenerateUserClaim();
            _exportController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            using (var cts = new CancellationTokenSource())
            {
                try
                {
                    // Execute the background task
                    var result = await _exportController.PostExport(exportDto); // test de export functie via de controller
                    var executeTask = _taskQueue.ExecuteBackgroundTask(cts.Token); // setup de Background Service
                    var task = await Task.WhenAny(_taskQueue._taskCompletionSource.Task, Task.Delay(30000)); // Wacht tot de export is afgerond - timeout = 30 sec

                    if (task == _taskQueue._taskCompletionSource.Task && _taskQueue._taskCompletionSource.Task.IsCompletedSuccessfully)
                    {
                        // Assert
                        var okResult = Assert.IsType<OkObjectResult>(result.Result);
                        var returnValue = Assert.IsType<Export>(okResult.Value);
                        Assert.Multiple(
                            () => Assert.Equal(Status.Succesvol, returnValue.Task.Status) // Test of behandelde taak succesvol is
                        );
                    }
                    else
                        Assert.True(false, "Taak is niet binnen 30 seconden voltooid");
                }
                catch (OperationCanceledException)
                {
                    if (!_taskQueue._taskCompletionSource.Task.IsCompletedSuccessfully)
                        Assert.True(false, "De taak was geannuleerd");
                }
                finally
                {
                    cts.Cancel();
                }
            }
        }

        [Fact]
        public async Task RunImportTask()
        {
            // Arrange
            ClearDatabase(); 

            // Creer een vertaalmodel die gebruikt wordt tijdens de test
            var translation1 = new Translation { CSVColummnName = "Naam", ModelColumnName = "Naam" };
            var translation2 = new Translation { CSVColummnName = "Email", ModelColumnName = "Emailadres" };
            var translation3 = new Translation { CSVColummnName = "Voornaam", ModelColumnName = "Voornamen" };
            var translation4 = new Translation { CSVColummnName = "Rol", ModelColumnName = "Role" };

            var translateModel = new TranslateModel
            {
                Id = 1,
                AanmaakDatum = DateTime.Now,
                Maker = "Tester",
                Model = "Erfpacht058_API.Models.Gebruiker",
                Naam = "Test vertalingen",
            };
            translateModel.Translations.Add(translation1);
            translateModel.Translations.Add(translation2);
            translateModel.Translations.Add(translation3);
            translateModel.Translations.Add(translation4);

            _context.TranslateModel.Add(translateModel);
            _context.SaveChanges();

            // creeer een test CSV IFormFile om een import mee te testen
            var csv = GenerateImportCSV();

            // Mock de Controller ivm de gebruiker in de claim en voer een gebruiker op in de context
            var claimsPrincipal = GenerateUserClaim();
            _importController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            using (var cts = new CancellationTokenSource())
            {
                try
                {
                    // Execute the background task
                    var result = await _importController.PostImport(csv, 1); // test de import functie via de controller
                    var executeTask = _taskQueue.ExecuteBackgroundTask(cts.Token); // setup de Background Service
                    var task = await Task.WhenAny(_taskQueue._taskCompletionSource.Task, Task.Delay(30000)); // Wacht tot de export is afgerond - timeout = 30 sec

                    if (task == _taskQueue._taskCompletionSource.Task && _taskQueue._taskCompletionSource.Task.IsCompletedSuccessfully)
                    {
                        // Assert
                        var okResult = Assert.IsType<OkObjectResult>(result.Result);
                        var returnValue = Assert.IsType<Import>(okResult.Value);
                        Assert.Multiple(
                            () => Assert.Equal(Status.Succesvol, returnValue.Task.Status) // Test of behandelde taak succesvol is
                        );
                    }
                    else
                        Assert.True(false, "Taak is niet binnen 30 seconden voltooid");
                }
                catch (OperationCanceledException)
                {
                    if (!_taskQueue._taskCompletionSource.Task.IsCompletedSuccessfully)
                        Assert.True(false, "De taak was geannuleerd");
                }
                finally
                {
                    cts.Cancel();
                }
            }
        }

        [Fact]
        public async Task RunFacturenTask()
        {
            // Arrange
            ClearDatabase();
            // Clear output directory
            foreach (var file in Directory.GetFiles(_configuration.Object["Bestanden:Facturen"])) File.Delete(file);
            foreach (var dir in Directory.GetDirectories(_configuration.Object["Bestanden:Facturen"])) Directory.Delete(dir, true);

            var factuurJobDto = new FactuurJobDto { FactureringsPeriode = Erfpacht058_API.Models.OvereenkomstNS.FactureringsPeriode.Juni };

            // Genereer een user claim
            var claimsPrinciple = GenerateUserClaim();
            _factuurJobController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrinciple }
            };

            GenerateOvereenkomsten(); // creer overeenkomsten in context om facturen te genereren

            // Act
            using (var cts = new CancellationTokenSource())
            {
                try
                {
                    // Execute the background task
                    var result = await _factuurJobController.PostFactuurJob(factuurJobDto); // test de export functie via de controller
                    var executeTask = _taskQueue.ExecuteBackgroundTask(cts.Token); // setup de Background Service
                    var task = await Task.WhenAny(_taskQueue._taskCompletionSource.Task, Task.Delay(30000)); // Wacht tot de export is afgerond - timeout = 30 sec

                    if (task == _taskQueue._taskCompletionSource.Task && _taskQueue._taskCompletionSource.Task.IsCompletedSuccessfully)
                    {
                        // Assert
                        var okResult = Assert.IsType<OkObjectResult>(result.Result);
                        var returnValue = Assert.IsType<FactuurJob>(okResult.Value);
                        Assert.Multiple(
                            () => Assert.Equal(Status.Succesvol, returnValue.Task.Status) // Test of behandelde taak succesvol is
                        );
                    }
                    else
                        Assert.True(false, "Taak is niet binnen 30 seconden voltooid");
                }
                catch (OperationCanceledException)
                {
                    if (!_taskQueue._taskCompletionSource.Task.IsCompletedSuccessfully)
                        Assert.True(false, "De taak was geannuleerd");
                }
                finally
                {
                    cts.Cancel();
                }
            }
        }

        // ++ HELPER FUNCTIONS
        // Functie die een user claim principal creeert die de achtergrond service anderzijds uitleest uit de sessie token
        private ClaimsPrincipal GenerateUserClaim()
        {
            var user = new Gebruiker { Naam = "Test Gebruiker", Emailadres = "test@gebruiker.nl", Actief = true, Role = Rol.Beheerder, Voornamen = "Tester", Wachtwoord = "" };
            _context.Gebruiker.Add(user);
            _context.SaveChanges();

            var claims = new List<Claim>
            {
                new Claim("Username", user.Emailadres),
                new Claim("Naam", user.Naam),
                new Claim(ClaimTypes.Role, "Beheerder"),
                new Claim("Role", "Beheerder"),
                new Claim("Login", (DateTimeOffset.Now).ToUnixTimeSeconds().ToString()),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            return new ClaimsPrincipal(identity);
        }

        // Functie die een CSV bestand IFormFile in het geheugen aanmaakt om de import taak te testen
        private IFormFile GenerateImportCSV()
        {
            // Genereer CSV content
            var sb = new StringBuilder();
            sb.AppendLine("Naam,Email,Voornaam,Rol\n");
            sb.AppendLine("Bast,gbast@test.nl,Gerben,0\n");
            sb.AppendLine("Jopen,jjoop@test.nl,Jan,0\n");

            // Stel een test bestand op
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
            var formFile = new FormFile(stream, 0, stream.Length, "file", "test.csv")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/csv"
            };

            return formFile;
        }

        // Functie die een aantal overeenkomsten genereert voor een facturatie test
        private void GenerateOvereenkomsten()
        {
            var adres = new Adres
            {
                Straatnaam = "Testlaan",
                Huisnummer = 11,
                Postcode = "1233AB",
                Woonplaats = "Testonie",
                Id = 1
            };

            var eigendom = new Eigendom
            {
                Id = 1,
                Ingangsdatum = DateTime.Parse("2024-01-01"),
                Adres = adres,
                Complexnummer = "RL1234",
                EconomischeWaarde = 1000,
                Relatienummer = "1234566",
                VerzekerdeWaarde = 1500,
            };

            // overeenkomst 1
            var financien1 = new Financien
            {
                Id = 1,
                Bedrag = 1000,
                Frequentie = Frequentie.Jaarlijks,
                FactureringsPeriode = FactureringsPeriode.Juni,
                FactureringsWijze = FactureringsWijze.Vooraf
            };
            var overeenkomst1 = new Overeenkomst
            {
                Id = 1,
                Dossiernummer = "XA12444",
                Ingangsdatum = DateTime.Parse("1990-01-01"),
                DatumAkte = DateTime.Parse("2020-01-01"),
                Grondwaarde = 10000,
                Rentepercentage = 8,
                Financien = financien1
            };

            var eigenaar1 = new Eigenaar
            {
                Id = 1,
                Debiteurnummer = "XS1234",
                Ingangsdatum = DateTime.Parse("2000-01-01"),
                Naam = "Tester",
                Voorletters = "T.",
                Voornamen = "Testertje",
                Straatnaam = "testlaan",
                Postcode = "12345",
                Woonplaats = "Testonie",
                Huisnummer = 12,
            };

            eigendom.Adres = adres;
            eigendom.Overeenkomst.Add(overeenkomst1);
            eigendom.Eigenaar.Add(eigenaar1);
            _context.Adres.Add(adres);
            _context.Eigendom.Add(eigendom);
            _context.Financien.Add(financien1);      
            _context.Overeenkomst.Add(overeenkomst1);
            _context.Eigenaar.Add(eigenaar1);
            _context.SaveChanges();
        }

        // Functie die de context database reset
        private void ClearDatabase()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
