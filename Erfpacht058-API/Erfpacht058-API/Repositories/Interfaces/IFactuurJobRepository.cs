using Erfpacht058_API.Models;
using Erfpacht058_API.Models.Facturen;

namespace Erfpacht058_API.Repositories.Interfaces
{
    public interface IFactuurJobRepository
    {
        Task<IEnumerable<FactuurJob>> GetFactuurJobs();
        Task<FactuurJob> GetFactuurJob(int id);
        Task<FactuurJob> AddFactuurJob(FactuurJobDto factuurJobDto, Gebruiker gebruiker);
    }
}
