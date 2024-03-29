﻿using System;
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
    using Erfpacht058_API.Models.Eigendom;

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EigendomController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;

        public EigendomController(Erfpacht058_APIContext context)
        {
            _context = context;
        }

        // GET: api/Eigendom
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Eigendom>>> GetEigendom()
        {
            return await _context.Eigendom.ToListAsync();
        }

        // GET: api/Eigendom/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Eigendom>> GetEigendom(int id)
        {
            var eigendom = await _context.Eigendom.FindAsync(id);

            if (eigendom == null)
            {
                return NotFound();
            }

            return eigendom;
        }

        // PUT: api/Eigendom/5
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
                Einddatum = null,
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

        // POST: api/Eigendom/adres/1
        /// <summary>
        /// Voeg een nieuw adres toe aan een eigendom object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("adres/{id}")]
        public async Task<ActionResult<Eigendom>> AddAdresToEigendom(int id, AdresDto adresDto)
        {
            // Verkrijg huidig Eigendom object
            var eigendom = _context.Eigendom.Find(id);
            if (eigendom == null)
                return NotFound();

            // Voeg referentie toe naar Eigendom object in nieuwe Adres object en voeg toe aan database
            var adres = new Adres
            {
                Eigendom = eigendom,
                EigendomId = id,
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
            return eigendom;
        }
    }
}
