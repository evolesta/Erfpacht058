using AutoMapper;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models;
using Erfpacht058_API.Models.Eigendom;
using Erfpacht058_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Erfpacht058_API.Repositories
{
    public class BestandRepository : IBestandRepository
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly IMapper _mapper;

        public BestandRepository(Erfpacht058_APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        async Task<Bestand> IBestandRepository.AddBestand(int eigendomId, Bestand bestand)
        {
            // Verkrijg eigendom object
            var eigendom = await _context.Eigendom
                .Include(e => e.Bestand)
                .FirstOrDefaultAsync(e => e.Id == eigendomId);

            if (eigendom == null) return null;

            // Leg relaties en voeg toe aan Context
            bestand.Eigendom = eigendom;

            eigendom.Bestand.Add(bestand);
            _context.Bestand.Add(bestand);
            _context.Entry(eigendom).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return bestand;
        }

        async Task<Bestand> IBestandRepository.DeleteBestand(int id)
        {
            // verkrijg object van context
            var bestand = await _context.Bestand
                .Include(e => e.Eigendom)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (bestand == null)
                return null;

            var eigendom = bestand.Eigendom;

            // Verwijder relaties en verwerk naar database
            eigendom.Bestand.Remove(bestand);
            _context.Bestand.Remove(bestand);
            _context.Entry(bestand).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return bestand;
        }

        async Task<Bestand> IBestandRepository.GetBestand(int id)
        {
            return await _context.Bestand.FindAsync(id);
        }

        async Task<Eigendom> IBestandRepository.GetEigendomById(int eigendomId)
        {
            return await _context.Eigendom.FindAsync(eigendomId);
        }

        async Task<Bestand> IBestandRepository.UpdateBestand(int id, BestandDto bestandDto)
        {
            // Verkrijg bestand
            var bestand = await _context.Bestand.FindAsync(id);
            if (bestand == null)
                return null;

            _mapper.Map(bestandDto, bestand);

            _context.Entry(bestand).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return bestand;
        }
    }
}
