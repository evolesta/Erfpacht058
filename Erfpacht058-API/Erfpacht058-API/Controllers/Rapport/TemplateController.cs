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
    [Authorize(Roles = "Beheerder")]
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;

        public TemplateController(Erfpacht058_APIContext context)
        {
            _context = context;
        }

        // GET: api/Template
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Template>>> GetTemplate()
        {
            return await _context.Template.ToListAsync();
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
            // Verkrijg Template object met relaties
            var template = await _context.Template
                .Include(x => x.RapportData)
                .Include(x => x.Filters)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (template == null) return BadRequest();

            return template;
        }

        // PUT: api/Template/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTemplate(int id, TemplateDto templateDto)
        {
            // verkrijg template object
            var template = await _context.Template
                .Include(x => x.RapportData)
                .Include(x => x.Filters)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (template == null) return BadRequest();

            template.Naam = templateDto.Naam;
            template.Maker = templateDto.Maker;
            template.Model = templateDto.Model;
            template.WijzigingsDatum = DateTime.Now;

            // Vernieuw de RapportData relationele objecten uit de lijst
            foreach (var rapportData in templateDto.RapportData)
            {
                // Verkrijg het bestaande object en werk bij
                var existingRapportData = template.RapportData.FirstOrDefault(rd => rd.Id == rapportData.Id);

                if (existingRapportData == null)
                {
                    // Nieuw record is toegevoegd
                    var newRapportData = new RapportData
                    {
                        Naam = rapportData.Naam,
                        Key = rapportData.Key,
                        Template = template
                    };

                    _context.RapportData.Add(newRapportData);
                    template.RapportData.Add(newRapportData);
                }
                else
                {
                    // bestaande record wordt gewijzigd
                    existingRapportData.Key = rapportData.Key;
                    existingRapportData.Naam = rapportData.Naam;

                    _context.Entry(existingRapportData).State = EntityState.Modified; // Wijzig de records in de database
                }

                await _context.SaveChangesAsync();
            }

            // Vernieuw de Filter relationele objecten
            foreach (var filter in templateDto.Filters)
            {
                var existingFilter = template.Filters.FirstOrDefault(f => f.Id == filter.Id);

                if (existingFilter == null)
                {
                    // Nieuw record is toegevoegd
                    var newFilter = new Filter
                    {
                        Key = filter.Key,
                        Operation = filter.Operation,
                        Value = filter.Value,
                        Template = template
                    };

                    _context.Filter.Add(newFilter);
                    template.Filters.Add(newFilter);
                }
                else
                {
                    existingFilter.Key = filter.Key;
                    existingFilter.Value = filter.Value;
                    existingFilter.Operation = filter.Operation;

                    _context.Entry(existingFilter).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
            }

            // Verwijder records die niet meer in de payload aanwezig zijn, maar nog wel in de database
            var rapportDataIdsInDto = templateDto.RapportData.Select(rd => rd.Id).ToList(); // verkrijg alle Ids uit de payload
            var filterIdsInDto = templateDto.Filters.Select(f => f.Id).ToList();
            // verkrijg alle Ids die nog wel in de database staan en niet meer in de payload (te verwijderen ids)
            var rapportDataToRemove = template.RapportData.Where(rd => !rapportDataIdsInDto.Contains(rd.Id)).ToList();
            var filterIdsToRemove = template.Filters.Where(f => !filterIdsInDto.Contains(f.Id)).ToList();
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

            return Ok(template);
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
            // Maak een nieuw Template object
            var template = new Template
            {
                Naam = templateDto.Naam,
                AanmaakDatum = DateTime.Now,
                Maker = templateDto.Maker,
                Model = templateDto.Model,
                WijzigingsDatum = DateTime.Now
            };

            // Loop door de rapportData en maak nieuwe records aan
            foreach (var rapportData in templateDto.RapportData)
            {
                var newRapportData = new RapportData
                {
                    Naam = rapportData.Naam,
                    Key = rapportData.Key,
                    Template = template
                };

                _context.RapportData.Add(newRapportData); // voeg toe aan Database
                template.RapportData.Add(newRapportData); // Voeg relatie toe aan Template object
            }

            // Loop door de filters en maak nieuwe data aan
            foreach (var filter in templateDto.Filters)
            {
                var newFilter = new Filter
                {
                    Key = filter.Key,
                    Operation = filter.Operation,
                    Value = filter.Value,
                    Template = template
                };

                _context.Filter.Add(newFilter); // Voeg nieuwe filter toe aan database
                template.Filters.Add(newFilter); // Voeg relatie toe aan Template object
            }
            
            // Voeg toe aan DB Context en sla op in database
            _context.Template.Add(template);
            await _context.SaveChangesAsync();

            return Ok(template);
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
            // Verkrijg object
            var template = await _context.Template
                .Include(x => x.RapportData)
                .Include(x => x.Filters)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (template == null) return BadRequest();

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

            return NoContent();
        }

        // GET /api/Template/models
        /// <summary>
        /// Verkrijg alle datastructuur voor het opstellen van een sjabloon (beheerder)
        /// </summary>
        /// <returns></returns>
        [HttpGet("models")]
        public async Task<ActionResult> GetModelsStructure()
        {
            // Vekrijg alle entiteiten uit de DbContext
            var entityTypes = _context.Model.GetEntityTypes();

            // Genereer een JSON structuur per entiteit en verkrijg de datatypen
            var structure = entityTypes.Select(entityType =>
            {
                var properties = entityType.GetProperties()
                    .Select(property => new
                    {
                        Name = property.Name,
                        Type = property.ClrType.Name
                    })
                    .ToList();

                return new
                {
                    TableName = entityType.ClrType.FullName,
                    Properties = properties,
                };
            }).ToList();

            return Ok(structure);
        }
    }
}
