using Erfpacht058_API.Models.Facturen;
using Erfpacht058_API.Models.Rapport;
using System.Text.Json.Serialization;

namespace Erfpacht058_API.Models
{
    public class Gebruiker
    {
        public int Id { get; set; }
        public string? Naam {  get; set; }
        public string? Voornamen { get; set; }
        public string? Emailadres { get; set; }
        public string? Wachtwoord { get; set; }
        public Rol Role { get; set; }
        public bool Actief { get; set; }
        public int LogingPoging { get; set; }
        public int? ExportId { get; set; } // one-to-one relatie
        [JsonIgnore]
        public Export? Export { get; set; } // one-to-one relatie
        public int? ImportId { get; set; }
        [JsonIgnore]
        public Import? Import { get; set; } // one-to-one relatie
        public int? FactuurJobId { get; set; } // one-to-one relatie
        [JsonIgnore]
        public FactuurJob? FactuurJob { get; set; }
    }

    public enum Rol
    {
        Gebruiker,
        Beheerder
    }

    public class GebruikerDto
    {
        public int Id { get; set; }
        public string? Naam { get; set; }
        public string? Voornamen { get; set; }
        public string? Emailadres { get; set; }
        public Rol Role { get; set; }
        public bool Actief { get; set; }
    }

    public class WachtwoordDto
    {
        public string Wachtwoord { set; get; }
    }
}
