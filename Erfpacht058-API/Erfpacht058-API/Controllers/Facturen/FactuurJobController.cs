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
using Erfpacht058_API.Services;
using Erfpacht058_API.Repositories.Interfaces;

namespace Erfpacht058_API.Controllers.Facturen
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class FactuurJobController : ControllerBase
    {
        private readonly TaskQueueHostedService _taskQueueHostedService;
        private readonly IFactuurJobRepository _factuurJobRepo;
        private readonly IGebruikerRepository _gebruikerRepo;

        public FactuurJobController(TaskQueueHostedService taskQueueHostedService, IFactuurJobRepository factuurJobRepository, 
            IGebruikerRepository gebruikerRepo)
        {
            _taskQueueHostedService = taskQueueHostedService;
            _factuurJobRepo = factuurJobRepository;
            _gebruikerRepo = gebruikerRepo;
        }

        // GET: api/FactuurJob
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FactuurJob>>> GetFactuurJob()
        {
            return Ok(await _factuurJobRepo.GetFactuurJobs());
        }

        // GET: api/FactuurJob/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FactuurJob>> GetFactuurJob(int id)
        {
            var result = await _factuurJobRepo.GetFactuurJob(id);

            if (result != null) return Ok(result);
            else return NotFound();
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
            // Verkrijg huidige gebruiker
            var username = User.Claims
                .FirstOrDefault(user => user.Type == "Username")?.Value;
            var gebruiker = await _gebruikerRepo.ZoekGebruiker(username);

            var result = await _factuurJobRepo.AddFactuurJob(factuurJobDto, gebruiker);

            // Start taak in background service
            _taskQueueHostedService.EnqueueTask(result.Task);

            return Ok(result);
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
            var factuurJob = await _factuurJobRepo.GetFactuurJob(id);

            if (factuurJob == null) 
                return NotFound();

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
 