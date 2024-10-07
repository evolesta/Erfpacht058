using Erfpacht058_API.Models.Eigendom;

namespace Erfpacht058_API.Repositories.Interfaces
{
    public interface IAdresRepository
    {
        Task<IEnumerable<Adres>> GetAdressen();
        Task<Adres> AddAdres(int eigendomId, Adres adres);
        Task<Adres> EditAdres(int eigendomId, AdresDto adresDto);
        Task<Eigendom> GetEigendomById(int eigendomId);
    }
}
