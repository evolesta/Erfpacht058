using System.Text.Json.Serialization;

namespace Erfpacht058_API.Models.Rapport
{
    public class TaskQueue
    {
        public int Id { get; set; }
        public SoortTaak SoortTaak { get; set; }
        public Status Status { get; set; }
        public Prioriteit Prioriteit { get; set; }
        public string? Fout { get; set; }
        public DateTime AanmaakDatum { get; set; }
        public DateTime AfgerondDatum { get; set; }
        public int? ExportId { get; set; } // Optionele one-to-one relatie
        [JsonIgnore]
        public Export? Export { get; set; }
    }

    public enum SoortTaak
    {
        Import,
        Export
    }

    public enum Status
    {
        Nieuw,
        InBehandeling,
        Succesvol,
        Mislukt,
        Verwijderd
    }

    public enum Prioriteit
    {
        Laag,
        Midden,
        Hoog
    }
}
