namespace Erfpacht058_API.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public string? BAGAPI {  get; set; }
    }

    public class SettingsDto
    {
        public string? BAGAPI { get; set; }
    }
}
