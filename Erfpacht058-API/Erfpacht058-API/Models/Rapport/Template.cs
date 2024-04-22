using System.Text.Json.Serialization;

namespace Erfpacht058_API.Models.Rapport
{
    public class Template
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Maker { get; set; }
        public string Model { get; set; } // Naam van het hoofdmodel waar data uit vergaart moet worden
        public List<RapportData>? RapportData { get; } = new List<RapportData>(); // One-to-many relatie
        public List<Filter>? Filters { get; } = new List<Filter>(); // One-to-Many relatie
        public int? ExportId { get; set; } // one-to-one relatie
        public Export? Export { get; set; }
        public DateTime AanmaakDatum { get; set; }
        public DateTime WijzigingsDatum { get; set; }
    }

    public enum Model
    {
        Eigendom,
        Adres,
        Eigenaar,
        Herziening,
        Overeenkomst,
        Bestands,
        Kadaster,
        Gebruiker
    }

    public class TemplateDto
    {
        public string Naam { get; set; }
        public string Maker { get; set; }
        public string Model { get; set; } // Naam van het hoofdmodel waar data uit vergaart moet worden
        public List<RapportDataDto>? RapportData { get; set; }
        public List<FilterDto>? Filters { get; set; }
    }

    public class RapportData
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Naam { set; get; }
        public int TemplateId { get; set; } // One-to-many relatie
        [JsonIgnore]
        public Template Template { get; set; }
    }

    public class RapportDataDto
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Naam { set; get; }
    }

    public class Filter
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public Operator Operation { get; set; }
        public string Value { get; set; }
        public int TemplateId { set; get; } // one-to-many relatie
        [JsonIgnore]
        public Template Template {  set; get; }
    }

    public enum Operator
    {
        Equal,
        NotEqual,
        GreaterThen,
        LessThen,
        GreaterThanEqual,
        LessThanEqual
    }

    public class FilterDto
    {
        public int Id {  set; get; }
        public string Key { get; set; }
        public Operator Operation { get; set; }
        public string Value { get; set; }
    }
}
