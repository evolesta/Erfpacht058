using Erfpacht058_API.Models.Rapport;
using Microsoft.AspNetCore.Mvc;

namespace Erfpacht058_API.Repositories.Interfaces
{
    public interface ITemplateRepository
    {
        Task<IEnumerable<Template>> GetTemplates();
        Task<Template> GetTemplate(int id);
        Task<Template> EditTemplate(int id, TemplateDto templateDto);
        Task<Template> AddTemplate(TemplateDto templateDto);
        Task<Template> DeleteTemplate(int id);
        List<EntityModelStructure> GetModelsStructure();
    }
}
