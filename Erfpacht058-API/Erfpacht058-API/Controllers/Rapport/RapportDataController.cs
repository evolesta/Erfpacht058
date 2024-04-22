using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Rapport;
using Microsoft.AspNetCore.Authorization;

namespace Erfpacht058_API.Controllers.Rapport
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class RapportDataController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;

        public RapportDataController(Erfpacht058_APIContext context)
        {
            _context = context;
        }

        // GET: api/RapportData
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RapportData>>> GetRapportData()
        {
            return await _context.RapportData.ToListAsync();
        }

        // GET: api/RapportData/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RapportData>> GetRapportData(int id)
        {
            var rapportData = await _context.RapportData.FindAsync(id);

            if (rapportData == null)
            {
                return NotFound();
            }

            return rapportData;
        }

        // PUT: api/RapportData/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRapportData(int id, RapportDataDto rapportDataDto)
        {
            // verkrijg object
            var rapportData = await _context.RapportData.FindAsync(id);
            if (rapportData == null) return BadRequest();

            // Wijzig object
            rapportData.Key = rapportDataDto.Key;
            rapportData.Naam = rapportDataDto.Naam;

            // Sla op in database
            _context.Entry(rapportData).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(rapportData);
        }

        // DELETE: api/RapportData/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRapportData(int id)
        {
            // Verkrijg rapportData object
            var rapportData = await _context.RapportData
                .Include(x => x.Template)
                .FirstOrDefaultAsync(x => x.Id == id);
            var template = rapportData.Template;
            if (rapportData == null) return BadRequest();

            // Verwijder relatie uit bovenstaand object en sla op in database
            template.RapportData.Remove(rapportData);
            _context.RapportData.Remove(rapportData);
            _context.Entry(template).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
