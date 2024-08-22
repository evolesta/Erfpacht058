using Erfpacht058_API.Controllers.Eigendom;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models;
using Erfpacht058_API.Models.Eigendom;
using Erfpacht058_API.Models.OvereenkomstNS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NuGet.ContentModel;
using System.Globalization;
using Xunit;

namespace UnitTesting
{
    /* Toelichting op UNIT TEST => MutatieEigendom
    Stap 1: Seed twee Eigendom objecten zonder relaties naar de InMemory database
    Test 1: Test of beiden geseede Eigendom objecten opgehaald kunnen worden
    Test 2: Test of een Eigendom object opgevoerd kan worden
    Test 3: Test of een Adres relatie opgevoerd kan worden
     */

    public class EigendomTests
    {
        private readonly EigendomController _controller;
        private readonly Erfpacht058_APIContext _context;
        private readonly Mock<IConfiguration> _configuration;

        // constructor
        public EigendomTests() 
        {
            // Setup een InMemory DB Context voor het testen op Eigendom en Seed deze met sample data
            var options = new DbContextOptionsBuilder<Erfpacht058_APIContext>()
                .UseInMemoryDatabase(databaseName: "Erfpacht058")
                .Options;
            _context = new Erfpacht058_APIContext(options);

            // Mock de Configuratie
            _configuration = new Mock<IConfiguration>();
            var dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            _configuration.Setup(config => config["Bestanden:OpslagPad"]).Returns(dir + "/temp");

            // Creeer controller object
            _controller = new EigendomController(_context, _configuration.Object);
        }

        [Fact]
        // Test 1: Test of twee Eigendom objecten opgehaald kunnen worden
        public async Task GetAlleEigendommen()
        {
            // Act
            ClearDatabase(); // reset Database

            _context.Eigendom.AddRange(new List<Eigendom> // Seed Eigendom data
            {
                new Eigendom { Id = 1, Relatienummer = "RL012335540", Ingangsdatum = DateTime.Parse("01-01-2025", new CultureInfo("nl-NL")), Complexnummer = "Xasss5s5s", EconomischeWaarde = 1002,
                VerzekerdeWaarde = 5550, Notities = "Test"},
                new Eigendom { Id = 2, Relatienummer = "RL01255511", Ingangsdatum = DateTime.Parse("01-01-2010", new CultureInfo("nl-NL")), Complexnummer = "ABCADDEEE", EconomischeWaarde = 66600,
                VerzekerdeWaarde = 1024, Notities = "Testertje"},
            });
            _context.SaveChanges();

            var result1 = await _controller.GetEigendom(1);
            var result2 = await _controller.GetEigendom(2);

            // Assert
            // Test of de Primary key die teruggegeven 1 en 2 zijn
            var okResult1 = Assert.IsType<OkObjectResult>(result1.Result);
            var returnValue1 = Assert.IsType<Eigendom>(okResult1.Value);
            var okResult2 = Assert.IsType<OkObjectResult>(result2.Result);
            var returnValue2 = Assert.IsType<Eigendom>(okResult2.Value);

            Assert.Multiple(
                () => Assert.Equal(1, returnValue1.Id),
                () => Assert.Equal(2, returnValue2.Id)
            );
        }

        [Fact]
        // Test 2: Test of een Eigendom object opgevoerd kan worden
        public async Task AddNewEigendom()
        {
            // Act
            ClearDatabase(); // Reset database

            var eigendom = new EigendomDto
            {
                Complexnummer = "012566",
                Relatienummer = "RL12346",
                Ingangsdatum = DateTime.Parse("2024-01-01"),
                Einddatum = DateTime.Parse("2030-01-01"),
                EconomischeWaarde = 10500,
                VerzekerdeWaarde = 115200,
                Notities = "Dit is een test"
            };

            var result = await _controller.PostEigendom(eigendom);

            // Assert
            // Test of het relatienummer klopt met de ingevoegde data
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Eigendom>(okResult.Value);
            Assert.Equal("RL12346", returnValue.Relatienummer); // Test of het eigendom object het opgevoerde relatienummer bevat
        }

        [Fact]
        // Test 3: Test of een Adres relatie opgevoerd kan worden
        public async Task AddAdresToEigendom()
        {
            // Act
            ClearDatabase(); // Reset database
            AddTestEigendom(); // voer test eigedom object op

            var adres = new AdresDto
            {
                Straatnaam = "Testlaan",
                Huisnummer = 12,
                Postcode = "1234AB",
                Woonplaats = "Testonie",
                Huisletter = "X",
                Toevoeging = "1"
            };

            var result = await _controller.AddAdresToEigendom(1, adres);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Eigendom>(okResult.Value);
            Assert.Equal("Testlaan", returnValue.Adres.Straatnaam); // Test of het eigendom object een relatie bevat met het opgevoerde adres
        }

        [Fact]
        // Test 4: Test of een eigenaar relatie opgevoerd kan worden
        public async Task AddEigenaarToEigendom()
        {
            // Act
            ClearDatabase(); // Reset database
            AddTestEigendom(); // voer test eigendom object op

            var eigenaar = new EigenaarDto
            {
                Ingangsdatum = DateTime.Parse("1900-01-01"),
                Einddatum = DateTime.Parse("2025-01-01"),
                Debiteurnummer = "66666662",
                Naam = "Kerel",
                Straatnaam = "Waalderlaan",
                Huisnummer = 12,
                Postcode = "1234AB",
                Toevoeging = "1",
                Voorletters = "X",
                Voornamen = "Semafoor",
                Woonplaats = "Drachten"
            };

            var result = await _controller.AddEigenaarToEigendom(1, eigenaar);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Eigendom>(okResult.Value);
            Assert.Equal("Kerel", returnValue.Eigenaar[0].Naam); // Test of het eigendom object een eigenaar in de collectie heeft staan met de naam
        }

        [Fact]
        // Test 5: Test of een herziening opgevoerd kan worden
        public async Task AddHerzieningToEigendom()
        {
            // Act
            ClearDatabase(); // Reset database
            AddTestEigendom();

            var herziening = new HerzieningDto
            {
                Herzieningsdatum = DateTime.Parse("2020-01-01"),
                VolgendeHerziening = 25
            };

            var result = await _controller.AddHerzieningToEigendom(1, herziening);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Eigendom>(okResult.Value);
            Assert.Equal(25, returnValue.Herziening.VolgendeHerziening); // Test of het relationele herzieningsobject 25 bevat voor de volgende herziening
        }

        [Fact]
        // Test 6: Test of er een overeenkomst toegevoegd kan worden + Financien
        public async Task AddOvereenkomstToEigendom()
        {
            // Act
            ClearDatabase(); // Reset database
            AddTestEigendom();

            var financien = new FinancienDto
            {
                Bedrag = 1000,
                Frequentie = Frequentie.Jaarlijks,
                FactureringsPeriode = FactureringsPeriode.December,
                FactureringsWijze = FactureringsWijze.Vooraf
            };

            var overeenkomst = new OvereenkomstDto
            {
                Dossiernummer = "XA12444",
                Ingangsdatum = DateTime.Parse("1990-01-01"),
                DatumAkte = DateTime.Parse("2020-01-01"),
                Grondwaarde = 10000,
                Rentepercentage = 8,
                Financien = financien
            };

            var result = await _controller.AddOvereenkomst(1, overeenkomst);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Eigendom>(okResult.Value);
            Assert.Equal("XA12444", returnValue.Overeenkomst.First().Dossiernummer);
        }

        [Fact]
        // Test 7: Test of er een bestand geupload kan worden onder een eigendom
        public async Task UploadBestand()
        {
            ClearDatabase(); // Reset database
            AddTestEigendom(); // Voeg een eigendom toe

            // Stel een test bestand op
            var filemock = new Mock<IFormFile>();
            var content = "Dit is een testbestand";
            var filename = "test.txt";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            // Setup IFormFile die meegestuurd wordt
            filemock.Setup(_ => _.OpenReadStream()).Returns(ms);
            filemock.Setup(_ => _.FileName).Returns(filename);
            filemock.Setup(_ => _.Length).Returns(ms.Length);
            var formFile = filemock.Object;
            
            // Creeer een nieuwe DTO
            var bestandDto = new BestandDtoUpload();
            bestandDto.Files = new List<IFormFile> { formFile };

            // Act
            var result = await _controller.AddBestand(1, bestandDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Eigendom>(okResult.Value);

            var config = _configuration.Object;
            var filepath = config["Bestanden:OpslagPad"] + returnValue.Bestand.First().Pad;
            Assert.Multiple(
                () => Assert.Equal("test.txt", returnValue.Bestand.First().Naam), // controleer naam van het bestand
                () => Assert.True(Path.Exists(filepath)) // controleer of het bestand bestaat
            );

            File.Delete(filepath); // verwijder test bestand
        }

        // ++ HELPER FUNCTIES ++
        // Helper functie om een Parent Eigendom object op te voeren
        private void AddTestEigendom()
        {
            // creer een object zonder relaties
            var eigendom = new Eigendom
            {
                Id = 1,
                Relatienummer = "RL123456789",
                Ingangsdatum = DateTime.Parse("01-01-1980", new CultureInfo("nl-NL")),
                Einddatum = null,
                Complexnummer = "ABC1223456889",
                EconomischeWaarde = 10000,
                VerzekerdeWaarde = 11250,
                Notities = "Dit is een testnotitie",
            };

            _context.Eigendom.Add(eigendom);
            _context.SaveChanges();
        }

        // Helper functie die de database reset voor iedere afzonderlijke test
        private void ClearDatabase()
        {
            _context.RemoveRange(_context.Eigendom); // verwijder alle Eigendommen 
            _context.SaveChanges();
        }
    }
}
