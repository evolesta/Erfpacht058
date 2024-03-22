namespace Erfpacht058_API.Models
{
    using Erfpacht058_API.Models.Eigendom;
    using Microsoft.AspNetCore.Mvc;
    using System.Text.Json.Serialization;

    public class Bestand
    {
        public int Id { get; set; }
        [JsonIgnore]
        public Eigendom.Eigendom? Eigendom { get; set; } // many-to-one relatie
        public string? Naam {  get; set; }
        public long GrootteInKb { get; set; }
        public SoortBestand SoortBestand { get; set; }
        public string? Beschrijving {  get; set; }
        public string? Pad {  get; set; }
    }

    public enum SoortBestand
    {
        Algemeen,
        Notitie,
        Bewijsstuk,
        Overeenkomst,
        Overig
    }

    public class BestandDtoUpload
    {
        public List<IFormFile> Files { get; set; }
    }

    public class BestandDto
    {
        public string? Naam { get; set; }
        public SoortBestand? SoortBestand { get; set; }
        public string? Beschrijving { get; set; }
    }
}
