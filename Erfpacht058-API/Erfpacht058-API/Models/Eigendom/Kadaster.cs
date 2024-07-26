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
        public string? BAGID { get; set; }
        public float Oppervlakte { get; set; }
        public float Bouwjaar { get; set; }
        public string? Gebruiksdoel { get; set; }
        public DateTime? LaatsteSynchronisatie { get; set; }
    }

    public class KadasterDto
    {
        public string? BAGID { get; set; }
        public float Oppervlakte { get; set; }
        public float Bouwjaar { get; set; }
        public string? Gebruiksdoel { get; set; }
        public DateTime? LaatsteSynchronisatie { get; set; }
    }

    // Kadaster API BAG Response
    public class BAGData
    {
        public BEmbedded _embedded { get; set; }
    }

    public class BEmbedded
    {
        public List<BAGAdres> adressen { get; set; }
    }

    public class BAGAdres
    {
        public string nummeraanduidingIdentificatie { get; set; }
        public string typeAdresseerbaarObject { get; set; }
        public string adresseerbaarObjectStatus { get; set; }
        public List<string> gebruiksdoelen { get; set; }
        public string oppervlakte { get; set; }
        public List<string> oorspronkelijkBouwjaar { get; set; }
    }
}
