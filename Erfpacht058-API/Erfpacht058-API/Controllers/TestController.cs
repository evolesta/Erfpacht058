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

        [HttpGet("data")]
        public async Task<ActionResult<TestObject>> TestWithData()
        {
            var test = new TestObject
            {
                Name = "de Vries",
                FirstName = "Jan",
                Age = 30,
                Gender = "Man"
            };

            return Ok(test);
        }
    }

    public class TestObject
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
    }
}
