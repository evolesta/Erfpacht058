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
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AdresController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;

        public AdresController(Erfpacht058_APIContext context)
        {
            _context = context;
        }

        // GET: api/Adres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Adres>>> GetAdres()
        {
            return await _context.Adres.ToListAsync();
        }

        // GET: api/Adres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Adres>> GetAdres(int id)
        {
            var adres = await _context.Adres.FindAsync(id);

            if (adres == null)
            {
                return NotFound();
            }

            return adres;
        }

        // PUT: api/Adres/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdres(int id, Adres adres)
        {
            if (id != adres.Id)
            {
                return BadRequest();
            }

            _context.Entry(adres).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdresExists(id))
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

        // POST: api/Adres
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Adres>> PostAdres(Adres adres)
        {
            _context.Adres.Add(adres);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdres", new { id = adres.Id }, adres);
        }

        // DELETE: api/Adres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdres(int id)
        {
            var adres = await _context.Adres.FindAsync(id);
            if (adres == null)
            {
                return NotFound();
            }

            _context.Adres.Remove(adres);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdresExists(int id)
        {
            return _context.Adres.Any(e => e.Id == id);
        }
    }
}
