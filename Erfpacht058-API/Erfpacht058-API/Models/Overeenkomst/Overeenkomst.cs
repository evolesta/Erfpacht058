namespace Erfpacht058_API.Models.Overeenkomst
{
    using Erfpacht058_API.Models.Eigendom;
    public class Overeenkomst
    {
        public int Id { get; set; }
        public Eigendom? Eigendom { get; set; } // many-to-one relatie
        public string? Dossiernummer {  get; set; }
        public DateTime Ingangsdatum { get; set; }
        public DateTime Einddatum { get; set; }
        public float Grondwaarde {  get; set; }
        public DateTime DatumAkte {  get; set; }
        public float Rentepercentage { get; set; }
        public Financien? Financien { get; set; } // required one-to-one relatie
    }
}
