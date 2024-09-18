using Erfpacht058_API.Models;

namespace Erfpacht058_API.Repositories.Interfaces
{
    public interface IGebruikerRepository
    {
        Task<IEnumerable<GebruikerDto>> GetGebruikers();
        Task<GebruikerDto> GetGebruiker(int id);
        Task<Gebruiker> ZoekGebruiker(string emailadres);
        Task<GebruikerDto> AddGebruiker(GebruikerDto gebruikerDto);
        Task<GebruikerDto> EditGebruiker(int id, GebruikerDto gebruikerDto);
        Task<GebruikerDto> DeleteGebruiker(int id);
        Task<GebruikerDto> EditWachtwoord(int id, WachtwoordDto wachtwoordDto);
        Task<Gebruiker> LockGebruiker(Gebruiker gebruiker);
        Task<Gebruiker> FouteLogin(Gebruiker gebruiker);
        Task<Gebruiker> ResetLogin(Gebruiker gebruiker);
    }
}
