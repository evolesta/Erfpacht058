using Erfpacht058_API.Models.Facturen;
using System.Text.Json.Serialization;

namespace Erfpacht058_API.Models.OvereenkomstNS
{
    // Relationeel model van Overeenkomst
    public class Financien
    {
        public int Id { get; set; }
        public int OvereenkomstId { get; set; } // Foreign key naar Overeenkomst
        [JsonIgnore]
        public Overeenkomst? Overeenkomst { get; set; } = null!; // one-to-one relatie
        public float Bedrag {  get; set; }
        public FactureringsWijze FactureringsWijze {  get; set; } 
        public FactureringsPeriode FactureringsPeriode { get; set; }
        public ICollection<Factuur> Facturen { get; } = new List<Factuur>(); // one-to-many relatie
        public Frequentie Frequentie { get; set; }
    }

    public enum FactureringsWijze
    {
        Vooraf,
        Achteraf,
        Halverwege,
        Eenmalig
    }

    public enum Frequentie
    {
        Maandelijks,
        Halfjaarlijks,
        Jaarlijks
    }

    public enum FactureringsPeriode
    { 
        Juni,
        December
    }

    public class FinancienDto
    {
        public float Bedrag { get; set; }
        public FactureringsWijze FactureringsWijze { get; set; }
        public FactureringsPeriode FactureringsPeriode { get; set; }
        public Frequentie Frequentie { get; set; }
    }
}
