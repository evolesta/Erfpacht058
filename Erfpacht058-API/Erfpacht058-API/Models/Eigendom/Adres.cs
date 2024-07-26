using System.Text.Json.Serialization;

namespace Erfpacht058_API.Models.Eigendom
{
    // Relationeel model onder Eigendom
    public class Adres
    {
        public int Id { get; set; }
        public int? EigendomId { get; set; } // Foreign key naar Eigendom
        [JsonIgnore]
        public Eigendom? Eigendom { get; set; } // one-to-one relatie
        public string? Straatnaam { get; set; }
        public int Huisnummer { get; set; }
        public string? Toevoeging { get; set; }
        public string? Huisletter { get; set; }
        public string? Postcode { get; set; }
        public string? Woonplaats { get; set; }
    }

    public class AdresDto
    {
        public string? Straatnaam { get; set; }
        public int Huisnummer { get; set; }
        public string? Toevoeging { get; set; }
        public string? Huisletter { get; set; }
        public string? Postcode { get; set; }
        public string? Woonplaats { get; set; }
    }
}
