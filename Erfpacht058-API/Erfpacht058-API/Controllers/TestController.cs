using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Erfpacht058_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // Publiek toegankelijke controller endpoint voor het uitvoeren van integratietests
    public class TestController : ControllerBase
    {
        /// <summary>
        /// Geeft een test response terug
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> TestResponse()
        {
            return Ok("Succesvolle response");
        }


    }
}
