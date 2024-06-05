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

namespace Erfpacht058_API.Controllers.Rapport
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Beheerder")]
    [ApiController]
    public class TranslateModelController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;

        public TranslateModelController(Erfpacht058_APIContext context)
        {
            _context = context;
        }

        // GET: api/TranslateModel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TranslateModel>>> GetTranslateModel()
        {
            return await _context.TranslateModel.ToListAsync();
        }

        // GET: api/TranslateModel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TranslateModel>> GetTranslateModel(int id)
        {
            // Verkrijg Vertaaltabellen inclusief vertalingen
            var translateModel = await _context.TranslateModel
                .Include(e => e.Translations)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (translateModel == null)
            {
                return BadRequest();
            }

            return translateModel;
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
            // Verkrijg vertaaltabel
            var translateModel = await _context.TranslateModel
                .Include(e => e.Translations)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (translateModel == null) return BadRequest();

            translateModel.Maker = translateModelDto.Maker;
            translateModel.Model = translateModelDto.Model;
            translateModel.WijzigingsDatum = DateTime.Now;
            translateModel.Naam = translateModelDto.Naam;

            // Vernieuw eventueel de vertalingen
            foreach (var translation in translateModelDto.Translations)
            {
                // Verkrijg het bestaande record
                var existingTranslation = translateModel.Translations
                    .FirstOrDefault(ts => ts.Id == translation.Id);

                if (existingTranslation == null)
                {
                    // Record niet gevonden, nieuwe aanmaken
                    var newTranslation = new Translation
                    {
                        CSVColummnName = translation.CSVColummnName,
                        ModelColumnName = translation.ModelColumnName,
                        TranslateModel = translateModel
                    };

                    _context.Translation.Add(newTranslation);
                    translateModel.Translations.Add(newTranslation);
                }
                else
                {
                    // bestaand record bijwerken
                    existingTranslation.CSVColummnName = translation.CSVColummnName;
                    existingTranslation.ModelColumnName = translation.ModelColumnName;

                    _context.Entry(existingTranslation).State = EntityState.Modified;
                }

                // Opslaan in context
                await _context.SaveChangesAsync();
            }

            // Verwijder records die niet meer in de json body payload aanwezig zijn, maar nog wel in de database
            var translationsIdsInDto = translateModelDto.Translations
                .Select(t => t.Id)
                .ToList(); // Verkrijg alle IDS vanuit de payload van het verzoek
            // Verkrijg de IDS die nog wel in de database staan, maar niet meer in de payload (te verwijderen IDS)
            var translationsToRemove = translateModel.Translations
                .Where(t => !translationsIdsInDto.Contains(0) && !translationsIdsInDto.Contains(t.Id))
                .ToList();
            // Verwijder alle IDS die net meer nodig zijn
            foreach (var idToRemove in translationsToRemove)
            {
                _context.Translation.Remove(idToRemove);
                translateModel.Translations.Remove(idToRemove);
            }

            // Wijzigingen opslaan in context
            _context.Entry(translateModel).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(translateModel);
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
            // Maak een nieuwe vertaaltabel
            var translateModel = new TranslateModel
            {
                Maker = translateModelDto.Maker,
                Model = translateModelDto.Model,
                AanmaakDatum = DateTime.Now,
                Naam = translateModelDto.Naam
            };

            // Doorloop iedere vertaling en maak een nieuw object
            foreach (var translation in translateModelDto.Translations)
            {
                var newTranslation = new Translation
                {
                    CSVColummnName = translation.CSVColummnName,
                    ModelColumnName = translation.ModelColumnName,
                    TranslateModel = translateModel
                };

                _context.Translation.Add(newTranslation);
                translateModel.Translations.Add(newTranslation);
            }

            // Voeg toe aan context
            _context.TranslateModel.Add(translateModel);
            await _context.SaveChangesAsync();

            return Ok(translateModel);
        }

        // DELETE: api/TranslateModel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTranslateModel(int id)
        {
            // Verkrijg vertaaltabel inclusief relaties
            var translateModel = await _context.TranslateModel
                .Include(e => e.Translations)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (translateModel == null) return BadRequest();

            // Verwijder alle onderliggende vertalingen
            foreach (var translation in translateModel.Translations)
            {
                _context.Translation.Remove(translation);
            }
            
            // Verwijder vanuit context
            _context.TranslateModel.Remove(translateModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
