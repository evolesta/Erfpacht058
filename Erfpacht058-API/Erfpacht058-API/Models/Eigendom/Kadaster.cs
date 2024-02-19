namespace Erfpacht058_API.Models.Eigendom
{
    // Relationeel model van Eigendom
    public class Kadaster
    {
        public int Id { get; set; }
        public string KadastraalNummer { get; set; }
        public float Deeloppervlakte { get; set; }
        public float KadastraleGrootte { get; set; }
        public string ObjectType { get; set; }
    }
}
