using System.Text.Json.Serialization;

namespace Erfpacht058_API.Models.Rapport
{
    public class TranslateModel
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Maker { get; set; }
        public string Model { get; set; }
        public int? ImportId { get; set; }
        public Import? Import { get; set; } // one-to-one relatie
        public ICollection<Translation>? Translations { get; } = new List<Translation>(); // optionele one-to-many relatie
        public DateTime AanmaakDatum { get; set; }
        public DateTime? WijzigingsDatum { get; set; }
    }

    public class TranslateModelDto
    {
        public string Maker { get; set; }
        public string Naam {  set; get; }
        public string Model { get; set; }
        public List<TranslationDto>? Translations { get; set; }
    }

    public class Translation
    {
        public int Id { get; set; }
        public string CSVColummnName { get; set; }
        public string ModelColumnName { get; set; }
        public int? TranslateModelId { get; set; }
        [JsonIgnore]
        public TranslateModel? TranslateModel { get; set; }
    }

    public class TranslationDto
    {
        public int Id { get; set; }
        public string CSVColummnName { get; set; }
        public string ModelColumnName { get; set; }
    }
}
