namespace Erfpacht058_API.Models.Rapport
{
    public class Import
    {
        public int Id { get; set; }
        public TaskQueue? Task { get; set; } // one-to-one relatie
        public TranslateModel TranslateModel { get; set; } // one-to-one relatie
        public DateTime Aanmaakdatum { get; set; }
        public DateTime WijzigingsDatum { get; set; }
        public Gebruiker Gebruiker { get; set; } // one-to-one relatie
        public string importPad { get; set; }
    }

    public class ImportDto
    {
        public TranslateModel TranslateModel { get; set; }
    }
}
