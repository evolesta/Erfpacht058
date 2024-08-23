using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Eigendom;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol;
using Newtonsoft.Json.Linq;

namespace Erfpacht058_API.Controllers.Eigendom
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class KadasterController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public KadasterController(Erfpacht058_APIContext context, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        // GET: /kadaster
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kadaster>>> GetKadaster()
        {
            return await _context.Kadaster.ToListAsync();
        }

        // POST: /kadaster/sync/5
        /// <summary>
        /// Synchroniseert data met het Kadaster a.d.h.v. eigendom
        /// </summary>
        /// <param name="eigendomId"></param>
        /// <returns></returns>
        [HttpGet("sync/{eigendomId}")]
        public async Task<ActionResult<Kadaster>> SyncMetKadaster(int eigendomId)
        {
            // Verkrijg het Kadaster object uit de database
            var eigendom = await _context.Eigendom
                .Include(e => e.Kadaster)
                .Include(e => e.Adres)
                .FirstOrDefaultAsync(e => e.Id == eigendomId);

            Kadaster kadaster;

            // check of er al een Kadaster object is
            if (eigendom.Kadaster == null)
            {
                // Creeer nieuw Kadaster object
                kadaster = new Kadaster
                {
                    Eigendom = eigendom,
                    EigendomId = eigendom.Id
                };

                eigendom.Kadaster = kadaster;
                _context.Kadaster.Add(kadaster);
                await _context.SaveChangesAsync();
            }
            else
                kadaster = eigendom.Kadaster;

            // Verkrijg de informatie uit het Kadaster
            var adres = eigendom.Adres;
            var kadasterAPI = new KadasterAPIServiceContext(new BAGAPIService(adres, _context, _httpClientFactory)); // Selecteer demo strategy

            try
            {
                // verkrijg data van API
                var kadasterData = await kadasterAPI.RetrieveSingleDataAsync();

                // Werk Kadaster object bij
                kadaster.BAGID = kadasterData["nummeraanduidingIdentificatie"].ToString();
                kadaster.Bouwjaar = float.Parse((kadasterData["oorspronkelijkBouwjaar"] as JArray)[0].ToString());
                kadaster.Oppervlakte = float.Parse(kadasterData["oppervlakte"].ToString());
                kadaster.Gebruiksdoel = (kadasterData["gebruiksdoelen"] as JArray)[0].ToString();
                kadaster.LaatsteSynchronisatie = DateTime.Now;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            _context.Entry(kadaster).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(kadaster);
        }
    }
}
