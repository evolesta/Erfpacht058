using AutoMapper;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Rapport;
using Erfpacht058_API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Erfpacht058_API.Repositories
{
    public class TemplateRepository : ITemplateRepository
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly IMapper _mapper;

        public TemplateRepository(Erfpacht058_APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        async Task<Template> ITemplateRepository.AddTemplate(TemplateDto templateDto)
        {
            // Map Dto aan Template
            var template = _mapper.Map<Template>(templateDto);
            template.AanmaakDatum = DateTime.Now;

            // Voeg toe aan DB Context en sla op in database
            _context.Template.Add(template);
            await _context.SaveChangesAsync();

            return template;
        }

        async Task<Template> ITemplateRepository.DeleteTemplate(int id)
        {
            // Verkrijg object
            var template = await _context.Template
                .Include(x => x.RapportData)
                .Include(x => x.Filters)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (template == null)
                return null;

            // Verwijder onderliggende RapportData objecten
            foreach (var rapportData in template.RapportData)
            {
                _context.RapportData.Remove(rapportData);
            }

            // Verwijder alle onderliggende Filter objecten
            foreach (var filter in template.Filters)
            {
                _context.Filter.Remove(filter);
            }

            // Verwijder Template object en sla op in database
            _context.Template.Remove(template);
            await _context.SaveChangesAsync();

            return template;
        }

        async Task<Template> ITemplateRepository.EditTemplate(int id, TemplateDto templateDto)
        {
            // verkrijg template object
            var template = await _context.Template
                .Include(x => x.RapportData)
                .Include(x => x.Filters)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (template == null)
                return null;

            _mapper.Map(templateDto, template);
            template.WijzigingsDatum = DateTime.Now;

            // Verwijder records die niet meer in de payload aanwezig zijn, maar nog wel in de database
            var rapportDataIdsInDto = templateDto.RapportData
                .Select(rd => rd.Id)
                .ToList(); // verkrijg alle Ids uit de payload
            var filterIdsInDto = templateDto.Filters
                .Select(f => f.Id)
                .ToList();
            // verkrijg alle Ids die nog wel in de database staan en niet meer in de payload (te verwijderen ids)
            var rapportDataToRemove = template.RapportData
                .Where(rd => !rapportDataIdsInDto.Contains(0) && !rapportDataIdsInDto.Contains(rd.Id))
                .ToList();
            var filterIdsToRemove = template.Filters
                .Where(f => !filterIdsInDto.Contains(0) && !filterIdsInDto.Contains(f.Id))
                .ToList();
            // Verwijder de IDS voor RapportData
            foreach (var idToRemove in rapportDataToRemove)
            {
                _context.RapportData.Remove(idToRemove); // verwijder uit context
                template.RapportData.Remove(idToRemove); // verwijder uit relatie lijst
            }

            // Verwijder de IDS voor Filters
            foreach (var filterToRemove in filterIdsToRemove)
            {
                _context.Filter.Remove(filterToRemove);
                template.Filters.Remove(filterToRemove);
            }

            // Sla op in database
            _context.Entry(template).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return template;
        }

        List<EntityModelStructure> ITemplateRepository.GetModelsStructure()
        {
            // Vekrijg alle entiteiten uit de DbContext
            var entityTypes = _context.Model.GetEntityTypes();

            // Genereer een JSON structuur per entiteit en verkrijg de datatypen
            var structure = entityTypes.Select(entityType =>
            {
                var properties = entityType.GetProperties()
                    .Where(property => !property.IsForeignKey()) // sluit Foreign Key velden uit
                    .Select(property => new PropertyModel
                    {
                        Name = property.Name,
                        Type = property.ClrType.Name
                    })
                    .ToList();

                return new EntityModelStructure
                {
                    TableName = entityType.ClrType.FullName,
                    Properties = properties,
                };
            }).ToList();

            return structure;
        }

        async Task<Template> ITemplateRepository.GetTemplate(int id)
        {
            // Verkrijg Template object met relaties
            var template = await _context.Template
                .Include(x => x.RapportData)
                .Include(x => x.Filters)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (template == null)
                return null;

            return template;
        }

        async Task<IEnumerable<Template>> ITemplateRepository.GetTemplates()
        {
            return await _context.Template.ToListAsync();
        }
    }
}
