namespace Erfpacht058_API.Models.Eigendom
{
    // Relationeel model voor Eigendom
    public class Herziening
    {
        public int Id { get; set; }
        public DateTime Herzieningsdatum { get; set; }
        public int VolgendeHerziening { get; set; }
    }
}
