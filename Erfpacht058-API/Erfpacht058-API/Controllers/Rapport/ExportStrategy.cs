using Erfpacht058_API.Models.Rapport;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Erfpacht058_API.Controllers.Rapport
{
    // Interface voor de strategieen om te implementeren
    public interface IExportStrategy
    {
        string GenerateFile(Dictionary<string, List<object>> data, IConfiguration config, Export export);
    }

    // ++ Klassen voor de concrete strategieen
    // Klasse voor het genereren van een PDF
    public class PDFExportStrategy : IExportStrategy
    {
        public string GenerateFile(Dictionary<string, List<object>> data, IConfiguration config, Export export)
        {
            // Verkrijg het aantal rijen die in de tabel komen te staan
            var amountRows = data.First().Value.Count;

            // Genereer een PDF document mbv de Quest PDF Library - zie https://www.questpdf.com/getting-started.html
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    // PDF Pagina settings
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(QuestPDF.Helpers.Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    // Pagina header
                    page.Header()
                        .Text("Export: " + export.Id.ToString() + " - " + export.Template.Naam)
                        .SemiBold().FontSize(28).FontColor(QuestPDF.Helpers.Colors.Black);

                    // PDF Tabel met data opstellen
                    page.Content().Element(compose =>
                    {
                        // Stel tabel met data op
                        compose.Table(table =>
                        {
                            // Tabel opmaak
                            IContainer CellStyle(IContainer container)
                            {
                                return container
                                .Border(1)
                                .BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1)
                                .Background(QuestPDF.Helpers.Colors.White)
                                .PaddingVertical(5)
                                .PaddingHorizontal(10)
                                .AlignCenter()
                                .AlignMiddle();
                            }

                            // Definieer alle kolommen op basis van het aantal kolommen in de dict.
                            table.ColumnsDefinition(columns =>
                            {
                                for (var i = 0; i < data.Count; i++)
                                {
                                    columns.RelativeColumn();
                                }
                            });

                            // Stel dynamisch kolomnamen op in de eerste rij
                            table.Header(header =>
                            {
                                foreach (var column in data)
                                {
                                    header.Cell()
                                    .Element(CellStyle)
                                    .Text(column.Key)
                                    .Bold();
                                }
                            });

                            // Loop door de rijen heen en stel een collectie cellen op
                            for (var i = 0; i < amountRows; i++) 
                            {
                                foreach (var column in data)
                                {
                                    table.Cell()
                                    .Element(CellStyle)
                                    .Text(column.Value[i].ToString());
                                }
                            }
                        });
                    });
                });
            });

            // Genereer een PDF bestand en geef het pad terug
            var filename = config["Bestanden:ExportPad"] + "/Export-" + export.Id.ToString() + " - " + export.Template.Naam + ".pdf";
            document.GeneratePdf(filename);

            return filename;
        }
    }

    // Klasse voor het genereren van een CSV
    public class CSVExportStrategy : IExportStrategy
    {
        public string GenerateFile(Dictionary<string, List<object>> data, IConfiguration config, Export export)
        {
            // Gebruik stringBuilder om een CSV string te genereren
            StringBuilder csvContent = new StringBuilder();
            string delimeter = ",";

            // Kolomnamen toevoegen aan de eerste rij
            csvContent.AppendLine(string.Join(delimeter, data.Keys));

            // Rijen toevoegen met data
            int aantalRijen = data.First().Value.Count;
            for (int i = 0; i < aantalRijen; i++)
            {
                csvContent.AppendLine(string.Join(delimeter, data.Select(col => col.Value[i])));
            }

            // Bestand wegschrijven
            var filename = config["Bestanden:ExportPad"] + "/Export-" + export.Id.ToString() + " - " + export.Template.Naam + ".csv";
            File.WriteAllText(filename, csvContent.ToString());

            return filename;
        }
    }

    // Klasse voor het genereren van een Excel
    public class ExcelExportStrategy : IExportStrategy
    {
        public string GenerateFile(Dictionary<string, List<object>> data, IConfiguration config, Export export)
        {
            var filename = config["Bestanden:ExportPad"] + "/Export-" + export.Id.ToString() + " - " + export.Template.Naam + ".xlsx";

            // Gebruik OpenXML om een Excel bestand te genereren
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filename, SpreadsheetDocumentType.Workbook))
            {
                // Creeer een nieuw Workbook object
                WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                // Creeer een nieuwe sheet
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                // Sheet toevoegen aan workbook
                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
                Sheet sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Export " + export.Template.Naam
                };
                sheets.Append(sheet);

                // Verkrijg de SheetData cel tabel
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                // Stel eerste rij met kolomnamen samen
                Row headerRow = new Row();
                foreach (var columnName in data.Keys)
                {
                    headerRow.Append(new Cell() { CellValue = new CellValue(columnName), DataType = CellValues.String });
                }
                sheetData.Append(headerRow);

                // Aantal rijen bepalen
                int aantalRijen = data.First().Value.Count;

                // Rijen toevoegen aan Excel
                for (int i = 0; i < aantalRijen; i++)
                {
                    Row newRow = new Row();
                    foreach (var column in data)
                    {
                        newRow.Append(new Cell() { CellValue = new CellValue(column.Value[i].ToString()), DataType = CellValues.String });
                    }
                    sheetData.Append(newRow);
                }

                // Excel opslaan
                workbookPart.Workbook.Save();

                // Sluiten
                spreadsheetDocument.Dispose();
            }

            return filename;
        }
    }

    // ++ Context klasse voor het gebruiken van de strategieeen
    public class ExportStrategyContext
    {
        private IExportStrategy _exportStrategy;

        public ExportStrategyContext(IExportStrategy exportStrategy)
        {
            _exportStrategy = exportStrategy;
        }

        public void SetExportStrategy(IExportStrategy exportStrategy)
        {
            _exportStrategy = exportStrategy;
        }

        public string GenerateFile(Dictionary<string, List<object>> data, IConfiguration config, Export export) 
        {
            return _exportStrategy.GenerateFile(data, config, export);
        }
    }
}
