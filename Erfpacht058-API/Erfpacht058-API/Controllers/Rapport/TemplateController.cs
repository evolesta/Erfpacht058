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
using Erfpacht058_API.Repositories.Interfaces;

namespace Erfpacht058_API.Controllers.Rapport
{
    [Authorize(Roles = "Beheerder")]
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateRepository _templateRepo;

        public TemplateController(ITemplateRepository templateRepository)
        {
            _templateRepo = templateRepository;
        }

        // GET: api/Template
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Template>>> GetTemplate()
        {
            return Ok(await _templateRepo.GetTemplates());
        }

        // GET: api/Template/5
        /// <summary>
        /// Verkrijg een Template object met relaties
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Template>> GetTemplate(int id)
        {
            var result = await _templateRepo.GetTemplate(id);

            if (result != null)
                return result;
            else return NotFound();
        }

        // PUT: api/Template/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTemplate(int id, TemplateDto templateDto)
        {
            var result = _templateRepo.EditTemplate(id, templateDto);

            if (result != null) return Ok(result);
            else return NotFound();
        }

        // POST: api/Template
        /// <summary>
        /// Voeg een nieuwe Template toe (beheerdersrol)
        /// </summary>
        /// <param name="templateDto"></param>
        /// <returns></returns>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Template>> PostTemplate(TemplateDto templateDto)
        {
            return await _templateRepo.AddTemplate(templateDto);
        }

        // DELETE: api/Template/5
        /// <summary>
        /// Verwijder een Template met onderliggende RapportData
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemplate(int id)
        {
            var result = _templateRepo.DeleteTemplate(id);

            if (result != null) return NoContent();
            else return NotFound();
        }

        // GET /api/Template/models
        /// <summary>
        /// Verkrijg alle datastructuur voor het opstellen van een sjabloon (beheerder)
        /// </summary>
        /// <returns></returns>
        [HttpGet("models")]
        public async Task<ActionResult> GetModelsStructure()
        {
            return Ok(_templateRepo.GetModelsStructure());
        }
    }
}
