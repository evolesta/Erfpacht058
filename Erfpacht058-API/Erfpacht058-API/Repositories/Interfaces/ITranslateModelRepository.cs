using Erfpacht058_API.Models.Rapport;

namespace Erfpacht058_API.Repositories.Interfaces
{
    public interface ITranslateModelRepository
    {
        Task<IEnumerable<TranslateModel>> GetTranslateModels();
        Task<TranslateModel> GetTranslateModel(int id);
        Task<TranslateModel> AddTranslateModel(TranslateModelDto translateModelDto);
        Task<TranslateModel> EditTranslateModel(int id, TranslateModelDto translateModelDto);
        Task<TranslateModel> DeleteTranslateModel(int id);
    }
}
