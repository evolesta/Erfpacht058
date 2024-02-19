namespace Erfpacht058_API.Models
{
    public class Bestand
    {
        public string bestandsId { get; set; }
        public string Naam {  get; set; }
        public int GrootteInKb { get; set; }
        public SoortBestand SoortBestand { get; set; }
        public string Beschrijving {  get; set; }
        public string Pad {  get; set; }
    }

    public enum SoortBestand
    {
        Algemeen,
        Notitie,
        Bewijsstuk,
        Overeenkomst,
        Overig
    }
}
