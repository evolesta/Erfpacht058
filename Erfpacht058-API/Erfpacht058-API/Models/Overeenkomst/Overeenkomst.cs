namespace Erfpacht058_API.Models.Overeenkomst
{
    public class Overeenkomst
    {
        public int Id { get; set; }
        public string Dossiernummer {  get; set; }
        public Gebruiker Gebruiker { get; set; }
        public DateTime Ingangsdatum { get; set; }
        public DateTime Einddatum { get; set; }
        public float Grondwaarde {  get; set; }
        public DateTime DatumAkte {  get; set; }
        public float Rentepercentage { get; set; }
        public Financien Financien { get; set; }
    }
}
