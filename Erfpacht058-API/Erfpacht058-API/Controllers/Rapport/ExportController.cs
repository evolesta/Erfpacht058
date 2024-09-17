using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Rapport;
using Erfpacht058_API.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Reflection;
using Erfpacht058_API.Services;

namespace Erfpacht058_API.Controllers.Rapport
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly TaskQueueHostedService _taskQueueHostedService;

        public ExportController(Erfpacht058_APIContext context, TaskQueueHostedService taskQueueHostedService)
        {
            _context = context;
            _taskQueueHostedService = taskQueueHostedService;
        }

        // GET: api/Export
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Export>>> GetExport()
        {
            return await _context.Export
                .Include(e => e.Task)
                .Include(e => e.Template)
                .ToListAsync();
        }

        // GET: api/Export/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Export>> GetExport(int id)
        {
            var export = await _context.Export
                .Include(e => e.Task)
                .Include(e => e.Template)
                .FirstOrDefaultAsync(e  => e.Id == id); 

            if (export == null)
            {
                return NotFound();
            }

            return export;
        }

        // POST: api/Export
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Export>> PostExport(ExportDto exportDto)
        {
            // Verkrijg huidige gebruiker
            var username = User.Claims.FirstOrDefault(user => user.Type == "Username")?.Value;
            var user = await _context.Gebruiker.FirstOrDefaultAsync(u => u.Emailadres == username);
            var template = await _context.Template.FindAsync(exportDto.TemplateId);
            if (template == null) return BadRequest();
            
            // Maak een nieuwe Export Taak aan
            var export = new Export
            {
                Formaat = exportDto.Formaat,
                Template = template,
                AanmaakDatum = DateTime.Now,
                Gebruiker = user
            };

            // Maak een nieuwe Taak aan
            var task = new TaskQueue
            {
                SoortTaak = SoortTaak.Export,
                Status = Status.Nieuw,
                Prioriteit = Prioriteit.Midden,
                AanmaakDatum = DateTime.Now,
                Export = export
            };

            export.Task = task; // Update Export relatie Taak

            _context.Export.Add(export);
            _context.TaskQueue.Add(task);

            await _context.SaveChangesAsync();

            // Voeg Task toe aan queue en voer taak uit
            _taskQueueHostedService.EnqueueTask(task);

            return Ok(export);
        }

        // GET: /api/Export/download/5
        /// <summary>
        /// Download een voltooide Export
        /// </summary>
        /// <param name="id"></param>
        /// <returns>octet-stream</returns>
        [HttpGet("download/{id}")]
        public async Task<ActionResult> DownloadExport(int id)
        {
            // Verkrijg export object van DB
            var export = await _context.Export
                .Include(e => e.Task)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (export == null) return BadRequest();

            // Check of Taak succesvol is
            if (export.Task.Status != Status.Succesvol)
                return BadRequest("Export nog niet gereed of mislukt");

            // Verkrijg bestandspad
            var filepath = export.ExportPad;
            if (!System.IO.File.Exists(filepath)) return BadRequest();

            // Converteer het bestand naar bytes om als response mee te sturen
            var fileBytes = await System.IO.File.ReadAllBytesAsync(filepath);

            // Zet content-Disposition om een download te forceren in de response
            var contentDisposition = new System.Net.Mime.ContentDisposition
            {
                FileName = Path.GetFileName(filepath),
                Inline = false
            };
            Response.Headers.Add("Content-Disposition", contentDisposition.ToString()); // voeg toe aan headers

            // Geef de byte array terug als download (octet-stream)
            return File(fileBytes, "application/octet-stream");
        }

        // GET: api/Export/status/5
        /// <summary>
        /// Verkrijg de status van een export verzoek
        /// </summary>
        /// <remarks>
        /// Status:
        ///     0 = Nieuw
        ///     1 = In Behandeling
        ///     2 = Succesvol
        ///     3 = Mislukt
        ///     4 = Verwijderd
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("status/{id}")]
        public async Task<ActionResult> GetStatus(int id)
        {
            // Verkrijg export object van DB
            var export = await _context.Export
                .Include(e => e.Task)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (export == null) return BadRequest();

            // Status response opstellen
            return Ok(export.Task);
        }
    }
}
