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
using Erfpacht058_API.Repositories.Interfaces;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using System.Composition;

namespace Erfpacht058_API.Controllers.Rapport
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly TaskQueueHostedService _taskQueueHostedService;
        private readonly IExportRepository _exportRepo;
        private readonly IGebruikerRepository _gebruikerRepo;

        public ExportController(TaskQueueHostedService taskQueueHostedService, IExportRepository exportRepository, IGebruikerRepository gebruikerRepo)
        {
            _taskQueueHostedService = taskQueueHostedService;
            _exportRepo = exportRepository;
            _gebruikerRepo = gebruikerRepo;
        }

        // GET: api/Export
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Export>>> GetExport()
        {
            return Ok(await _exportRepo.GetExports());
        }

        // GET: api/Export/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Export>> GetExport(int id)
        {
            var result = await _exportRepo.GetExport(id);

            if(result != null) return Ok(result);
            else return NotFound();
        }

        // POST: api/Export
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Export>> PostExport(ExportDto exportDto)
        {
            // Verkrijg huidige gebruiker
            var username = User.Claims.FirstOrDefault(user => user.Type == "Username")?.Value;
            var user = await _gebruikerRepo.ZoekGebruiker(username);
            
            // Voer export op
            var result = await _exportRepo.AddExport(exportDto, user);

            if (result != null)
            {
                // Voeg Task toe aan queue en voer taak uit
                _taskQueueHostedService.EnqueueTask(result.Task);

                return Ok(result);
            }
            else
                return NotFound();
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
            var export = await _exportRepo.GetExport(id);

            if (export == null) 
                return NotFound();

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
            var export = await _exportRepo.GetExport(id);

            if (export == null) 
                return NotFound();

            // Status response opstellen
            return Ok(export.Task);
        }
    }
}
