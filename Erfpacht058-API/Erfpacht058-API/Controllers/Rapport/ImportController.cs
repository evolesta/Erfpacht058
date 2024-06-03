using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Rapport;

namespace Erfpacht058_API.Controllers.Rapport
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly IConfiguration _configuration;
        private readonly TaskQueueHostedService _taskQueueHostedService;

        public ImportController(Erfpacht058_APIContext context, IConfiguration configuration, TaskQueueHostedService taskQueueHostedService)
        {
            _context = context;
            _configuration = configuration;
            _taskQueueHostedService = taskQueueHostedService;
        }

        // GET: api/Import
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Import>>> GetImport()
        {
            return await _context.Import
                .Include(e => e.Task)
                .Include(e => e.TranslateModel)
                .ToListAsync();
        }

        // GET: api/Import/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Import>> GetImport(int id)
        {
            var import = await _context.Import
                .Include(x => x.Task)
                .Include(x => x.TranslateModel)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (import == null) return BadRequest();

            return Ok(import);
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
            var user = await _context.Gebruiker
                .FirstOrDefaultAsync(u => u.Emailadres == username);

            var translationTabel = await _context.TranslateModel
                .FirstOrDefaultAsync(x => x.Id == translateModelId);
            if (translationTabel == null) return BadRequest();

            // CSV bestand verwerken naar lokale storage
            var filepath = _configuration["Bestanden:ImportPad"] + "/Import-CSV-" + (new Random().Next(1, 999999999).ToString()) + ".csv";
            using (var stream = System.IO.File.Create(filepath))
            {
                await csvFile.CopyToAsync(stream);
            }

            // Nieuw import entry maken
            var import = new Import
            {
                Aanmaakdatum = DateTime.Now,
                Gebruiker = user,
                TranslateModel = translationTabel,
                importPad = filepath
            };

            // Nieuwe taak aanmaken
            var task = new TaskQueue
            {
                SoortTaak = SoortTaak.Import,
                AanmaakDatum = DateTime.Now,
                Status = Status.Nieuw,
                Prioriteit = Prioriteit.Midden,
                Import = import
            };

            // Relaties leggen en entrys toevoegen aan context
            import.Task = task;
            _context.Import.Add(import);
            _context.TaskQueue.Add(task);
            await _context.SaveChangesAsync();

            // Nieuwe taak toevoegen naar background worker
            _taskQueueHostedService.EnqueueTask(task);

            return Ok(import);

        }
    }
}
