using Erfpacht058_API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Erfpacht058_API.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class AuthenticatieController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly Erfpacht058_APIContext _context;

        public AuthenticatieController(Erfpacht058_APIContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Authenticatie endpoint om een jwt toegangstoken te genereren
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> authenticate(Credentials credentials)
        {
            // Controleer of het emailadres en wachtwoord aanwezig zijn in de body van het request
            if (credentials.Emailadres == null || credentials.Wachtwoord == null)
                return BadRequest("Geen valide credentials opgegeven.");

            // Verkrijg gebruiker uit Database
            var gebruiker = await _context.Gebruiker.FirstOrDefaultAsync(u => u.Emailadres == credentials.Emailadres);

            // Controleer of de gebruiker is gevonden
            if (gebruiker == null)
                return BadRequest("Geen valide credentials opgegeven.");

            // Controleer of het wachtwoord juist is
            if (BCrypt.Net.BCrypt.Verify(credentials.Wachtwoord, gebruiker.Wachtwoord))
            {
                // Wachtwoord is correct
                var JwtToken = GenerateJwt(gebruiker.Emailadres, gebruiker.Role.ToString());
                return Ok(new {token = JwtToken});
            }
            else
                return BadRequest("Geen valide credentials opgegeven."); // Geen correct wachtwoord
        }

        // Helper functie voor het genereren van een jwt token
        private string GenerateJwt(string username, string role)
        {
            var tokenkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var SignIn = new SigningCredentials(tokenkey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["JWT:Issuer"],
                _configuration["JWT:Audience"],
                claims: new[]
                {
                        new Claim("Username", username),
                        new Claim(ClaimTypes.Role, role),
                },
                expires: DateTime.Now.AddSeconds(Convert.ToDouble(_configuration["JWT:ExpirationInSeconds"])),
                signingCredentials: SignIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class Credentials
    {
        public string Emailadres { get; set; }
        public string Wachtwoord { get; set; }
    }
}
