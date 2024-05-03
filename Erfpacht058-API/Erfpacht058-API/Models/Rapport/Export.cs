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
        public string? ExportPad { get; set; } 
    }

    public class ExportDto
    {
        public Formaat Formaat { get; set; }
        public int TemplateId { get; set; } // One-to-one relatie
        public bool Start { get; set; } // Bepaald of de taak direct moet starten
    }

    public enum Formaat
    {
        PDF,
        Excel,
        CSV
    }
}
