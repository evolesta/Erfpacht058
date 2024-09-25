using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models;
using Erfpacht058_API.Models.Rapport;
using Erfpacht058_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Erfpacht058_API.Repositories
{
    public class ExportRepository : IExportRepository
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly IMapper _mapper;

        public ExportRepository(Erfpacht058_APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        async Task<Export> IExportRepository.AddExport(ExportDto exportDto, Gebruiker gebruiker)
        {
            // Verkrijg template
            var template = await _context.Template
                .FindAsync(exportDto.TemplateId);

            if (template == null)
                return null;

            // Map Dto naar Export
            var export = _mapper.Map<Export>(exportDto);
            export.Gebruiker = gebruiker;
            export.Template = template;
            export.AanmaakDatum = DateTime.Now;

            // Maak een nieuwe Taak aan
            var task = new TaskQueue
            {
                SoortTaak = SoortTaak.Export,
                Status = Status.Nieuw,
                Prioriteit = Prioriteit.Midden,
                AanmaakDatum = DateTime.Now,
                Export = export
            };

            export.Task = task; // Update Export relatie Taak

            _context.Export.Add(export);
            _context.TaskQueue.Add(task);

            await _context.SaveChangesAsync();

            return export;
        }

        async Task<Export> IExportRepository.GetExport(int id)
        {
            return await _context.Export
                .Include(e => e.Task)
                .Include(e => e.Template)
                .Include(e => e.Gebruiker)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        async Task<IEnumerable<Export>> IExportRepository.GetExports()
        {
            return await _context.Export
                .Include(e => e.Task)
                .Include(e => e.Template)
                .Include(e => e.Gebruiker)
                .ToListAsync();
        }
    }
}
