namespace Erfpacht058_API.Models.Eigendom
{
    // Relationeel model voor Eigendom
    public class Herziening
    {
        public int Id { get; set; }
        public int EigendomId { get; set; } // Foreign key naar eigendom
        public Eigendom Eigendom { get; set; } = null!; // one-to-one relatie
        public DateTime Herzieningsdatum { get; set; }
        public int VolgendeHerziening { get; set; }
    }
}
