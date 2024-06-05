using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Eigendom;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Erfpacht058_API.Controllers.Eigendom
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class KadasterController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly IConfiguration _configuration;

        public KadasterController(Erfpacht058_APIContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration; 
        }

        // GET: /kadaster
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kadaster>>> GetKadaster()
        {
            return await _context.Kadaster.ToListAsync();
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
            var kadasterAPI = new KadasterAPIServiceContext(new KadasterDemoService(_configuration["Kadaster:JsonFile"])); // Selecteer demo strategy
            var kadasterData = await kadasterAPI.RetrieveSingleDataAsync(kadaster.KadastraalNummer);
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
