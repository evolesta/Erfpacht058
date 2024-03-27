using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erfpacht058_API.Data;

namespace Erfpacht058_API.Controllers.Overeenkomst
{
    using Erfpacht058_API.Models.OvereenkomstNS;
    using Microsoft.AspNetCore.Authorization;
    using Erfpacht058_API.Models.Eigendom;

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
            return await _context.Overeenkomst
                .Include(e => e.Financien)
                .Include(e => e.Eigendom)
                .ToListAsync();
        }

        // GET: api/Overeenkomst/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Overeenkomst>> GetOvereenkomst(int id)
        {
            var overeenkomst = await _context.Overeenkomst
                .Include(e => e.Financien)
                .Include(e => e.Eigendom)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (overeenkomst == null)
            {
                return NotFound();
            }

            return overeenkomst;
        }

        // PUT: api/Overeenkomst/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOvereenkomst(int id, OvereenkomstDto overeenkomstDto)
        {
            // Verkrijg overenkomst object
            var overeenkomst = await _context.Overeenkomst
                .Include(e => e.Financien)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (overeenkomst == null) return BadRequest();

            // Wijzig overeenkomst adhv dto
            overeenkomst.Dossiernummer = overeenkomstDto.Dossiernummer;
            overeenkomst.Ingangsdatum = overeenkomstDto.Ingangsdatum;
            overeenkomst.Einddatum = overeenkomstDto.Einddatum;
            overeenkomst.Grondwaarde = overeenkomstDto.Grondwaarde;
            overeenkomst.DatumAkte = overeenkomstDto.DatumAkte;
            overeenkomst.Rentepercentage = overeenkomstDto.Rentepercentage;
            overeenkomst.Financien.Bedrag = overeenkomstDto.Financien.Bedrag;
            overeenkomst.Financien.FactureringsWijze = overeenkomstDto.Financien.FactureringsWijze;
            overeenkomst.Financien.Frequentie = overeenkomstDto.Financien.Frequentie;

            // Sla wijzigingen op in database
            _context.Entry(overeenkomst).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(overeenkomst);
        }

        // DELETE: api/Overeenkomst/5
        /// <summary>
        /// Verwijder een bestaande overeenkomst en relaties
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Eigendom>> DeleteOvereenkomst(int id)
        {
            // verkrijg benodigde objecten
            var overeenkomst = await _context.Overeenkomst
                .Include(e => e.Eigendom)
                .Include(e => e.Financien)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (overeenkomst == null) return BadRequest();
            var eigendom = overeenkomst.Eigendom;
            if (eigendom == null) return BadRequest();
            var financien = overeenkomst.Financien;

            // Verwijder relaties en object en sla op
            eigendom.Overeenkomst.Remove(overeenkomst);
            _context.Overeenkomst.Remove(overeenkomst);
            _context.Financien.Remove(financien);
            _context.Entry(eigendom).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(eigendom);
        }
    }
}
