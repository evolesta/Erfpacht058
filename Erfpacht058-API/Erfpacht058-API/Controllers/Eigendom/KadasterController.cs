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
    public class KadasterController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;

        public KadasterController(Erfpacht058_APIContext context)
        {
            _context = context;
        }

        // GET: api/Kadaster/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Kadaster>> GetKadaster(int id)
        {
            var kadaster = await _context.Kadaster.FindAsync(id);

            if (kadaster == null)
            {
                return NotFound();
            }

            return kadaster;
        }

        // Todo: SYNC bestaande Kadaster

        // Todo: Nieuwe Kadaster Sync
        // POST: api/Kadaster
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Kadaster>> PostKadaster(Kadaster kadaster)
        {
            _context.Kadaster.Add(kadaster);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKadaster", new { id = kadaster.Id }, kadaster);
        }

        // DELETE: api/Kadaster/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKadaster(int id)
        {
            var kadaster = await _context.Kadaster.FindAsync(id);
            if (kadaster == null)
            {
                return NotFound();
            }

            _context.Kadaster.Remove(kadaster);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KadasterExists(int id)
        {
            return _context.Kadaster.Any(e => e.Id == id);
        }
    }
}
