using System.Text.Json.Serialization;

namespace Erfpacht058_API.Models.Eigendom
{
    // Relationeel model van Eigendom
    public class Eigenaar
    {
        public int Id { get; set; }
        [JsonIgnore]
        public List<Eigendom> Eigendom { get; } = []; // many-to-many relatie
        public string Naam { get; set; }
        public string? Voornamen { get; set; }
        public string Voorletters { get; set; }
        public string Straatnaam { get; set; }
        public int Huisnummer { get; set; }
        public string? Toevoeging {  get; set; }
        public string Postcode { get; set; }
        public string Woonplaats { get; set; }
        public string? Debiteurnummer { get; set; }
        public DateTime Ingangsdatum { get; set; }
        public DateTime? Einddatum { get; set; }
    }

    // Dto voor Eigenaar
    public class EigenaarDto
    {
        public string Naam { get; set; }
        public string? Voornamen { get; set; }
        public string Voorletters { get; set; }
        public int EigendomId { get; set; }
        public string Straatnaam { get; set; }
        public int Huisnummer { get; set; }
        public string? Toevoeging { get; set; }
        public string Postcode { get; set; }
        public string Woonplaats { get; set; }
        public string? Debiteurnummer { get; set; }
        public DateTime Ingangsdatum { get; set; }
        public DateTime? Einddatum { get; set; }
    }
}
