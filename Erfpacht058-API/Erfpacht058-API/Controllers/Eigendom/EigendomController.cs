using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Eigendom;
using Microsoft.AspNetCore.Authorization;

namespace Erfpacht058_API.Controllers.Eigendom
{
    using Erfpacht058_API.Models;
    using Erfpacht058_API.Models.Eigendom;
    using System.Configuration;
    using Erfpacht058_API.Models.OvereenkomstNS;

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EigendomController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly IConfiguration _configuration;

        public EigendomController(Erfpacht058_APIContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Eigendom
        /// <summary>
        /// Verkrijg een lijst van alle eigendommen
        /// </summary>
        /// <param name="context"></param>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Eigendom>>> GetEigendom()
        {
            return await _context.Eigendom.ToListAsync();
        }

        // GET: api/Eigendom/5
        /// <summary>
        /// Verkrijg een enkel eigendom object
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Eigendom>> GetEigendom(int id)
        {
            var eigendom = await _context.Eigendom
                .Include(e => e.Adres)
                .Include(e => e.Eigenaar)
                .Include(e => e.Herziening)
                .Include(e => e.Kadaster)
                .Include(e => e.Bestand)
                .Include(e => e.Overeenkomst)
                    .ThenInclude(o => o.Financien)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eigendom == null)
            {
                return NotFound();
            }

            return eigendom;
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
            // Verkrijg het object uit de database
            var eigendom = await _context.Eigendom.FindAsync(id);
            
            if (id != eigendom.Id)
            {
                return BadRequest();
            }

            // Wijzig het Eigendom object met de velden uit het Dto
            eigendom.Relatienummer = eigendomDto.Relatienummer;
            eigendom.Ingangsdatum = eigendomDto.Ingangsdatum;
            eigendom.Einddatum = eigendomDto.Einddatum;
            eigendom.Complexnummer = eigendomDto.Complexnummer;
            eigendom.EconomischeWaarde = eigendomDto.EconomischeWaarde;
            eigendom.VerzekerdeWaarde = eigendomDto.VerzekerdeWaarde;
            eigendom.Notities = eigendomDto.Notities;

            // wijzig gewijzigde object naar database
            _context.Entry(eigendom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EigendomExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
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
            // Gebruik de Dto uit het model om een nieuw eigendom aan te maken
            var eigendom = new Eigendom
            {
                Adres = null,
                Relatienummer = eigendomDto.Relatienummer,
                Ingangsdatum = eigendomDto.Ingangsdatum,
                Einddatum = eigendomDto.Einddatum,
                Complexnummer = eigendomDto.Complexnummer,
                EconomischeWaarde = eigendomDto.EconomischeWaarde,
                VerzekerdeWaarde = eigendomDto.VerzekerdeWaarde,
                Kadaster = null,
                Herziening = null,
                Notities = eigendomDto.Notities,
            };

            // Nieuw object toevoegen aan database
            _context.Eigendom.Add(eigendom);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEigendom", new { id = eigendom.Id }, eigendom);
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
            var eigendom = await _context.Eigendom.FindAsync(id);
            if (eigendom == null)
            {
                return NotFound();
            }

            _context.Eigendom.Remove(eigendom);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EigendomExists(int id)
        {
            return _context.Eigendom.Any(e => e.Id == id);
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
            return await _context.Adres.ToListAsync();
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
            // Verkrijg huidig Eigendom object
            var eigendom = await _context.Eigendom.FindAsync(eigendomId);
            if (eigendom == null)
                return BadRequest();

            // Voeg referentie toe naar Eigendom object in nieuwe Adres object en voeg toe aan database
            var adres = new Adres
            {
                Eigendom = eigendom,
                EigendomId = eigendomId,
                Straatnaam = adresDto.Straatnaam,
                Huisnummer = adresDto.Huisnummer,
                Postcode = adresDto.Postcode,
                Toevoeging = adresDto.Toevoeging,
                Woonplaats = adresDto.Woonplaats,
            };
            _context.Adres.Add(adres);

            // Pas nieuwe relatie toe in eigendom object
            eigendom.Adres = adres;
            _context.Entry(eigendom).State = EntityState.Modified;

            // Pas toe naar database
            await _context.SaveChangesAsync();
            return Ok(eigendom);
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
            // verkrijg huidig eigendom en adres object
            var eigendom = await _context.Eigendom
                .Include(e => e.Adres)
                .FirstOrDefaultAsync(e => e.Id == eigendomId);
            if (eigendom == null) // null check
                return BadRequest();
            var adres = eigendom.Adres;
            if (adres == null)
                return BadRequest("Geen adres aanwezig in eigendom object");

            // Muteer het Adres object
            adres.Straatnaam = adresDto.Straatnaam;
            adres.Huisnummer = adresDto.Huisnummer;
            adres.Toevoeging = adresDto.Toevoeging;
            adres.Postcode = adresDto.Postcode;
            adres.Woonplaats = adresDto.Woonplaats;
            
            // Adres object opslaan
            _context.Entry(adres).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(adres);
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
        public async Task<ActionResult<Eigenaar>> AddEigenaarToEigendom(int eigendomId, EigenaarDto eigenaarDto)
        {
            // Verkrijg Eigendom object
            var eigendom = await _context.Eigendom
                .Include(e => e.Eigenaar)
                .FirstOrDefaultAsync(e => e.Id == eigendomId);
            if (eigendom == null) // Null check
                return BadRequest();

            // Nieuw relatie object aanmaken en relaties leggen
            var eigenaar = new Eigenaar
            {
                Naam = eigenaarDto.Naam,
                Voornamen = eigenaarDto.Voornamen,
                Voorletters = eigenaarDto.Voorletters,
                Straatnaam = eigenaarDto.Straatnaam,
                Huisnummer = eigenaarDto.Huisnummer,
                Toevoeging = eigenaarDto.Toevoeging,
                Postcode = eigenaarDto.Postcode,
                Woonplaats = eigenaarDto.Woonplaats,
                Debiteurnummer = eigenaarDto.Debiteurnummer,
                Ingangsdatum = eigenaarDto.Ingangsdatum,
                Einddatum = eigenaarDto.Einddatum,
            };
            eigenaar.Eigendom.Add(eigendom);
            eigendom.Eigenaar.Add(eigenaar);

            // Nieuwe eigenaar opslaan in database
            _context.Eigenaar.Add(eigenaar);
            _context.Entry(eigendom).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return Ok(eigenaar);
        }

        // PUT: /eigendom/5/eigenaar/5 
        /// <summary>
        /// Voeg een bestaande eigenaar toe aan een eigendom
        /// </summary>
        /// <param name="eigendomId"></param>
        /// <param name="eigenaarId"></param>
        /// <returns></returns>
        [HttpPut("eigenaar/{eigendomId}/{eigenaarId}")]
        public async Task<ActionResult<Eigendom>> addEigenaarToEigendom(int eigendomId, int eigenaarId)
        {
            // Verkrijg eigendom object
            var eigendom = await _context.Eigendom
                .Include(e => e.Eigenaar)
                .FirstOrDefaultAsync(e => e.Id == eigendomId);
            if (eigendom == null)
                return BadRequest();

            // Verkrijg eigenaar object
            var eigenaar = await _context.Eigenaar
                .Include(e => e.Eigendom)
                .FirstOrDefaultAsync(e => e.Id == eigenaarId);
            if (eigenaar == null)
                return BadRequest();

            // Leg relaties vast in database
            eigendom.Eigenaar.Add(eigenaar);
            eigenaar.Eigendom.Add(eigendom);
            _context.Entry(eigendom).State = EntityState.Modified;
            _context.Entry(eigenaar).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(eigenaar);
        }

        // PUT: /eigendom/eigenaar/5/5
        /// <summary>
        /// Ontkoppel een bestaande eigenaar van een eigendom object
        /// </summary>
        /// <param name="eigendomId"></param>
        /// <param name="eigenaarId"></param>
        /// <returns></returns>
        [HttpDelete("eigenaar/{eigendomId}/{eigenaarId}")]
        public async Task<ActionResult<Eigendom>> DeleteEigenaarFromEigendom(int eigendomId, int eigenaarId)
        {
            // Verkrijg eigendom object
            var eigendom = await _context.Eigendom
                .Include(e => e.Eigenaar)
                .FirstOrDefaultAsync(e => e.Id == eigendomId);
            if (eigendom == null) return BadRequest();

            // Verkrijg eigenaar object
            var eigenaar = await _context.Eigenaar
                .Include(e => e.Eigendom)
                .FirstOrDefaultAsync(e => e.Id == eigenaarId);
            if (eigenaar == null) return BadRequest();

            // Verwijder relatie uit eigendom en verwijder eigenaar object
            eigendom.Eigenaar.Remove(eigenaar);
            eigenaar.Eigendom.Remove(eigendom);
            _context.Entry(eigendom).State = EntityState.Modified;
            _context.Entry(eigenaar).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return Ok(eigendom);
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
        public async Task<ActionResult<Eigendom>> AddHerzieningToEigendom(int eigendomId, HerzieningDto herzieningDto)
        {
            // Verkrijg eigendom object
            var eigendom = await _context.Eigendom
                .Include(e => e.Herziening)
                .FirstOrDefaultAsync(e => e.Id == eigendomId);
            if(eigendom == null) return BadRequest();

            // Creeer een nieuw herziening object
            var herziening = new Herziening
            {
                Herzieningsdatum = herzieningDto.Herzieningsdatum,
                VolgendeHerziening = herzieningDto.VolgendeHerziening,
                Eigendom = eigendom,
                EigendomId = eigendomId
            };

            // Voeg toe aan database en leg relaties
            _context.Herziening.Add(herziening);
            eigendom.Herziening = herziening;
            _context.Entry(eigendom).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(eigendom);
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
            // verkrijg eigendom object
            var eigendom = await _context.Eigendom
               .Include(e => e.Herziening)
               .FirstOrDefaultAsync(e => e.Id == eigendomId);
            if (eigendom == null) return BadRequest();

            // Verkrijg herziening van eigendom object
            var herziening = eigendom.Herziening;
            if (herziening == null)
                return BadRequest();

            // Wijzig object
            herziening.Herzieningsdatum = herzieningDto.Herzieningsdatum;
            herziening.VolgendeHerziening = herzieningDto.VolgendeHerziening;

            // Sla wijzigingen op naar database
            _context.Entry(herziening).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(herziening);
        }

        // === Kadaster gerelateerde functies ===
        // POST: /eigendom/kadaster/5
        /// <summary>
        /// Voeg een nieuw kadaster object toe aan een eigendom
        /// </summary>
        /// <param name="eigendomId"></param>
        /// <param name="kadasterDto"></param>
        /// <returns></returns>
        [HttpPost("kadaster/{eigendomId}")]
        public async Task<ActionResult<Kadaster>> AddKadasterToEigendom(int eigendomId, KadasterDto kadasterDto)
        {
            // verkrijg eigendom object
            var eigendom = await _context.Eigendom
               .Include(e => e.Kadaster)
               .FirstOrDefaultAsync(e => e.Id == eigendomId);
            if (eigendom == null) return BadRequest();

            // Maak een nieuw Kadaster object aan
            var kadaster = new Kadaster
            {
                // Koppel enkel het kadastraal nr. voor de Sync
                KadastraalNummer = kadasterDto.KadastraalNummer,
                Eigendom = eigendom,
                EigendomId = eigendomId
            };

            // Maak nieuw Kadaster object, leg relaties en voeg toe aan database
            _context.Kadaster.Add(kadaster);
            eigendom.Kadaster = kadaster;
            _context.Entry(eigendom).State = EntityState.Modified;
            
            await _context.SaveChangesAsync();

            return Ok(kadaster);
        }

        // PUT: /eigendom/kadaster/5
        /// <summary>
        /// Wijzig een bestaand kadaster object bij een eigendom
        /// </summary>
        /// <param name="eigendomId"></param>
        /// <param name="kadasterDto"></param>
        /// <returns></returns>
        [HttpPut("kadaster/{eigendomId}")]
        public async Task<ActionResult<Kadaster>> EditKadaster(int eigendomId, KadasterDto kadasterDto)
        {
            // verkrijg eigendom object
            var eigendom = await _context.Eigendom
               .Include(e => e.Kadaster)
               .FirstOrDefaultAsync(e => e.Id == eigendomId);
            if (eigendom == null) return BadRequest();
            var kadaster = eigendom.Kadaster;
            if (kadaster == null) return BadRequest();

            // Wijzig kadaster object
            kadaster.KadastraalNummer = kadasterDto.KadastraalNummer;

            // Wijzig naar database
            _context.Entry(kadaster).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(kadaster);
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
        public async Task<ActionResult<Bestand>> AddBestand(int eigendomId, BestandDtoUpload bestandDto)    
        {
            // Verkrijg eigendom object
            var eigendom = await _context.Eigendom
                .Include(e => e.Bestand)
                .FirstOrDefaultAsync(e => e.Id == eigendomId);
            if (eigendom == null) return BadRequest();

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
                    Eigendom = eigendom,
                    Naam = file.FileName,
                    Beschrijving = "",
                    GrootteInKb = file.Length / 1024,
                    SoortBestand = SoortBestand.Algemeen,
                    Pad = filepath
                };

                // Leg relaties vast en voeg toe aan context
                eigendom.Bestand.Add(bestand);
                _context.Bestand.Add(bestand);
            }

            // Opslaan in database
            _context.Entry(eigendom).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(eigendom);
        }

        // POST: api/overeenkomst/{eigendomId}
        /// <summary>
        /// Maak een nieuwe overeenkomst aan
        /// </summary>
        /// <param name="eigendomId"></param>
        /// <param name="overeenkomstDto"></param>
        /// <returns></returns>
        [HttpPost("overeenkomst/{eigendomId}")]
        public async Task<ActionResult<Overeenkomst>> AddOvereenkomst(int eigendomId, OvereenkomstDto overeenkomstDto)
        {
            // Verkrijg eigendom object
            var eigendom = await _context.Eigendom
                .Include(e => e.Overeenkomst)
                .FirstOrDefaultAsync(e => e.Id == eigendomId);
            if (eigendom == null) return BadRequest();

            // Creeer een nieuw Financien child object
            var financien = new Financien
            {
                Bedrag = overeenkomstDto.Financien.Bedrag,
                FactureringsWijze = overeenkomstDto.Financien.FactureringsWijze,
                Frequentie = overeenkomstDto.Financien.Frequentie
            };
            // Creeer een nieuw overeenkomst object
            var overeenkomst = new Overeenkomst
            {
                Dossiernummer = overeenkomstDto.Dossiernummer,
                Ingangsdatum = overeenkomstDto.Ingangsdatum,
                Einddatum = overeenkomstDto.Einddatum,
                Grondwaarde = overeenkomstDto.Grondwaarde,
                DatumAkte = overeenkomstDto.DatumAkte,
                Rentepercentage = overeenkomstDto.Rentepercentage,
                Eigendom = eigendom,
                Financien = financien
            };

            // Leg relaties vast en sla object op in database
            _context.Financien.Add(financien);
            _context.Overeenkomst.Add(overeenkomst);
            eigendom.Overeenkomst.Add(overeenkomst);
            _context.Entry(eigendom).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(overeenkomst);
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
            // Verkrijg eigendom en overeenkomst objecten
            var eigendom = await _context.Eigendom.FindAsync(eigendomId);
            if (eigendom == null) return BadRequest();
            var overeenkomst = await _context.Overeenkomst.FindAsync(overeenkomstId);
            if (overeenkomst == null) return BadRequest();

            // Relaties leggen en opslaan
            eigendom.Overeenkomst.Add(overeenkomst);
            overeenkomst.Eigendom = eigendom;
            _context.Entry(eigendom).State = EntityState.Modified;
            _context.Entry(overeenkomst).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(eigendom);
        }
    }
}
