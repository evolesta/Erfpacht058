namespace Erfpacht058_API.Models.Eigendom
{
    public class Eigendom
    {
        public int Id { get; set; }
        public string EigendomNummer { get; set; }
        public Adres Adres { get; set; }
        public string Relatienummer { get; set; }
        public Eigenaar Eigenaar { get; set; }
        public DateTime Ingangsdatum { get; set; }
        public DateTime Einddatum { get; set; }
        public string Complexnummer { get; set; }
        public float EconomischeWaarde {  get; set; }
        public float VerzekerdeWaarde { get; set; }
        public Kadaster Kadaster { get; set; }
        public Erfpacht058_API.Models.Overeenkomst.Overeenkomst Overeenkomst { get; set; }
        public Herziening Herziening { get; set; }
        public string Notities { get; set; }
        public Bestand Bestand { get; set; }
    }
}
