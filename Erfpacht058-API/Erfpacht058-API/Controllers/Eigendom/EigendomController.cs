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
        public async Task<IActionResult> PutEigendom(int id, Eigendom eigendom)
        {
            if (id != eigendom.Id)
            {
                return BadRequest();
            }

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
        [HttpPost]
        public async Task<ActionResult<Eigendom>> PostEigendom(Eigendom eigendom)
        {
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
    }
}
