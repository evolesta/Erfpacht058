using Erfpacht058_API.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using System.Text;

namespace Erfpacht058_API.Controllers.Eigendom
{
    // Strategy pattern toegepast zodat in een later stadium de Kadaster API eenvoudig geimplementeerd kan worden met weinig refactoring van code
    public interface IKadasterAPIService
    {
        Task<Dictionary<string, object>> RetrieveSingleDataAsync(string id); // Functie die een single Kadaster object teruggeeft
    }

    // Demo strategy voor het simuleren van een API adhv een JSON bestand
    public class KadasterDemoService : IKadasterAPIService
    {
        private readonly string _filepath;

        public KadasterDemoService(string filepath)
        {
            _filepath = filepath;
        }

        // Lees het JSON bestand met testdata uit om een API te simuleren
        public async Task<Dictionary<string, object>> RetrieveSingleDataAsync(string kadastNr)
        {
            // Lees het JSON data bestand uit
            var jsonContent = await File.ReadAllTextAsync(_filepath);

            // Converteer de string naar een List van Dictionaries
            var jsonCollection = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonContent);

            // Zoek het kadastrale nr. m.b.v. Linq
            var foundObject = jsonCollection.FirstOrDefault(dict =>
                dict["KadastraalNummer"].ToString() == kadastNr);

            return foundObject;
        }
    }

    // Strategy voor het bevragen van de BAG API service als tijdelijke implementatie voor de Kadaster API
    public class BAGAPIService : IKadasterAPIService
    {
        private readonly string _postcode;
        private readonly string _huisnummer;
        private readonly string _huisnummerToevoeging;
        private readonly string _huisletter;
        private readonly Erfpacht058_APIContext _context;
        private static readonly HttpClient _httpClient = new HttpClient();

        public BAGAPIService(string postcode, string huisnummer, Erfpacht058_APIContext context, string huisnummerToevoeging = "", string huisletter = "")
        {
            _postcode = postcode;
            _huisnummer = huisnummer;
            _huisnummerToevoeging = huisnummerToevoeging;
            _huisletter = huisletter;
            _context = context;
        }

        public async Task<Dictionary<string, object>> RetrieveSingleDataAsync(string id)
        {
            // Verkrijg API sleutel
            var settings = await _context.Settings.FindAsync(1);
            var BAGAPIKey = settings.BAGAPI;
            if (BAGAPIKey == null) throw new Exception("Geen BAG API sleutel gevonden");

            // Stel zoek parameters samen met String builder
            var sb = new StringBuilder("?");
            sb.Append("postcode=" + _postcode); // verplicht
            sb.Append("&huisnummer=" + _huisnummer); // verplicht
            if (!string.IsNullOrEmpty(_huisnummerToevoeging)) sb.Append("&huisnummertoevoeging=" + _huisnummerToevoeging); // optioneel, alleen toevoegen wanneer deze niet null is
            if (!string.IsNullOrEmpty(_huisletter)) sb.Append("&huisletter=" + _huisletter); // optioneel, alleen toevoegen wanneer deze niet null is

            // Vraag informatie op bij de BAG API
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", BAGAPIKey); // Voeg de API sleutel toe aan de headers
            var response = await _httpClient.GetStringAsync("https://api.bag.kadaster.nl/lvbag/individuelebevragingen/v2/adressen" + sb.ToString());

            // Converteer naar object en geef terug als Dictionary
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
        }
    }

    //Todo: Implementeer strategy voor bevragen Kadaster API
    public class KadasterAPIService : IKadasterAPIService
    {
        public KadasterAPIService()
        {
               // empty constructor
        }

        public async Task<Dictionary<string, object>> RetrieveSingleDataAsync(string kadastNr)
        {
            // Todo -- later uitwerken
            throw new NotImplementedException();
        }
    }

    // Context Klasse voor het selecteren van een strategy
    public class KadasterAPIServiceContext
    {
        private IKadasterAPIService _kadasterAPIService;

        public KadasterAPIServiceContext(IKadasterAPIService kadasterAPIService)
        {
            _kadasterAPIService = kadasterAPIService;
        }

        public void SetKadasterAPIServiceStrategy(IKadasterAPIService kadasterAPIService)
        {
            _kadasterAPIService = kadasterAPIService;
        }

        public async Task<Dictionary<string, object>> RetrieveSingleDataAsync(string kadasterNr)
        {
            return await _kadasterAPIService.RetrieveSingleDataAsync(kadasterNr);
        }
    }
}
