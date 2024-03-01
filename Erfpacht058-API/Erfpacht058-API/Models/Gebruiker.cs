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
}
