using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Facturen;
using Erfpacht058_API.Models.Rapport;
using System.IO.Compression;
using Microsoft.AspNetCore.Authorization;

namespace Erfpacht058_API.Controllers.Facturen
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class FactuurJobController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly TaskQueueHostedService _taskQueueHostedService;

        public FactuurJobController(Erfpacht058_APIContext context, TaskQueueHostedService taskQueueHostedService)
        {
            _context = context;
            _taskQueueHostedService = taskQueueHostedService;
        }

        // GET: api/FactuurJob
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FactuurJob>>> GetFactuurJob()
        {
            return await _context.FactuurJob
                .Include(e=> e.Task)
                .ToListAsync();
        }

        // GET: api/FactuurJob/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FactuurJob>> GetFactuurJob(int id)
        {
            var factuurJob = await _context.FactuurJob
                .Include(e=> e.Task)
                .Include(e=> e.Facturen)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (factuurJob == null) return BadRequest();

           return factuurJob;
        }

        // POST: api/FactuurJob
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Creeer een nieuwe facturatie taak
        /// </summary>
        /// <param name="factuurJob"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<FactuurJob>> PostFactuurJob(FactuurJobDto factuurJobDto)
        {
            // Nieuwe facturatie taak aanmaken
            var factuurJob = new FactuurJob
            {
                AanmaakDatum = DateTime.Now,
                FactureringsPeriode = factuurJobDto.FactureringsPeriode,
                StoragePad = "",
            };

            // Nieuwe taak aanmaken
            var task = new TaskQueue
            {
                AanmaakDatum = DateTime.Now,
                FactuurJob = factuurJob,
                Prioriteit = Prioriteit.Midden,
                SoortTaak = SoortTaak.Facturen,
                Status = Status.Nieuw,
            };

            // Verkrijg huidige gebruiker
            var username = User.Claims
                .FirstOrDefault(user => user.Type == "Username")?.Value;
            var user = await _context.Gebruiker
                .FirstOrDefaultAsync(u => u.Emailadres == username);

            // Relaties leggen en entities toevoegen aan context
            factuurJob.Gebruiker = user;
            factuurJob.Task = task;

            _context.TaskQueue.Add(task);
            _context.FactuurJob.Add(factuurJob);

            await _context.SaveChangesAsync();

            // Start taak in background service
            _taskQueueHostedService.EnqueueTask(task);

            return Ok(factuurJob);
        }

        // GET: /factuurJob/export/5
        /// <summary>
        /// Exporteer de gegenereerde facturen
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("download/{id}")]
        public async Task<ActionResult> ExportInvoices(int id)
        {
            // Verkrijg FactuurJob
            var factuurJob = await _context.FactuurJob
                .Include(e => e.Task)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (factuurJob == null) return BadRequest();

            // Check of taak succesvol is
            if (factuurJob.Task.Status != Status.Succesvol) return BadRequest("Export nog niet gereed of mislukt");

            // Download genereren
            var filepath = factuurJob.StoragePad;
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
    }
}
 