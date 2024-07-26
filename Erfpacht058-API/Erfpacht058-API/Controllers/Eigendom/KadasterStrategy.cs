using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Eigendom;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Erfpacht058_API.Controllers.Eigendom
{
    // Strategy pattern toegepast zodat in een later stadium de Kadaster API eenvoudig geimplementeerd kan worden met weinig refactoring van code
    public interface IKadasterAPIService
    {
        Task<Dictionary<string, object>> RetrieveSingleDataAsync(); // Functie die een single Kadaster object teruggeeft
    }

    // Strategy voor het bevragen van de BAG API service als tijdelijke implementatie voor de Kadaster API
    public class BAGAPIService : IKadasterAPIService
    {
        private readonly Adres _adres;
        private readonly Erfpacht058_APIContext _context;
        private readonly IHttpClientFactory _factory;

        public BAGAPIService(Adres adres, Erfpacht058_APIContext context, IHttpClientFactory factory)
        {
            _adres = adres;
            _factory = factory;
            _context = context;
        }

        public async Task<Dictionary<string, object>> RetrieveSingleDataAsync()
        {
            // Verkrijg API sleutel
            var settings = await _context.Settings.FindAsync(1);
            var BAGAPIKey = settings.BAGAPI;
            if (string.IsNullOrEmpty(BAGAPIKey)) 
                throw new Exception("Geen BAG API sleutel gevonden");

            // Stel zoek parameters samen met String builder
            var sb = new StringBuilder("?");
            sb.Append("postcode=" + _adres.Postcode); // verplicht
            sb.Append("&huisnummer=" + _adres.Huisnummer); // verplicht
            if (!string.IsNullOrEmpty(_adres.Toevoeging)) sb.Append("&huisnummertoevoeging=" + _adres.Toevoeging); // optioneel, alleen toevoegen wanneer deze niet null is
            if (!string.IsNullOrEmpty(_adres.Huisletter)) sb.Append("&huisletter=" + _adres.Huisletter); // optioneel, alleen toevoegen wanneer deze niet null is

            // Vraag informatie op bij de BAG API
            var httpClient = _factory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", BAGAPIKey); // Voeg de API sleutel toe aan de headers
            httpClient.DefaultRequestHeaders.Add("Accept-Crs", "epsg:28992");
            var response = await httpClient.GetStringAsync("https://api.bag.kadaster.nl/lvbag/individuelebevragingen/v2/adressenuitgebreid" + sb.ToString());

            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

            // check of er adressen zijn teruggegeven
            if (!dictionary.ContainsKey("_embedded"))
                throw new Exception("Geen adressen gevonden");

            // Verkrijg de collectie van adressen en check of er met de zoekopdracht slechts 1 resultaat is overgebleven
            var embedded = dictionary["_embedded"] as JObject;
            var BAGadres = embedded["adressen"].ToObject<List<Dictionary<string, object>>>();

            if (BAGadres.Count > 1)
                throw new Exception("Meerdere adressen gevonden, specificeer toevoeging en/of huisletter");

            return BAGadres[0]; // Geef het adres terug als dictionary
        }
    }

    //Todo: Implementeer strategy voor bevragen Kadaster API
    public class KadasterAPIService : IKadasterAPIService
    {
        public KadasterAPIService()
        {
               // empty constructor
        }

        public async Task<Dictionary<string, object>> RetrieveSingleDataAsync()
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

        public async Task<Dictionary<string, object>> RetrieveSingleDataAsync()
        {
            return await _kadasterAPIService.RetrieveSingleDataAsync();
        }
    }
}
