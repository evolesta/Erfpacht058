using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Overeenkomst;
using Microsoft.AspNetCore.Authorization;

namespace Erfpacht058_API.Controllers.Overeenkomst
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class FinancienController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;

        public FinancienController(Erfpacht058_APIContext context)
        {
            _context = context;
        }

        // GET: api/Financien
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Financien>>> GetFinancien()
        {
            return await _context.Financien.ToListAsync();
        }

        // GET: api/Financien/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Financien>> GetFinancien(int id)
        {
            var financien = await _context.Financien.FindAsync(id);

            if (financien == null)
            {
                return NotFound();
            }

            return financien;
        }

        // PUT: api/Financien/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFinancien(int id, Financien financien)
        {
            if (id != financien.Id)
            {
                return BadRequest();
            }

            _context.Entry(financien).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinancienExists(id))
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

        // POST: api/Financien
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Financien>> PostFinancien(Financien financien)
        {
            _context.Financien.Add(financien);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFinancien", new { id = financien.Id }, financien);
        }

        // DELETE: api/Financien/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFinancien(int id)
        {
            var financien = await _context.Financien.FindAsync(id);
            if (financien == null)
            {
                return NotFound();
            }

            _context.Financien.Remove(financien);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FinancienExists(int id)
        {
            return _context.Financien.Any(e => e.Id == id);
        }
    }
}
