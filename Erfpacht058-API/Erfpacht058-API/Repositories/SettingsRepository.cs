using AutoMapper;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models;
using Erfpacht058_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Erfpacht058_API.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly IMapper _mapper;

        public SettingsRepository(Erfpacht058_APIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        async Task<Settings> ISettingsRepository.EditSettings(SettingsDto settingsDto)
        {
            // Verkrijg settings, map de Dto
            var settings = await _context.Settings.FindAsync(1);
            _mapper.Map(settingsDto, settings);

            _context.Entry(settings).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return settings;
        }

        async Task<Settings> ISettingsRepository.GetSettings()
        {
            return await _context.Settings.FindAsync(1);
        }
    }
}
