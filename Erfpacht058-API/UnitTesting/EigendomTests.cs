using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using Erfpacht058_API.Controllers.Eigendom;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models;
using Erfpacht058_API.Models.Eigendom;
using Erfpacht058_API.Models.OvereenkomstNS;
using Erfpacht058_API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NuGet.ContentModel;
using System.Globalization;
using Xunit;
using Erfpacht058_API;

namespace UnitTesting
{
    /* Toelichting op UNIT TEST => MutatieEigendom
    Stap 1: Seed twee Eigendom objecten zonder relaties naar de InMemory database
    Test 1: Test of beiden geseede Eigendom objecten opgehaald kunnen worden
    Test 2: Test of een Eigendom object opgevoerd kan worden
    Test 3: Test of een Adres relatie opgevoerd kan worden
     */

    [Collection("Tests")]
    public class EigendomTests
    {
        private readonly EigendomController _controller;
        private readonly Mock<IConfiguration> _configuration;
        private readonly Mock<IEigendomRepository> _eigendomRepo;
        private readonly Mock<IAdresRepository> _adresRepo;
        private readonly Mock<IEigenaarRepository> _eigenaarRepo;
        private readonly Mock<IHerzieningRepository> _herzieningRepo;
        private readonly Mock<IBestandRepository> _bestandRepo;
        private readonly Mock<IOvereenkomstRepository> _overeenkomstRepo;
        private readonly IMapper _mapper;

        // constructor
        public EigendomTests() 
        {
            // Mock Config
            _configuration = new Mock<IConfiguration>();
            var dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName.Replace("\\", "/");
            _configuration.Setup(config => config["Bestanden:OpslagPad"]).Returns(dir + "/temp");

            // Mocking
            _eigendomRepo = new Mock<IEigendomRepository>();
            _adresRepo = new Mock<IAdresRepository>();
            _eigenaarRepo = new Mock<IEigenaarRepository>();
            _herzieningRepo = new Mock<IHerzieningRepository>();
            _bestandRepo = new Mock<IBestandRepository>();
            _overeenkomstRepo = new Mock<IOvereenkomstRepository>();

            // Setup mapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            });
            _mapper = config.CreateMapper();

            // Creeer controller object
            _controller = new EigendomController(_configuration.Object, _mapper, _eigendomRepo.Object, _adresRepo.Object, _eigenaarRepo.Object, _herzieningRepo.Object,
                _bestandRepo.Object, _overeenkomstRepo.Object);
        }

        [Fact]
        // Test 1: Test of twee Eigendom objecten opgehaald kunnen worden
        public async Task GetAlleEigendommen()
        {
            // Arrange => voer test eigendommen op
            var eigendommen = new List<Eigendom> // Seed Eigendom data
            {
                new Eigendom { Id = 1, Relatienummer = "RL012335540", Ingangsdatum = DateTime.Parse("01-01-2025", new CultureInfo("nl-NL")), Complexnummer = "Xasss5s5s", EconomischeWaarde = 1002,
                VerzekerdeWaarde = 5550, Notities = "Test"},
                new Eigendom { Id = 2, Relatienummer = "RL01255511", Ingangsdatum = DateTime.Parse("01-01-2010", new CultureInfo("nl-NL")), Complexnummer = "ABCADDEEE", EconomischeWaarde = 66600,
                VerzekerdeWaarde = 1024, Notities = "Testertje"},
            }.AsEnumerable();
            _eigendomRepo.Setup(repo => repo.GetEigendommen()).ReturnsAsync(eigendommen);

            // Act => Test Controller
            var result = await _controller.GetEigendom();

            // Assert => Valideer resultaten
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Eigendom>>(okResult.Value);

            // Valideer of the PK klopt / Twee items retour komen
            Assert.Multiple(
                () => Assert.Equal(1, returnValue[0].Id),
                () => Assert.Equal(2, returnValue[1].Id),
                () => Assert.Equal(2, returnValue.Count())
            );
        }

        [Fact]
        // Test 2: Test of een Eigendom object opgevoerd kan worden
        public async Task AddNewEigendom()
        {
            // Arrange =>
            var eigendomDto = new EigendomDto
            {
                Complexnummer = "012566",
                Relatienummer = "RL12346",
                Ingangsdatum = DateTime.Parse("2024-01-01"),
                Einddatum = DateTime.Parse("2030-01-01"),
                EconomischeWaarde = 10500,
                VerzekerdeWaarde = 115200,
                Notities = "Dit is een test"
            };
            // Mock nieuwe Eigendom object naar Repo
            var eigendom = _mapper.Map<Eigendom>(eigendomDto);
            _eigendomRepo.Setup(repo => repo.AddEigendom(It.IsAny<Eigendom>())).ReturnsAsync(eigendom);

            // Act
            var result = await _controller.PostEigendom(eigendomDto);

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
            // Arrange
            var eigendom = ParentEigendom();

            var adresDto = new AdresDto
            {
                Straatnaam = "Testlaan",
                Huisnummer = 12,
                Postcode = "1234AB",
                Woonplaats = "Testonie",
                Huisletter = "X",
                Toevoeging = "1",
            };
            var adres = _mapper.Map<Adres>(adresDto);
            adres.Eigendom = eigendom;

            _adresRepo.Setup(repo => repo.GetEigendomById(eigendom.Id)).ReturnsAsync(eigendom);
            _adresRepo.Setup(repo => repo.AddAdres(1, It.IsAny<Adres>())).ReturnsAsync(adres);

            // Act
            var result = await _controller.AddAdresToEigendom(1, adresDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Adres>(okResult.Value);
            Assert.Equal("Testlaan", returnValue.Straatnaam); // Test of het eigendom object een relatie bevat met het opgevoerde adres
        }

        [Fact]
        // Test 4: Test of een eigenaar relatie opgevoerd kan worden
        public async Task AddEigenaarToEigendom()
        {
            // Arrange
            var eigendom = ParentEigendom();

            var eigenaarDto = new EigenaarDto
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
            var eigenaar = _mapper.Map<Eigenaar>(eigenaarDto);
            eigenaar.Eigendom.Add(eigendom);

            _eigenaarRepo.Setup(repo => repo.GetEigendomById(eigendom.Id)).ReturnsAsync(eigendom);
            _eigenaarRepo.Setup(repo => repo.AddNewEigenaarToEigendom(1, It.IsAny<Eigenaar>())).ReturnsAsync(eigenaar);

            // Act
            var result = await _controller.AddNewEigenaarToEigendom(1, eigenaarDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Eigenaar>(okResult.Value);
            Assert.Equal("Kerel", returnValue.Naam); // Test of het eigendom object een eigenaar in de collectie heeft staan met de naam
        }

        [Fact]
        // Test 5: Test of een herziening opgevoerd kan worden
        public async Task AddHerzieningToEigendom()
        {
            // Arrange
            var eigendom = ParentEigendom();

            var herzieningDto = new HerzieningDto
            {
                Herzieningsdatum = DateTime.Parse("2020-01-01"),
                VolgendeHerziening = 25
            };
            var herziening = _mapper.Map<Herziening>(herzieningDto);
            herziening.Eigendom = eigendom;

            _herzieningRepo.Setup(repo => repo.GetEigendomById(eigendom.Id)).ReturnsAsync(eigendom);
            _herzieningRepo.Setup(repo => repo.AddHerzieningToEigendom(1, It.IsAny<Herziening>())).ReturnsAsync(herziening);

            // Act
            var result = await _controller.AddHerzieningToEigendom(1, herzieningDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Herziening>(okResult.Value);
            Assert.Equal(25, returnValue.VolgendeHerziening); // Test of het relationele herzieningsobject 25 bevat voor de volgende herziening
        }

        [Fact]
        // Test 6: Test of er een overeenkomst toegevoegd kan worden + Financien
        public async Task AddOvereenkomstToEigendom()
        {
            // Arrange
            var eigendom = ParentEigendom();

            var financienDto = new FinancienDto
            {
                Bedrag = 1000,
                Frequentie = Frequentie.Jaarlijks,
                FactureringsPeriode = FactureringsPeriode.December,
                FactureringsWijze = FactureringsWijze.Vooraf
            };

            var overeenkomstDto = new OvereenkomstDto
            {
                Dossiernummer = "XA12444",
                Ingangsdatum = DateTime.Parse("1990-01-01"),
                DatumAkte = DateTime.Parse("2020-01-01"),
                Grondwaarde = 10000,
                Rentepercentage = 8,
                Financien = financienDto
            };
            var overeenkomst = _mapper.Map<Overeenkomst>(overeenkomstDto);
            overeenkomst.Eigendom = eigendom;

            _overeenkomstRepo.Setup(repo => repo.GetEigendomById(eigendom.Id)).ReturnsAsync(eigendom);
            _overeenkomstRepo.Setup(repo => repo.AddOvereenkomst(1, It.IsAny<Overeenkomst>())).ReturnsAsync(overeenkomst);

            // Act
            var result = await _controller.AddOvereenkomst(1, overeenkomstDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Overeenkomst>(okResult.Value);
            Assert.Equal("XA12444", returnValue.Dossiernummer);
        }

        [Fact]
        // Test 7: Test of er een bestand geupload kan worden onder een eigendom
        public async Task UploadBestand()
        {
            var eigendom = ParentEigendom();

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

            var path = _configuration.Object["Bestanden:OpslagPad"];
            var bestand = new Bestand
            {
                Naam = filename,
                Beschrijving = "",
                GrootteInKb = ms.Length,
                SoortBestand = SoortBestand.Algemeen,
                Pad = path
            };

            _bestandRepo.Setup(repo => repo.GetEigendomById(eigendom.Id)).ReturnsAsync(eigendom);
            _bestandRepo.Setup(repo => repo.AddBestand(1, It.IsAny<Bestand>())).ReturnsAsync(bestand);

            // Act
            var result = await _controller.AddBestand(1, bestandDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result.Result);

            var file = path + "/1/" + filename;
            Assert.Multiple(
                () => Assert.True(Path.Exists(file)) // controleer of het bestand bestaat
            );

            File.Delete(file); // verwijder test bestand
        }

        // Geeft een hoofd Eigendom object terug
        private Eigendom ParentEigendom()
        {
            return new Eigendom
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
        }
    }
    
}
