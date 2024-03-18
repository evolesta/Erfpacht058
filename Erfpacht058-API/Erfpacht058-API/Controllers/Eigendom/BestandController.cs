using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models;

namespace Erfpacht058_API.Controllers.Eigendom
{
    [Route("api/[controller]")]
    [ApiController]
    public class BestandController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;

        public BestandController(Erfpacht058_APIContext context)
        {
            _context = context;
        }

        // GET: api/Bestand
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bestand>>> GetBestand()
        {
            return await _context.Bestand.ToListAsync();
        }

        // GET: api/Bestand/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bestand>> GetBestand(int id)
        {
            var bestand = await _context.Bestand.FindAsync(id);

            if (bestand == null)
            {
                return NotFound();
            }

            return bestand;
        }

        // PUT: api/Bestand/5
        /// <summary>
        /// Wijzig een bestaand bestand
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bestandDto"></param>
        /// <returns></returns> 
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutBestand(int id, BestandDto bestandDto)
        {
            var bestand = await _context.Bestand.FindAsync(id);
            if (bestand == null) return BadRequest();

            bestand.Naam = bestandDto.Naam;
            bestand.Beschrijving = bestandDto.Beschrijving;
            bestand.SoortBestand = bestandDto.SoortBestand;
            
            _context.Entry(bestand).State = EntityState.Modified; 
            await _context.SaveChangesAsync(); 

            return Ok(bestand);
        }

        // DELETE: api/Bestand/5
        /// <summary>
        /// Verwijder een bestand
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> */
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBestand(int id)
        {
            var bestand = await _context.Bestand.FindAsync(id);
            if (bestand == null) return NotFound();
            var eigendom = bestand.Eigendom;

            // Verwijder bestand uit storage
            var filepath = bestand.Pad;
            System.IO.File.Delete(filepath);

            // Verwijder relaties en verwerk naar database
            eigendom.Bestand.Remove(bestand);
            _context.Bestand.Remove(bestand);
            _context.Entry(bestand).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
