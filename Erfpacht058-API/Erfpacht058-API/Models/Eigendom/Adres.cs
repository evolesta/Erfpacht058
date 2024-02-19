namespace Erfpacht058_API.Models.Eigendom
{
    // Relationeel model onder Eigendom
    public class Adres
    {
        public int Id { get; set; }
        public string Straatnaam { get; set; }
        public int Huisnummer { get; set; }
        public string Toevoeging { get; set; }
        public string Postcode { get; set; }
        public string Woonplaats { get; set; }
    }
}
