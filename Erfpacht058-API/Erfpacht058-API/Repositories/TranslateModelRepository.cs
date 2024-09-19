using AutoMapper;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Rapport;
using Erfpacht058_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Erfpacht058_API.Repositories
{
    public class TranslateModelRepository : ITranslateModelRepository
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly IMapper _mapper;

        public TranslateModelRepository(Erfpacht058_APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        async Task<TranslateModel> ITranslateModelRepository.AddTranslateModel(TranslateModelDto translateModelDto)
        {
            // Maak een nieuwe vertaaltabel
            var translateModel = _mapper.Map<TranslateModel>(translateModelDto);

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

            return translateModel;
        }

        async Task<TranslateModel> ITranslateModelRepository.DeleteTranslateModel(int id)
        {
            // Verkrijg vertaaltabel inclusief relaties
            var translateModel = await _context.TranslateModel
                .Include(e => e.Translations)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (translateModel == null)
                return null;

            // Verwijder alle onderliggende vertalingen
            foreach (var translation in translateModel.Translations)
            {
                _context.Translation.Remove(translation);
            }

            // Verwijder vanuit context
            _context.TranslateModel.Remove(translateModel);
            await _context.SaveChangesAsync();

            return translateModel;
        }

        async Task<TranslateModel> ITranslateModelRepository.EditTranslateModel(int id, TranslateModelDto translateModelDto)
        {
            // Verkrijg vertaaltabel
            var translateModel = await _context.TranslateModel
                .Include(e => e.Translations)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (translateModel == null)
                return null;

            _mapper.Map(translateModelDto, translateModel);
            translateModel.WijzigingsDatum = DateTime.Now;

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

            return translateModel;
        }

        async Task<TranslateModel> ITranslateModelRepository.GetTranslateModel(int id)
        {
            // Verkrijg Vertaaltabellen inclusief vertalingen
            var translateModel = await _context.TranslateModel
                .Include(e => e.Translations)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (translateModel == null)
                return null;

            return translateModel;
        }

        async Task<IEnumerable<TranslateModel>> ITranslateModelRepository.GetTranslateModels()
        {
            return await _context.TranslateModel.ToListAsync();
        }
    }
}
