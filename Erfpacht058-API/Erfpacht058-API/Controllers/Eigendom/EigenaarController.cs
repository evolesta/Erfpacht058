using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Eigendom;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Authorization;

namespace Erfpacht058_API.Controllers.Eigendom
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EigenaarController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;

        public EigenaarController(Erfpacht058_APIContext context)
        {
            _context = context;
        }

        // GET: api/Eigenaar
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Eigenaar>>> GetEigenaar()
        {
            return await _context.Eigenaar.ToListAsync();
        }

        // GET: api/Eigenaar/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Eigenaar>> GetEigenaar(int id)
        {
            var eigenaar = await _context.Eigenaar.FindAsync(id);

            if (eigenaar == null)
            {
                return NotFound();
            }

            return eigenaar;
        }

        // POST: api/Eigenaar
        /// <summary>
        /// Voeg een nieuwe eigenaar toe
        /// </summary>
        /// <param name="id"></param>
        /// <param name="eigenaarDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Eigenaar>> PostEigenaar(EigenaarDto eigenaarDto)
        {
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
                Einddatum = eigenaarDto.Einddatum
            };

            _context.Eigenaar.Add(eigenaar);
            await _context.SaveChangesAsync();

            return Ok(eigenaar);
        }

        // PUT: api/Eigenaar/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Eigenaar>> PutEigenaar(int id, EigenaarDto eigenaarDto)
        {
            // Verkrijg eigenaar object van context en wijzig de attributen uit de dto
            var eigenaar = await _context.Eigenaar.FindAsync(id);
            if (eigenaar == null)
                return BadRequest();

            eigenaar.Naam = eigenaarDto.Naam;
            eigenaar.Voornamen = eigenaarDto.Voornamen;
            eigenaar.Voorletters = eigenaarDto.Voorletters;
            eigenaar.Straatnaam = eigenaarDto.Straatnaam;
            eigenaar.Huisnummer = eigenaarDto.Huisnummer;
            eigenaar.Toevoeging = eigenaarDto.Toevoeging;
            eigenaar.Postcode = eigenaarDto.Postcode;
            eigenaar.Woonplaats = eigenaarDto.Woonplaats;
            eigenaar.Debiteurnummer = eigenaarDto.Debiteurnummer;
            eigenaar.Ingangsdatum = eigenaarDto.Ingangsdatum;
            eigenaar.Einddatum = eigenaarDto.Einddatum;

            // Wijzigingen opslaan naar database
            _context.Entry(eigenaar).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(eigenaar);
        }

        // DELETE: api/Eigenaar/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEigenaar(int id)
        {
            var eigenaar = await _context.Eigenaar.FindAsync(id);
            if (eigenaar == null)
            {
                return NotFound();
            }

            _context.Eigenaar.Remove(eigenaar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EigenaarExists(int id)
        {
            return _context.Eigenaar.Any(e => e.Id == id);
        }
    }
}
