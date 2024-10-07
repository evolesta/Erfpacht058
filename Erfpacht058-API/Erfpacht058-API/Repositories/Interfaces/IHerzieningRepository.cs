using Erfpacht058_API.Models.Eigendom;

namespace Erfpacht058_API.Repositories.Interfaces
{
    public interface IHerzieningRepository
    {
        Task<Herziening> AddHerzieningToEigendom(int eigendomId, Herziening herziening);
        Task<Herziening> UpdateHerziening(int eigendomId, HerzieningDto herzieningDto);
        Task<Eigendom> GetEigendomById(int eigendomId);
    }
}
