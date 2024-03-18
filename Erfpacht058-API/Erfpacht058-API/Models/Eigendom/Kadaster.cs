using System.Text.Json.Serialization;

namespace Erfpacht058_API.Models.Eigendom
{
    // Relationeel model van Eigendom
    public class Kadaster
    {
        public int Id { get; set; }
        public int? EigendomId { get; set; } // Foreign key naar Eigendom
        [JsonIgnore]
        public Eigendom? Eigendom { get; set; } // one-to-one relatie
        public string? KadastraalNummer { get; set; }
        public float Deeloppervlakte { get; set; }
        public float KadastraleGrootte { get; set; }
        public string? ObjectType { get; set; }
        public DateTime? LaatsteSynchronisatie { get; set; }
    }

    public class KadasterDto
    {
        public string KadastraalNummer { get; set; }
        public float? Deeloppervlakte { get; set; }
        public float? KadastraleGrootte { get; set; }
        public string? ObjectType { get; set; }
    }
}
