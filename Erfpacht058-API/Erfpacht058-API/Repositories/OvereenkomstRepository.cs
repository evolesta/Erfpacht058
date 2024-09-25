using AutoMapper;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Eigendom;
using Erfpacht058_API.Models.OvereenkomstNS;
using Erfpacht058_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Erfpacht058_API.Repositories
{
    public class OvereenkomstRepository : IOvereenkomstRepository
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly IMapper _mapper;

        public OvereenkomstRepository(Erfpacht058_APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        async Task<Overeenkomst> IOvereenkomstRepository.AddOvereenkomst(int eigendomId, Overeenkomst overeenkomst)
        {
            // Verkrijg eigendom object
            var eigendom = await _context.Eigendom
            .Include(e => e.Overeenkomst)
                .FirstOrDefaultAsync(e => e.Id == eigendomId);

            if (eigendom == null) 
                return null;
         
            overeenkomst.Eigendom = eigendom;

            // Leg relaties vast en sla object op in database
            _context.Financien.Add(overeenkomst.Financien);
            _context.Overeenkomst.Add(overeenkomst);
            eigendom.Overeenkomst.Add(overeenkomst);

            _context.Entry(eigendom).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return overeenkomst;
        }

        async Task<Overeenkomst> IOvereenkomstRepository.DeleteOvereenkomst(int id)
        {
            // verkrijg benodigde objecten
            var overeenkomst = await _context.Overeenkomst
                .Include(e => e.Eigendom)
                .Include(e => e.Financien)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (overeenkomst == null)
                return null;

            var eigendom = overeenkomst.Eigendom;

            if (eigendom == null)
                return null;

            var financien = overeenkomst.Financien;

            // Verwijder relaties en object en sla op
            eigendom.Overeenkomst.Remove(overeenkomst);
            _context.Overeenkomst.Remove(overeenkomst);
            _context.Financien.Remove(financien);
            _context.Entry(eigendom).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return overeenkomst;
        }

        async Task<Overeenkomst> IOvereenkomstRepository.EditOvereenkomst(int id, OvereenkomstDto overeenkomstDto)
        {
            // Verkrijg bestaande overeenkomst
            var overeenkomst = await _context.Overeenkomst
                .Include(e => e.Financien)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (overeenkomst == null)
                return null;

            _mapper.Map(overeenkomstDto, overeenkomst);
            _mapper.Map(overeenkomstDto.Financien, overeenkomst.Financien);

            // Sla wijzigingen op in database
            _context.Entry(overeenkomst).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return overeenkomst;
        }

        async Task<Overeenkomst> IOvereenkomstRepository.GetOvereenkomst(int id)
        {
            return await _context.Overeenkomst
                .Include(e => e.Financien)
                .Include(e => e.Eigendom)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        async Task<IEnumerable<Overeenkomst>> IOvereenkomstRepository.GetOvereenkomsten()
        {
            return await _context.Overeenkomst
                .Include(e => e.Financien)
                .Include(e => e.Eigendom)
                .ToListAsync();
        }

        async Task<Eigendom> IOvereenkomstRepository.KoppelOvereenkomstAanEigendom(int eigendomId, int overeenkomstId)
        {
            // Verkrijg eigendom en overeenkomst objecten
            var eigendom = await _context.Eigendom.FindAsync(eigendomId);
            if (eigendom == null)
                return null;

            var overeenkomst = await _context.Overeenkomst.FindAsync(overeenkomstId);
            if (overeenkomst == null)
                return null;

            // Relaties leggen en opslaan
            eigendom.Overeenkomst.Add(overeenkomst);
            overeenkomst.Eigendom = eigendom;

            // Sla op in database
            _context.Entry(eigendom).State = EntityState.Modified;
            _context.Entry(overeenkomst).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return eigendom;
        }
    }
}
