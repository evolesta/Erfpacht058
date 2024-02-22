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
    public class HerzieningController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;

        public HerzieningController(Erfpacht058_APIContext context)
        {
            _context = context;
        }

        // GET: api/Herziening
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Herziening>>> GetHerziening()
        {
            return await _context.Herziening.ToListAsync();
        }

        // GET: api/Herziening/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Herziening>> GetHerziening(int id)
        {
            var herziening = await _context.Herziening.FindAsync(id);

            if (herziening == null)
            {
                return NotFound();
            }

            return herziening;
        }

        // PUT: api/Herziening/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHerziening(int id, Herziening herziening)
        {
            if (id != herziening.Id)
            {
                return BadRequest();
            }

            _context.Entry(herziening).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HerzieningExists(id))
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

        // POST: api/Herziening
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Herziening>> PostHerziening(Herziening herziening)
        {
            _context.Herziening.Add(herziening);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHerziening", new { id = herziening.Id }, herziening);
        }

        // DELETE: api/Herziening/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHerziening(int id)
        {
            var herziening = await _context.Herziening.FindAsync(id);
            if (herziening == null)
            {
                return NotFound();
            }

            _context.Herziening.Remove(herziening);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HerzieningExists(int id)
        {
            return _context.Herziening.Any(e => e.Id == id);
        }
    }
}
