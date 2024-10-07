using AutoMapper;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Eigendom;
using Erfpacht058_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Erfpacht058_API.Repositories
{
    public class AdresRepository : IAdresRepository
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly IMapper _mapper;

        public AdresRepository (Erfpacht058_APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        async Task<Adres> IAdresRepository.AddAdres(int eigendomId, Adres adres)
        {
            // Verkrijg huidig Eigendom object
            var eigendom = await _context.Eigendom.FindAsync(eigendomId);
            if (eigendom == null)
                return null;

            // Voeg referentie toe naar Eigendom object in nieuwe Adres object en voeg toe aan database
            adres.Eigendom = eigendom;
            _context.Adres.Add(adres);

            // Pas nieuwe relatie toe in eigendom object
            eigendom.Adres = adres;
            _context.Entry(eigendom).State = EntityState.Modified;

            // Pas toe naar database
            await _context.SaveChangesAsync();

            return adres;
        }

        async Task<Adres> IAdresRepository.EditAdres(int eigendomId, AdresDto adresDto)
        {
            // verkrijg huidig eigendom en adres object
            var eigendom = await _context.Eigendom
                .Include(e => e.Adres)
                .FirstOrDefaultAsync(e => e.Id == eigendomId);
            if (eigendom == null)
                return null;

            // Update DB object via Mapper
            var adres = eigendom.Adres;
            _mapper.Map(adresDto, adres);

            // Adres object opslaan
            _context.Entry(adres).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return adres;
        }

        async Task<IEnumerable<Adres>> IAdresRepository.GetAdressen()
        {
            return await _context.Adres.ToListAsync();
        }

        async Task<Eigendom> IAdresRepository.GetEigendomById(int eigendomId)
        {
            return await _context.Eigendom.FindAsync(eigendomId);
        }
    }
}
