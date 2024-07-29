using Erfpacht058_API.Controllers.Eigendom;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Eigendom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        // constructor
        public EigendomTests() 
        {
            // Setup een InMemory DB Context voor het testen op Eigendom en Seed deze met sample data
            var options = new DbContextOptionsBuilder<Erfpacht058_APIContext>()
                .UseInMemoryDatabase(databaseName: "Erfpacht058")
                .Options;
            _context = new Erfpacht058_APIContext(options);

            // Creeer controller object
            _controller = new EigendomController(_context, _configuration);
        }

        [Fact]
        // Test 1: Test of twee Eigendom objecten opgehaald kunnen worden
        public async Task a_GetAlleEigendommen()
        {
            // Act
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

        // Helper functie om een Parent Eigendom object op te voeren
        private async Task<ActionResult<Eigendom>> AddTestEigendom()
        {
            var eigendom = new EigendomDto
            {
                Relatienummer = "RL123456789",
                Ingangsdatum = DateTime.Parse("01-01-1980", new CultureInfo("nl-NL")),
                Einddatum = null,
                Complexnummer = "ABC1223456889",
                EconomischeWaarde = 10000,
                VerzekerdeWaarde = 11250,
                Notities = "Dit is een testnotitie",
            };

            return await _controller.PostEigendom(eigendom);
        }

        [Fact]
        // Test 2: Test of een Eigendom object opgevoerd kan worden
        public async Task b_AddNewEigendom()
        {
            // Act
            var result = await AddTestEigendom();

            // Assert
            // Test of het relatienummer klopt met de ingevoegde data
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Eigendom>(okResult.Value);
            Assert.Equal("RL123456789", returnValue.Relatienummer);
        }

        [Fact]
        // Test 3: Test of een Adres relatie opgevoerd kan worden
        public async Task c_AddAdresToEigendom()
        {
            // Act
            var eigendom = await AddTestEigendom(); // Voeg parent eigendom object toe
            eigendom = eigendom.Value;

            
            
        }
    }
}
