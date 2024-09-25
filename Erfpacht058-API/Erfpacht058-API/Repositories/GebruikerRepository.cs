using AutoMapper;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models;
using Erfpacht058_API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Erfpacht058_API.Repositories
{
    public class GebruikerRepository : IGebruikerRepository
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly IMapper _mapper;

        public GebruikerRepository(Erfpacht058_APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        async Task<GebruikerDto> IGebruikerRepository.AddGebruiker(GebruikerDto gebruikerDto)
        {
            // Map Dto to Gebruiker
            var gebruiker = _mapper.Map<Gebruiker>(gebruikerDto);

            // Hash wachtwoord
            gebruiker.Wachtwoord = BCrypt.Net.BCrypt.HashPassword(gebruiker.Wachtwoord);

            _context.Gebruiker.Add(gebruiker);
            await _context.SaveChangesAsync();

            return gebruikerDto;
        }

        async Task<GebruikerDto> IGebruikerRepository.DeleteGebruiker(int id)
        {
            // Verkrijg gebruiker
            var gebruiker = await _context.Gebruiker.FindAsync(id);

            if (gebruiker == null)
                return null;

            // Verwijder user
            _context.Gebruiker.Remove(gebruiker);
            await _context.SaveChangesAsync();

            return _mapper.Map<GebruikerDto>(gebruiker);
        }

        async Task<GebruikerDto> IGebruikerRepository.EditGebruiker(int id, GebruikerDto gebruikerDto)
        {
            // Verkrijg gebruiker
            var gebruiker = await _context.Gebruiker
                .FirstOrDefaultAsync(x => x.Id == id);

            if (gebruiker == null)
                return null;

            // Map Dto met gewijzigde data
            _mapper.Map(gebruikerDto, gebruiker);
            gebruiker.Id = id; // overschrijf ID uit DTO ivm UPDATE

            // Als gebruiker op actief wordt gezet, reset dan de inlogpogingen
            if (gebruiker.Actief)
                gebruiker.LogingPoging = 0;

            // Wijzig naar context
            _context.Entry(gebruiker).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return gebruikerDto;
        }

        async Task<GebruikerDto> IGebruikerRepository.EditWachtwoord(int id, WachtwoordDto wachtwoordDto)
        {
            // Verkrijg gebruiker
            var gebruiker = await _context.Gebruiker.FindAsync(id);

            if (gebruiker == null)
                return null;

            // Hash en update het nieuwe wachtwoord
            gebruiker.Id = id;
            gebruiker.Wachtwoord = BCrypt.Net.BCrypt.HashPassword(wachtwoordDto.Wachtwoord);

            // Opslaan naar database
            _context.Entry(gebruiker).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _mapper.Map<GebruikerDto>(gebruiker);
        }

        async Task<IEnumerable<GebruikerDto>> IGebruikerRepository.GetGebruikers()
        {
            var gebruikers = await _context.Gebruiker.ToListAsync();
            var gebruikersDto = _mapper.Map<IEnumerable<GebruikerDto>>(gebruikers);

            return gebruikersDto;
        }

        async Task<GebruikerDto> IGebruikerRepository.GetGebruiker(int id)
        {
            var gebruiker = await _context.Gebruiker.FindAsync(id);
            var gebruikerDto = _mapper.Map<GebruikerDto>(gebruiker);

            return gebruikerDto;
        }

        async Task<Gebruiker> IGebruikerRepository.ZoekGebruiker(string emailadres)
        {
            return await _context.Gebruiker
                .FirstOrDefaultAsync(u => u.Emailadres == emailadres);
        }

        async Task<Gebruiker> IGebruikerRepository.LockGebruiker(Gebruiker gebruiker)
        {
            // Zet gebruiker inactief
            gebruiker.Actief = false;

            _context.Entry(gebruiker).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return gebruiker;
        }

        async Task<Gebruiker> IGebruikerRepository.FouteLogin(Gebruiker gebruiker)
        {
            // Verhoog logingpoging
            // Wachtwoord is incorrect
            gebruiker.LogingPoging++; // Verhoog verk. loging pogingen

            _context.Entry(gebruiker).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return gebruiker;
        }

        async Task<Gebruiker> IGebruikerRepository.ResetLogin(Gebruiker gebruiker)
        {
            // Reset loginpogingen
            gebruiker.LogingPoging = 0;
            _context.Entry(gebruiker).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return gebruiker;
        }
    }
}
