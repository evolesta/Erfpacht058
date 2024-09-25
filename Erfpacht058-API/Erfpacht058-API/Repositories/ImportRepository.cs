using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models;
using Erfpacht058_API.Models.Rapport;
using Erfpacht058_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Erfpacht058_API.Repositories
{
    public class ImportRepository : IImportRepository
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly IMapper _mapper;

        public ImportRepository(Erfpacht058_APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        async Task<Import> IImportRepository.AddImport(int translateModelId, Gebruiker gebruiker, string filepath)
        {
            var translationTabel = await _context.TranslateModel
                .FirstOrDefaultAsync(x => x.Id == translateModelId);

            if (translationTabel == null)
                return null;

            // Nieuw import entry maken
            var import = new Import
            {
                Aanmaakdatum = DateTime.Now,
                Gebruiker = gebruiker,
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

            return import;
        }

        async Task<Import> IImportRepository.GetImport(int id)
        {
            return await _context.Import
                .Include(x => x.Task)
                .Include(x => x.TranslateModel)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        async Task<IEnumerable<Import>> IImportRepository.GetImports()
        {
            return await _context.Import
                .Include(e => e.Task)
                .Include(e => e.TranslateModel)
                .ToListAsync();
        }
    }
}
