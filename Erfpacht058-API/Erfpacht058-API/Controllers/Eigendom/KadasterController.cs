using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Eigendom;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Erfpacht058_API.Controllers.Eigendom
{
    // Strategy pattern toegepast zodat in een later stadium de Kadaster API eenvoudig geimplementeerd kan worden met weinig refactoring van code
    public interface IKadasterAPIService
    {
        Task<Dictionary<string, object>> RetrieveSingleDataAsync(string id); // Functie die een single Kadaster object teruggeeft
    }

    // Helper klasse om de Kadaster data op te halen
    public class KadasterAPIService : IKadasterAPIService
    {
        private readonly string _filepath;

        public KadasterAPIService(string filepath)
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

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class KadasterController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly IKadasterAPIService _kadasterAPIService;

        public KadasterController(Erfpacht058_APIContext context, IKadasterAPIService kadasterAPIService)
        {
            _context = context;
            _kadasterAPIService = kadasterAPIService;
        }

        // POST: /kadaster/sync/5
        /// <summary>
        /// Synchroniseert data met het Kadaster a.d.h.v. Kadastrale nummer
        /// </summary>
        /// <param name="kadasterId"></param>
        /// <returns></returns>
        [HttpPost("sync/{kadasterId}")]
        public async Task<ActionResult<Kadaster>> SyncMetKadaster(int kadasterId)
        {
            // Verkrijg het Kadaster object uit de database
            var kadaster = await _context.Kadaster.FindAsync(kadasterId);
            if (kadaster == null) return BadRequest();
            if (kadaster.KadastraalNummer == null) return BadRequest(); // Kadast. nr. mag niet null zijn

            // Verkrijg de informatie uit het Kadaster
            var kadasterData = await _kadasterAPIService.RetrieveSingleDataAsync(kadaster.KadastraalNummer);
            if (kadasterData == null) return BadRequest();  

            // Wijzig heb object en sla op in database
            kadaster.Deeloppervlakte = float.Parse(kadasterData["deeloppervlakte"].ToString());
            kadaster.KadastraleGrootte = float.Parse(kadasterData["kadastraleGrootte"].ToString());
            kadaster.ObjectType = kadasterData["objectType"].ToString();
            kadaster.LaatsteSynchronisatie = DateTime.Now; // Werk laatste sync datum bij

            _context.Entry(kadaster).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(kadaster);
        }
    }
}
