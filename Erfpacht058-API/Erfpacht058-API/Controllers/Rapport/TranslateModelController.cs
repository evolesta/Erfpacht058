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
    [Route("api/[controller]")]
    [Authorize(Roles = "Beheerder")]
    [ApiController]
    public class TranslateModelController : ControllerBase
    {
        private readonly ITranslateModelRepository _translationsRepo;

        public TranslateModelController(ITranslateModelRepository translationsRepository)
        {
            _translationsRepo = translationsRepository;
        }

        // GET: api/TranslateModel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TranslateModel>>> GetTranslateModel()
        {
            return Ok(await _translationsRepo.GetTranslateModels());
        }

        // GET: api/TranslateModel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TranslateModel>> GetTranslateModel(int id)
        {
            var result = _translationsRepo.GetTranslateModel(id);

            if (result != null)
                return Ok(result);
            else return NotFound();
        }

        // PUT: api/TranslateModel/5
        /// <summary>
        /// Wijzig een bestaande vertaaltabel inclusief onderliggende vertalingen
        /// </summary>
        /// <param name="id"></param>
        /// <param name="translateModel"></param>
        /// <returns></returns>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTranslateModel(int id, TranslateModelDto translateModelDto)
        {
            var result = _translationsRepo.EditTranslateModel(id, translateModelDto);

            if (result != null) return Ok(result);
            else return NotFound();
        }

        // POST: api/TranslateModel
        /// <summary>
        /// Voeg een nieuwe vertaaltabel toe
        /// </summary>
        /// <param name="translateModelDto"></param>
        /// <returns></returns>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TranslateModel>> PostTranslateModel(TranslateModelDto translateModelDto)
        {
            return Ok(await _translationsRepo.AddTranslateModel(translateModelDto));
        }

        // DELETE: api/TranslateModel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTranslateModel(int id)
        {
            var result = await _translationsRepo.DeleteTranslateModel(id);

            if (result != null) return NoContent();
            else return NotFound();
        }
    }
}
