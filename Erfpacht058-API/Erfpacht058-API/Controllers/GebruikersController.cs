﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models;
using Microsoft.AspNetCore.Authorization;

namespace Erfpacht058_API.Controllers
{
    // Usermanagement van de applicatie, alleen toegankelijk voor gebruikers met de rol 'Beheerder'
    [Route("api/[controller]")]
    [Authorize(Roles = "Beheerder")]
    [ApiController]
    public class GebruikersController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;

        public GebruikersController(Erfpacht058_APIContext context)
        {
            _context = context;
        }

        // GET: api/Gebruikers
        /// <summary>
        /// Verkrijg alle gebruikers (admin rol)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GebruikerDto>>> GetGebruiker()
        {
            // Dto (data transfer object) toepassen om het wachtwoord veld te excluden van de response
            var Gebruikers = from u in _context.Gebruiker
                             select new GebruikerDto()
                             { Id = u.Id, Voornamen = u.Voornamen, Naam = u.Naam, Actief = u.Actief, Emailadres = u.Emailadres, Role = u.Role };
            
            return await Gebruikers.ToListAsync();
        }

        // GET: api/Gebruikers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GebruikerDto>> GetGebruiker(int id)
        {
            // Dto toepassen om het wachtwoord veld te excluden van de response
            var gebruiker = await _context.Gebruiker.Select(u =>
                new GebruikerDto()
                { Id = u.Id, Voornamen = u.Voornamen, Naam = u.Naam, Actief = u.Actief, Emailadres = u.Emailadres, Role = u.Role })
                .SingleOrDefaultAsync(u => u.Id == id);

            if (gebruiker == null)
            {
                return NotFound();
            }

            return gebruiker;
        }

        // PUT: api/Gebruikers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Wijzig een bestaande gebruiker (admin rol) (let op: geen wachtwoord!)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gebruikerDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGebruiker(int id, GebruikerDto gebruikerDto)
        {
            // Verkrijg gebruiker object en wijzig
            var gebruiker = await _context.Gebruiker.FindAsync(id);
            if (gebruiker == null) return BadRequest();

            gebruiker.Naam = gebruikerDto.Naam;
            gebruiker.Voornamen = gebruikerDto.Voornamen;
            gebruiker.Emailadres = gebruikerDto.Emailadres;
            gebruiker.Role = gebruikerDto.Role;
            gebruiker.Actief = gebruikerDto.Actief;

            // Als gebruiker op actief wordt gezet, reset dan de inlogpogingen
            if (gebruiker.Actief)
                gebruiker.LogingPoging = 0;

            _context.Entry(gebruiker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GebruikerExists(id))
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

        // PUT: api/Gebruikers/wachtwoord/5
        /// <summary>
        /// Wijzig het wachtwoord van een gebruiker (admin rol)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="wachtwoord"></param>
        /// <returns></returns>
        [HttpPut("wachtwoord/{id}")]
        public async Task<ActionResult> UpdateWachtwoord(int id, WachtwoordDto wachtwoord)
        {
            // Verkrijg gebruiker
            var gebruiker = await _context.Gebruiker.FindAsync(id);
            if (gebruiker == null) return BadRequest();

            // Hash en update het nieuwe wachtwoord
            gebruiker.Wachtwoord = BCrypt.Net.BCrypt.HashPassword(wachtwoord.Wachtwoord);

            // Opslaan naar database
            _context.Entry(gebruiker).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Gebruikers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Gebruiker>> PostGebruiker(Gebruiker gebruiker)
        {
            // Hash wachtwoord
            gebruiker.Wachtwoord = BCrypt.Net.BCrypt.HashPassword(gebruiker.Wachtwoord);
            
            _context.Gebruiker.Add(gebruiker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGebruiker", new { id = gebruiker.Id }, gebruiker);
        }

        // DELETE: api/Gebruikers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGebruiker(int id)
        {
            var gebruiker = await _context.Gebruiker.FindAsync(id);
            if (gebruiker == null)
            {
                return NotFound();
            }

            _context.Gebruiker.Remove(gebruiker);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GebruikerExists(int id)
        {
            return _context.Gebruiker.Any(e => e.Id == id);
        }
    }
}
