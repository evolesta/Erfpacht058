using Erfpacht058_API.Models;
using Erfpacht058_API.Models.Rapport;

namespace Erfpacht058_API.Repositories.Interfaces
{
    public interface IExportRepository
    {
        Task<IEnumerable<Export>> GetExports();
        Task<Export> GetExport(int id);
        Task<Export> AddExport(ExportDto exportDto, Gebruiker gebruiker);
        Task<Template> GetTemplateById(int id); 
    }
}
