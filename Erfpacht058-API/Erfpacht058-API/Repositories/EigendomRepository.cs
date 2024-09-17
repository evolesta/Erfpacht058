using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Eigendom;
using Erfpacht058_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Erfpacht058_API.Repositories
{
    public class EigendomRepository : IEigendomRepository
    {
        private readonly Erfpacht058_APIContext _context;

        public EigendomRepository(Erfpacht058_APIContext context)
        {
            _context = context;
        }

        async Task<Eigendom> IEigendomRepository.AddEigendom(Eigendom eigendom)
        {
            // Nieuw object toevoegen aan database
            _context.Eigendom.Add(eigendom);
            await _context.SaveChangesAsync();

            return eigendom;
        }

        async Task<Eigendom> IEigendomRepository.DeleteEigendom(int id)
        {
            // verkrijg object vanuit DB en verwijder uit de context
            var eigendom = await _context.Eigendom.FindAsync(id);

            if (eigendom != null)
            {
                _context.Eigendom.Remove(eigendom);
                await _context.SaveChangesAsync();

                return eigendom;
            }
            else
                return null;
        }

        async Task<Eigendom> IEigendomRepository.EditEigendom(int id, Eigendom eigendom)
        {
            var result = await _context.Eigendom.FindAsync(id); // verkrijg object

            // wijzig object
            if (result != null)
            {
                result.Relatienummer = eigendom.Relatienummer;
                result.Ingangsdatum = eigendom.Ingangsdatum;
                result.Einddatum = eigendom.Einddatum;
                result.Complexnummer = eigendom.Complexnummer;
                result.EconomischeWaarde = eigendom.EconomischeWaarde;
                result.VerzekerdeWaarde = eigendom.VerzekerdeWaarde;
                result.Notities = eigendom.Notities;

                // sla op in DB
                _context.Entry(result).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return result;
            }

            return null;
        }

        async Task<Eigendom> IEigendomRepository.GetEigendom(int id)
        {
            return await _context.Eigendom
                .Include(e => e.Adres)
                .Include(e => e.Eigenaar)
                .Include(e => e.Herziening)
                .Include(e => e.Kadaster)
                .Include(e => e.Bestand)
                .Include(e => e.Overeenkomst)
                    .ThenInclude(o => o.Financien)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        async Task<IEnumerable<Eigendom>> IEigendomRepository.GetEigendommen()
        {
            return await _context.Eigendom.ToListAsync();
        }
    }
}
