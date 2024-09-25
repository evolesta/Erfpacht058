using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Erfpacht058_API.Controllers.Eigendom
{
    using Erfpacht058_API.Models;
    using Erfpacht058_API.Models.Eigendom;
    using System.Configuration;
    using Erfpacht058_API.Models.OvereenkomstNS;
    using AutoMapper;
    using Erfpacht058_API.Repositories.Interfaces;

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EigendomController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        private readonly IEigendomRepository _eigendomRepo;
        private readonly IAdresRepository _adresRepo;
        private readonly IEigenaarRepository _eigenaarRepo;
        private readonly IHerzieningRepository _herzieningRepo;
        private readonly IBestandRepository _bestandRepository;
        private readonly IOvereenkomstRepository _overeenkomstRepository;

        public EigendomController(IConfiguration configuration, IMapper mapper, IEigendomRepository eigendomRepo, IAdresRepository adresRepository,
            IEigenaarRepository eigenaarRepo, IHerzieningRepository herzieningRepository, IBestandRepository bestandRepository, IOvereenkomstRepository overeenkomstRepository)
        {
            _configuration = configuration;
            _mapper = mapper;

            _eigendomRepo = eigendomRepo;
            _adresRepo = adresRepository;
            _eigenaarRepo = eigenaarRepo;
            _herzieningRepo = herzieningRepository;
            _bestandRepository = bestandRepository;
            _overeenkomstRepository = overeenkomstRepository;
        }

        // GET: api/Eigendom
        /// <summary>
        /// Verkrijg een lijst van alle eigendommen
        /// </summary>
        /// <param name="context"></param>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Eigendom>>> GetEigendom()
        {
            return Ok(await _eigendomRepo.GetEigendommen());
        }

        // GET: api/Eigendom/5
        /// <summary>
        /// Verkrijg een enkel eigendom object
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Eigendom>> GetEigendom(int id)
        {
            var eigendom = await _eigendomRepo.GetEigendom(id);

            if (eigendom != null) return Ok(eigendom);
            else return NotFound();
        }

        // PUT: api/Eigendom/5
        /// <summary>
        /// Wijzig een bestaand eigendom object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="eigendomDto"></param>
        /// <returns></returns>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEigendom(int id, EigendomDto eigendomDto)
        {
            // Map de Dto naar Eigendom
            var eigendom = _mapper.Map<Eigendom>(eigendomDto);

            // Werk object bij in Database
            var result = await _eigendomRepo.EditEigendom(id, eigendom);

            if (result != null) return Ok(eigendom);
            else return NotFound();
        }

        // POST: api/Eigendom
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Aanmaken van een nieuw Eigendom object voor een erfpacht constructie
        /// </summary>
        /// <param name="eigendomDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Eigendom>> PostEigendom(EigendomDto eigendomDto)
        {
            // map Dto naar Eigendom
            var eigendom = _mapper.Map<Eigendom>(eigendomDto);

            // Voer object op in database
            return Ok(await _eigendomRepo.AddEigendom(eigendom));
        }

        // DELETE: api/Eigendom/5
        /// <summary>
        /// Verwijder een bestaand eigendom object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEigendom(int id)
        {
            // Verwijder uit DB
            var result = await _eigendomRepo.DeleteEigendom(id);

            if (result != null) return Ok(result);
            else return NotFound();
        }

        // === Adres gerelateerde routes ===

        // GET: api/Eigendom/adres
        /// <summary>
        /// Verkrijg alle adressen
        /// </summary>
        /// <returns></returns>
        [HttpGet("adres")]
        public async Task<ActionResult<IEnumerable<Adres>>> GetAdressen()
        {
            return Ok(await _adresRepo.GetAdressen());
        }

        // POST: api/Eigendom/adres/1
        /// <summary>
        /// Voeg een nieuw adres toe aan een eigendom object
        /// </summary>
        /// <param name="eigendomId">ID van het Eigendom object</param>
        /// <returns></returns>
        [HttpPost("adres/{eigendomId}")]
        public async Task<ActionResult<Eigendom>> AddAdresToEigendom(int eigendomId, AdresDto adresDto)
        {
            // Map Dto naar object
            var adres = _mapper.Map<Adres>(adresDto);

            var result = await _adresRepo.AddAdres(eigendomId, adres);

            if (result != null) return Ok(result);
            else return NotFound();
        }

        // PUT: /eigendom/adres/1
        /// <summary>
        /// Wijzig een bestaand adres van een Eigendom object
        /// </summary>
        /// <param name="eigendomId"></param>
        /// <param name="adresDto"></param>
        /// <returns></returns>
        [HttpPut("adres/{eigendomId}")]
        public async Task<ActionResult<Adres>> EditAdres(int eigendomId, AdresDto adresDto)
        {
            var result = await _adresRepo.EditAdres(eigendomId, adresDto);

            if (result != null) return Ok(result);
            else return NotFound();
        }

        // === Eigenaar gerelateerde endpoints

        // POST: /eigendom/eigenaar/1
        /// <summary>
        /// Voeg een nieuwe eigenaar toe en koppel deze aan het eigendom object
        /// </summary>
        /// <param name="eigendomId"></param>
        /// <param name="eigenaarDto"></param>
        /// <returns></returns>
        [HttpPost("eigenaar/{eigendomId}")]
        public async Task<ActionResult<Eigendom>> AddNewEigenaarToEigendom(int eigendomId, EigenaarDto eigenaarDto)
        {
            // Map Dto naar Eigenaar
            var eigenaar = _mapper.Map<Eigenaar>(eigenaarDto);

            var result = await _eigenaarRepo.AddNewEigenaarToEigendom(eigendomId, eigenaar);
            
            if (result != null) return Ok(eigenaar);
            else return NotFound();
        }

        // PUT: /eigendom/5/eigenaar/5 
        /// <summary>
        /// Voeg een bestaande eigenaar toe aan een eigendom
        /// </summary>
        /// <param name="eigendomId"></param>
        /// <param name="eigenaarId"></param>
        /// <returns></returns>
        [HttpPut("eigenaar/{eigendomId}/{eigenaarId}")]
        public async Task<ActionResult<Eigenaar>> addExistingEigenaarToEigendom(int eigendomId, int eigenaarId)
        {
            var result = await _eigenaarRepo.AddExistingEigenaarToEigendom(eigendomId, eigenaarId);

            if (result != null) return Ok(result);
            else return NotFound();
        }

        // PUT: /eigendom/eigenaar/5/5
        /// <summary>
        /// Ontkoppel een bestaande eigenaar van een eigendom object
        /// </summary>
        /// <param name="eigendomId"></param>
        /// <param name="eigenaarId"></param>
        /// <returns></returns>
        [HttpDelete("eigenaar/{eigendomId}/{eigenaarId}")]
        public async Task<ActionResult<Eigenaar>> DeleteEigenaarFromEigendom(int eigendomId, int eigenaarId)
        {
            var result = await _eigenaarRepo.DeleteEigenaarFromEigendom(eigendomId, eigenaarId);

            if (result != null) return Ok(result);
            else return NotFound();
        }

        // === HERZIENING gerelateerde velden ===

        // POST /Eigendom/herziening/5
        /// <summary>
        /// Voeg een nieuw herziening object toe aan het Eigendom
        /// </summary>
        /// <param name="eigendomId"></param>
        /// <param name="herzieningDto"></param>
        /// <returns></returns>
        [HttpPost("herziening/{eigendomId}")]
        public async Task<ActionResult<Herziening>> AddHerzieningToEigendom(int eigendomId, HerzieningDto herzieningDto)
        {
            // map Dto naar Herziening
            var herziening = _mapper.Map<Herziening>(herzieningDto);

            var result = await _herzieningRepo.AddHerzieningToEigendom(eigendomId, herziening);

            if (result != null) return Ok(result);
            else return NotFound();
        }

        // PUT: /eigendom/herziening/5
        /// <summary>
        /// Wijzig een bestaande herziening van een eigendom object
        /// </summary>
        /// <param name="eigendomId"></param>
        /// <param name="herzieningDto"></param>
        /// <returns></returns>
        [HttpPut("herziening/{eigendomId}")]
        public async Task<ActionResult<Herziening>> UpdateHerziening(int eigendomId, HerzieningDto herzieningDto)
        {
            var result = await _herzieningRepo.UpdateHerziening(eigendomId, herzieningDto);

            if (result != null) return Ok(result);
            else return NotFound();
        }

        // POST: /eigendom/bestand/5
        /// <summary>
        /// Upload een nieuw bestand onder het eigendom
        /// </summary>
        /// <param name="eigendomId"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Geef een array van bestanden in de body van de request mee als formdata:
        ///     POST /eigendom/bestand/1
        ///     {
        ///         "Files": binary
        ///         "Files": binary
        ///     }
        /// </remarks>
        [HttpPost("bestand/{eigendomId}")]
        public async Task<ActionResult<Eigendom>> AddBestand(int eigendomId, BestandDtoUpload bestandDto)    
        {
            // Behandel bestand
            if (bestandDto.Files == null || bestandDto.Files.Count == 0)
                return BadRequest("Bestand niet aanwezig of geaccepteerd");

            // Genereer storage pad
            var basePath = _configuration["Bestanden:OpslagPad"];
            var rootPath =  "/" + eigendomId.ToString();
            var fullRoot = basePath + rootPath;
            if (!Directory.Exists(fullRoot)) // Check of de directory bestaat, zo niet maak aan
                Directory.CreateDirectory(fullRoot);

            // Doorloop de lijst met alle bestanden
            foreach (var file in bestandDto.Files)
            {
                // Schrijf het geuploade bestand naar de storage
                //var filepath = Path.Combine(storageLoc, file.FileName);
                var filepath = rootPath + "/" + file.FileName;
                using (var stream = new FileStream(fullRoot + "/" + file.FileName, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Maak een nieuw database object
                var bestand = new Bestand
                {
                    Naam = file.FileName,
                    Beschrijving = "",
                    GrootteInKb = file.Length / 1024,
                    SoortBestand = SoortBestand.Algemeen,
                    Pad = filepath
                };

                await _bestandRepository.AddBestand(eigendomId, bestand);
            }

            return Ok();
        }

        // POST: api/overeenkomst/{eigendomId}
        /// <summary>
        /// Maak een nieuwe overeenkomst aan
        /// </summary>
        /// <param name="eigendomId"></param>
        /// <param name="overeenkomstDto"></param>
        /// <returns></returns>
        [HttpPost("overeenkomst/{eigendomId}")]
        public async Task<ActionResult<Eigendom>> AddOvereenkomst(int eigendomId, OvereenkomstDto overeenkomstDto)
        {
            // Map Dto naar overeenkomst
            var overeenkomst = _mapper.Map<Overeenkomst>(overeenkomstDto);

            var result = await _overeenkomstRepository.AddOvereenkomst(eigendomId, overeenkomst);

            if (result != null) return Ok(result);
            else return NotFound();
        }

        // PUT: /eigendom/overeenkomst/5/5
        /// <summary>
        /// Koppel een overeenkomst aan een eigendom object
        /// </summary>
        /// <param name="eigendomId"></param>
        /// <param name="overeenkomstId"></param>
        /// <returns></returns>
        [HttpPut("overeenkomst/{eigendomId}/{overeenkomstId}")]
        public async Task<ActionResult<Eigendom>> KoppelOvereenkomstAanEigendom(int eigendomId, int overeenkomstId)
        {
            var result = await _overeenkomstRepository.KoppelOvereenkomstAanEigendom(eigendomId, overeenkomstId);

            if (result != null) return Ok(result); 
            else return NotFound();
        }
    }
}
