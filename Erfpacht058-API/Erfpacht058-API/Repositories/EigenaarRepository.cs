using AutoMapper;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Eigendom;
using Erfpacht058_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Erfpacht058_API.Repositories
{
    public class EigenaarRepository : IEigenaarRepository
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly IMapper _mapper;

        public EigenaarRepository (Erfpacht058_APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        async Task<Eigenaar> IEigenaarRepository.AddEigenaar(Eigenaar eigenaar)
        {
            _context.Eigenaar.Add(eigenaar);
            await _context.SaveChangesAsync();

            return eigenaar;
        }

        async Task<Eigenaar> IEigenaarRepository.AddExistingEigenaarToEigendom(int eigendomId, int eigenaarId)
        {
            // Verkrijg eigendom object
            var eigendom = await _context.Eigendom
                .Include(e => e.Eigenaar)
                .FirstOrDefaultAsync(e => e.Id == eigendomId);

            if (eigendom == null)
                return null;

            // Verkrijg eigenaar object
            var eigenaar = await _context.Eigenaar
                .Include(e => e.Eigendom)
                .FirstOrDefaultAsync(e => e.Id == eigenaarId);

            if (eigenaar == null)
                return null;

            // Leg relaties vast in database
            eigendom.Eigenaar.Add(eigenaar);
            eigenaar.Eigendom.Add(eigendom);
            _context.Entry(eigendom).State = EntityState.Modified;
            _context.Entry(eigenaar).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return eigenaar;
        }

        async Task<Eigenaar> IEigenaarRepository.AddNewEigenaarToEigendom(int eigendomId, Eigenaar eigenaar)
        {
            // Verkrijg Eigendom object
            var eigendom = await _context.Eigendom
                .Include(e => e.Eigenaar)
                .FirstOrDefaultAsync(e => e.Id == eigendomId);

            if (eigendom == null) // Null check
                return null;

            eigenaar.Eigendom.Add(eigendom);
            eigendom.Eigenaar.Add(eigenaar);

            // Nieuwe eigenaar opslaan in database
            _context.Eigenaar.Add(eigenaar);
            _context.Entry(eigendom).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return eigenaar;
        }

        async Task<Eigenaar> IEigenaarRepository.DeleteEigenaar(int id)
        {
            var eigenaar = await _context.Eigenaar.FindAsync(id);

            if (eigenaar != null)
            {
                _context.Eigenaar.Remove(eigenaar);
                await _context.SaveChangesAsync();
                return eigenaar;
            }
            else
                return null;
        }

        async Task<Eigenaar> IEigenaarRepository.DeleteEigenaarFromEigendom(int eigendomId, int eigenaarId)
        {
            // Verkrijg eigendom object
            var eigendom = await _context.Eigendom
                .Include(e => e.Eigenaar)
                .FirstOrDefaultAsync(e => e.Id == eigendomId);

            if (eigendom == null) 
                return null;

            // Verkrijg eigenaar object
            var eigenaar = await _context.Eigenaar
                .Include(e => e.Eigendom)
                .FirstOrDefaultAsync(e => e.Id == eigenaarId);

            if (eigenaar == null)
                return null;

            // Verwijder relatie uit eigendom en verwijder eigenaar object
            eigendom.Eigenaar.Remove(eigenaar);
            eigenaar.Eigendom.Remove(eigendom);
            _context.Entry(eigendom).State = EntityState.Modified;
            _context.Entry(eigenaar).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return eigenaar;
        }

        async Task<Eigenaar> IEigenaarRepository.EditEigenaar(int id, EigenaarDto eigenaarDto)
        {
            var eigenaar = await _context.Eigenaar.FindAsync(id);
            if (eigenaar == null)
                return null;

            _mapper.Map(eigenaarDto, eigenaar);

            // Wijzigingen opslaan naar database
            _context.Entry(eigenaar).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return eigenaar;
        }

        async Task<Eigenaar> IEigenaarRepository.GetEigenaar(int id)
        {
            return await _context.Eigenaar.FindAsync(id);
        }

        async Task<IEnumerable<Eigenaar>> IEigenaarRepository.GetEigenaren()
        {
            return await _context.Eigenaar.ToListAsync();
        }
    }
}
