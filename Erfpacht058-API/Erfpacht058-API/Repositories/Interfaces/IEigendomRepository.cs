using Erfpacht058_API.Models.Eigendom;

namespace Erfpacht058_API.Repositories.Interfaces
{
    public interface IEigendomRepository
    {
        Task<IEnumerable<Eigendom>> GetEigendommen();
        Task<Eigendom> GetEigendom(int id);
        Task<Eigendom> AddEigendom(Eigendom eigendom);
        Task<Eigendom> EditEigendom(int id, Eigendom eigendom);
        Task<Eigendom> DeleteEigendom(int id);
    }
}
