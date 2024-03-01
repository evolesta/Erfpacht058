namespace Erfpacht058_API.Models.Eigendom
{
    using Erfpacht058_API.Models.Overeenkomst;
    using System.Numerics;

    public class Eigendom
    {
        public int Id { get; set; }
        public Adres? Adres { get; set; } // one-to-one relatie
        public string? Relatienummer { get; set; }
        public List<Eigenaar> Eigenaar { get; } = []; // many-to-many relatie
        public DateTime Ingangsdatum { get; set; }
        public DateTime? Einddatum { get; set; }
        public string? Complexnummer { get; set; }
        public float? EconomischeWaarde {  get; set; }
        public float? VerzekerdeWaarde { get; set; }
        public Kadaster? Kadaster { get; set; } // one-to-one relatie
        public ICollection<Overeenkomst> Overeenkomst { get; } = new List<Overeenkomst>(); // Optionele one-to-many relatie
        public Herziening? Herziening { get; set; } // one-to-one relatie
        public string? Notities { get; set; }
        public ICollection<Bestand> Bestand { get; } = new List<Bestand>(); // Optionele one-to-many relatie
    }

    // Data transfer object model voor aanmaken en muteren van Eigendom objecten
    public class EigendomDto
    {
        public int Id { get; set; }
        public string? Relatienummer { get; set; }
        public DateTime Ingangsdatum { get; set; }
        public string? Complexnummer { get; set; }
        public float? EconomischeWaarde { get; set; }
        public float? VerzekerdeWaarde { get; set; }
        public string? Notities { get; set; }
    }
}
