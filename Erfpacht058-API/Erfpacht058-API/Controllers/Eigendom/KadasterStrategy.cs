using Newtonsoft.Json;

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
