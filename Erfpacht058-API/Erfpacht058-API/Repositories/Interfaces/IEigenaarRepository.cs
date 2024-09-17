using Erfpacht058_API.Models.Eigendom;

namespace Erfpacht058_API.Repositories.Interfaces
{
    public interface IEigenaarRepository
    {
        Task<IEnumerable<Eigenaar>> GetEigenaren();
        Task<Eigenaar> GetEigenaar(int id);
        Task<Eigenaar> AddEigenaar(Eigenaar eigenaar);
        Task<Eigenaar> AddExistingEigenaarToEigendom(int eigendomId, int eigenaarId);
        Task<Eigenaar> AddNewEigenaarToEigendom(int eigendomId, Eigenaar eigenaar);
        Task<Eigenaar> EditEigenaar(int id, EigenaarDto eigenaarDto);
        Task<Eigenaar> DeleteEigenaar(int id);
        Task<Eigenaar> DeleteEigenaarFromEigendom(int eigendomId, int eigenaarId);
    }
}
