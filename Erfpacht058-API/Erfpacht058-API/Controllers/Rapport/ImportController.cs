using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Rapport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Erfpacht058_API.Controllers.Rapport
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Beheerder")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly TaskQueueHostedService _taskQueueHostedService;
        private readonly IConfiguration _configuration;

        public ImportController(Erfpacht058_APIContext context, TaskQueueHostedService taskQueueHostedService, IConfiguration configuration)
        {
            _context = context;
            _taskQueueHostedService = taskQueueHostedService;
            _configuration = configuration;
        }

        // POST: api/import
        /// <summary>
        /// Importeer een CSV naar de applicatie
        /// </summary>
        /// <param name="csv"></param>
        /// <param name="importDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> NewImport(IFormFile csvFile, ImportDto importDto)
        {
            // Verkrijg huidige gebruiker en vertaaltabel
            var username = User.Claims.FirstOrDefault(user => user.Type == "Username")?.Value;
            var user = await _context.Gebruiker.FirstOrDefaultAsync(u => u.Emailadres == username);

            var translationTabel = await _context.TranslateModel.FindAsync(importDto.TranslateModel);
            if (translationTabel == null) return BadRequest();

            // CSV bestand verwerken naar lokale storage
            var filepath = _configuration["Bestanden:ImportPad"] + "/Import-CSV-" + BCrypt.Net.BCrypt.HashString(new Random().Next(1, 999999999).ToString()) + ".csv";
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
