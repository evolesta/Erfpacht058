using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Overeenkomst;

namespace Erfpacht058_API.Controllers.Overeenkomst
{
    using Erfpacht058_API.Models.Overeenkomst;
    using Microsoft.AspNetCore.Authorization;

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OvereenkomstController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;

        public OvereenkomstController(Erfpacht058_APIContext context)
        {
            _context = context;
        }

        // GET: api/Overeenkomst
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Overeenkomst>>> GetOvereenkomst()
        {
            return await _context.Overeenkomst.ToListAsync();
        }

        // GET: api/Overeenkomst/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Overeenkomst>> GetOvereenkomst(int id)
        {
            var overeenkomst = await _context.Overeenkomst.FindAsync(id);

            if (overeenkomst == null)
            {
                return NotFound();
            }

            return overeenkomst;
        }

        // PUT: api/Overeenkomst/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOvereenkomst(int id, Overeenkomst overeenkomst)
        {
            if (id != overeenkomst.Id)
            {
                return BadRequest();
            }

            _context.Entry(overeenkomst).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OvereenkomstExists(id))
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

        // POST: api/Overeenkomst
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Overeenkomst>> PostOvereenkomst(Overeenkomst overeenkomst)
        {
            _context.Overeenkomst.Add(overeenkomst);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOvereenkomst", new { id = overeenkomst.Id }, overeenkomst);
        }

        // DELETE: api/Overeenkomst/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOvereenkomst(int id)
        {
            var overeenkomst = await _context.Overeenkomst.FindAsync(id);
            if (overeenkomst == null)
            {
                return NotFound();
            }

            _context.Overeenkomst.Remove(overeenkomst);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OvereenkomstExists(int id)
        {
            return _context.Overeenkomst.Any(e => e.Id == id);
        }
    }
}
