using Erfpacht058_API.Models;
using Erfpacht058_API.Models.Eigendom;

namespace Erfpacht058_API.Repositories.Interfaces
{
    public interface IBestandRepository
    {
        Task<Bestand> GetBestand(int id);
        Task<Bestand> AddBestand(int eigendomId, Bestand bestand);
        Task<Bestand> UpdateBestand(int id, BestandDto bestandDto);
        Task<Bestand> DeleteBestand(int id);
        Task<Eigendom> GetEigendomById(int eigendomId); 
    }
}
