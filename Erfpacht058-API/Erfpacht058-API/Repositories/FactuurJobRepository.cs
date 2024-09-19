using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models;
using Erfpacht058_API.Models.Facturen;
using Erfpacht058_API.Models.Rapport;
using Erfpacht058_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Erfpacht058_API.Repositories
{
    public class FactuurJobRepository : IFactuurJobRepository
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly IMapper _mapper;

        public FactuurJobRepository(Erfpacht058_APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        async Task<FactuurJob> IFactuurJobRepository.AddFactuurJob(FactuurJobDto factuurJobDto, Gebruiker gebruiker)
        {
            // Map dto naar FactuurJob
            var factuurJob = _mapper.Map<FactuurJob>(factuurJobDto);

            // Nieuwe taak aanmaken
            var task = new TaskQueue
            {
                AanmaakDatum = DateTime.Now,
                FactuurJob = factuurJob,
                Prioriteit = Prioriteit.Midden,
                SoortTaak = SoortTaak.Facturen,
                Status = Status.Nieuw,
            };

            // Relaties leggen en entities toevoegen aan context
            factuurJob.Gebruiker = gebruiker;
            factuurJob.Task = task;

            _context.TaskQueue.Add(task);
            _context.FactuurJob.Add(factuurJob);

            await _context.SaveChangesAsync();

            return factuurJob;
        }

        async Task<FactuurJob> IFactuurJobRepository.GetFactuurJob(int id)
        {
            return await _context.FactuurJob
                .Include(e => e.Task)
                .Include(e => e.Facturen)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        async Task<IEnumerable<FactuurJob>> IFactuurJobRepository.GetFactuurJobs()
        {
            return await _context.FactuurJob
                .Include(e => e.Task)
                .ToListAsync();
        }
    }
}
