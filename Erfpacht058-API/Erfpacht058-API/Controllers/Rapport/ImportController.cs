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
using Erfpacht058_API.Services;
using Erfpacht058_API.Repositories.Interfaces;

namespace Erfpacht058_API.Controllers.Rapport
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly TaskQueueHostedService _taskQueueHostedService;
        private readonly IImportRepository _importRepo;
        private readonly IGebruikerRepository _gebruikerRepo;

        public ImportController(IConfiguration configuration, TaskQueueHostedService taskQueueHostedService, 
            IImportRepository importRepository, IGebruikerRepository gebruikerRepo)
        {
            _configuration = configuration;
            _taskQueueHostedService = taskQueueHostedService;
            _importRepo = importRepository;
            _gebruikerRepo = gebruikerRepo;
        }

        // GET: api/Import
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Import>>> GetImport()
        {
            return Ok(await _importRepo.GetImports());
        }

        // GET: api/Import/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Import>> GetImport(int id)
        {
            var result = await _importRepo.GetImport(id);

            if (result == null) return Ok(result);
            else return NotFound();
        }

        // POST: api/Import
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Voeg een nieuwe import taak toe (upload een CSV)
        /// </summary>
        /// <param name="csvFile"></param>
        /// <param name="importDto"></param>
        /// <returns></returns>
        [HttpPost("{translateModelId}")]
        public async Task<ActionResult<Import>> PostImport(IFormFile csvFile, int translateModelId)
        {
            // Verkrijg huidige gebruiker en vertaaltabel
            var username = User.Claims
                .FirstOrDefault(user => user.Type == "Username")?.Value;
            var user = await _gebruikerRepo.ZoekGebruiker(username);            

            // CSV bestand verwerken naar lokale storage
            var filepath = _configuration["Bestanden:ImportPad"] + "/Import-CSV-" + (new Random().Next(1, 999999999).ToString()) + ".csv";
            using (var stream = System.IO.File.Create(filepath))
            {
                await csvFile.CopyToAsync(stream);
            }

            var result = await _importRepo.AddImport(translateModelId, user, filepath);

            if (result != null)
            {
                // Nieuwe taak toevoegen naar background worker
                _taskQueueHostedService.EnqueueTask(result.Task);

                return Ok(result);
            }
            else
                return NotFound();
        }
    }
}
