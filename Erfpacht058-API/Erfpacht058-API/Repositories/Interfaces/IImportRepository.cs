using Erfpacht058_API.Models;
using Erfpacht058_API.Models.Rapport;

namespace Erfpacht058_API.Repositories.Interfaces
{
    public interface IImportRepository
    {
        Task<IEnumerable<Import>> GetImports();
        Task<Import> GetImport(int id);
        Task<Import> AddImport(int translateModelId, Gebruiker gebruiker, string filepath);
    }
}
