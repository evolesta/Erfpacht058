using Erfpacht058_API.Models;

namespace Erfpacht058_API.Repositories.Interfaces
{
    public interface ISettingsRepository
    {
        Task<Settings> GetSettings();
        Task<Settings> EditSettings(SettingsDto settingsDto);
    }
}
