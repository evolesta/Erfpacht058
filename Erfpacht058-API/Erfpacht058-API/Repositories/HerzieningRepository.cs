using AutoMapper;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Eigendom;
using Erfpacht058_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Erfpacht058_API.Repositories
{
    public class HerzieningRepository : IHerzieningRepository
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly IMapper _mapper;

        public HerzieningRepository(Erfpacht058_APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        async Task<Herziening> IHerzieningRepository.AddHerzieningToEigendom(int eigendomId, Herziening herziening)
        {
            // Verkrijg eigendom object
            var eigendom = await _context.Eigendom
                .Include(e => e.Herziening)
                .FirstOrDefaultAsync(e => e.Id == eigendomId);
            if (eigendom == null) return null;

            // Leg relaties tussen objecten
            herziening.Eigendom = eigendom;
            eigendom.Herziening = herziening;

            // Voeg toe aan database
            _context.Herziening.Add(herziening);
            _context.Entry(eigendom).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return herziening;
        }

        async Task<Eigendom> IHerzieningRepository.GetEigendomById(int eigendomId)
        {
            return await _context.Eigendom.FindAsync(eigendomId);
        }

        async Task<Herziening> IHerzieningRepository.UpdateHerziening(int eigendomId, HerzieningDto herzieningDto)
        {
            // verkrijg eigendom object
            var eigendom = await _context.Eigendom
               .Include(e => e.Herziening)
               .FirstOrDefaultAsync(e => e.Id == eigendomId);

            if (eigendom == null) 
                return null;

            var herziening = eigendom.Herziening;
            _mapper.Map(herzieningDto, herziening);

            // Sla wijzigingen op naar database
            _context.Entry(herziening).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return eigendom.Herziening;
        }
    }
}
