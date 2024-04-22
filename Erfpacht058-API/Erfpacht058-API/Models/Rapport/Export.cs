namespace Erfpacht058_API.Models.Rapport
{
    public class Export
    {
        public int Id { get; set; }
        public Formaat Formaat { get; set; }
        public TaskQueue Task {  get; set; } // Optionele one-to-one relatie
        public Template Template { get; set; } // One-to-one relatie
        public DateTime AanmaakDatum { get; set; }
        public Gebruiker Gebruiker { get; set; } // one-to-one relatie
    }

    public enum Formaat
    {
        PDF,
        Excel,
        CSV
    }
}
