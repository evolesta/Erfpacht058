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

        // PUT: api/Eigenaar/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEigenaar(int id, Eigenaar eigenaar)
        {
            if (id != eigenaar.Id)
            {
                return BadRequest();
            }

            _context.Entry(eigenaar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EigenaarExists(id))
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

        // POST: api/Eigenaar
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Eigenaar>> PostEigenaar(Eigenaar eigenaar)
        {
            _context.Eigenaar.Add(eigenaar);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEigenaar", new { id = eigenaar.Id }, eigenaar);
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
