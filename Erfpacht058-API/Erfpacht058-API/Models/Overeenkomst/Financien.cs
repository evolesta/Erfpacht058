namespace Erfpacht058_API.Models.Overeenkomst
{
    // Relationeel model van Overeenkomst
    public class Financien
    {
        public int Id { get; set; }
        public int OvereenkomstId { get; set; } // Foreign key naar Overeenkomst
        public Overeenkomst? Overeenkomst { get; set; } = null!; // one-to-one relatie
        public DateTime Ingangsdatum { get; set; }
        public DateTime Einddatum { get; set; }
        public float Bedrag {  get; set; }
        public FactureringsWijze FactureringsWijze {  get; set; } 
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
}
