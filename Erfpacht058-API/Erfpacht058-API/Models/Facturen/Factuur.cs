using Erfpacht058_API.Models.Eigendom;
using Erfpacht058_API.Models.OvereenkomstNS;
using Erfpacht058_API.Models.Rapport;

namespace Erfpacht058_API.Models.Facturen
{
    public class Factuur
    {
        public int Id { get; set; }
        public int? FinancienId { get; set; } // optionele foreign key naar Financien
        public Financien? Financien { get; set; }
        public int EigenaarId { get; set; }
        public Eigenaar Eigenaar { get; set; } // many-to-one relatie - een factuur heeft 1 eigenaar
        public DateTime Datum { get; set; }
        public string Nummer { get; set; }
        public float Bedrag {  get; set; }
        public int FactuurJobId { get; set; }
        public FactuurJob FactuurJob { get; set; } // many-to-one relatie - een factuur heeft 1 factuurJob
        public ICollection<FactuurRegels> Regels { get; } = new List<FactuurRegels>(); // opt. one-to-many relatie

    }

    public class FactuurJob
    {
        public int Id { get; set; }
        public TaskQueue Task { get; set; }
        public DateTime AanmaakDatum { get; set; }
        public DateTime AfrondDatum { get; set; }
        public Gebruiker Gebruiker { get; set; }
        public FactureringsPeriode FactureringsPeriode { get; set; }
        public string StoragePad { get; set; }
        public ICollection<Factuur> Facturen { get; } = new List<Factuur>(); // many-to-one relatie - een factuurJob heeft meerdere facturen
    }

    public class FactuurJobDto
    {
        public FactureringsPeriode FactureringsPeriode { get; set; }
    }

    public class FactuurRegels
    {
        public int Id { get; set; }
        public int Aantal {  get; set; }
        public string Beschrijving { get; set; }
        public float Prijs { get; set; }
        public float Totaal { get; set; }
        public int FactuurId { get; set; } // 
        public Factuur Factuur { get; set; }
    }
}
