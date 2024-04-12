using Erfpacht058_API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
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
        /// Ontvangen een Jwt Bearer token voor beveiligde endpoints
        /// </summary>
        /// <remarks>Voorbeeld:
        /// 
        /// POST api/token
        /// {
        ///     "emailadres": "test@gebruiker.nl",
        ///     "wachtwoord": "mijnwachtwoord"
        /// }</remarks>
        /// <param name="credentials"></param>
        /// <returns>JWT Bearer token</returns>
        /// <response code="200">Een JWT Access bearer token voor beveiligde endpoint routes</response>
        [HttpPost]
        public async Task<IActionResult> authenticate(Credentials credentials)
        {
            // Controleer of het emailadres en wachtwoord aanwezig zijn in de body van het request
            if (!ModelState.IsValid)
                return BadRequest("Geen valide credentials opgegeven.");

            // Verkrijg gebruiker uit Database
            var gebruiker = await _context.Gebruiker.FirstOrDefaultAsync(u => u.Emailadres == credentials.Emailadres);

            // Controleer of de gebruiker is gevonden
            if (gebruiker == null)
                return BadRequest("Geen valide credentials opgegeven.");

            // Controleer max. loging Pogingen
            if (gebruiker.LogingPoging >= 3)
            {
                // Zet gebruiker inactief
                gebruiker.Actief = false;
                _context.Entry(gebruiker).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return BadRequest();
            }

            // Controleer of gebruiker actief is
            if (!gebruiker.Actief)
                return BadRequest("Inactief");

            // Controleer of het wachtwoord juist is
            if (BCrypt.Net.BCrypt.Verify(credentials.Wachtwoord, gebruiker.Wachtwoord))
            {
                // Wachtwoord is correct
                // Reset login pogingen
                gebruiker.LogingPoging = 0;
                _context.Entry(gebruiker).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                
                // Genereer token en geef terug
                var naam = gebruiker.Voornamen + " " + gebruiker.Naam;
                var JwtToken = GenerateJwt(gebruiker.Emailadres, naam, gebruiker.Role.ToString());

                return Ok(new {token = JwtToken});
            }
            else
            {
                // Wachtwoord is incorrect
                gebruiker.LogingPoging++; // Verhoog verk. loging pogingen
                _context.Entry(gebruiker).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return BadRequest("Geen valide credentials opgegeven."); // Geen correct wachtwoord
            }
            
        }

        // Helper functie voor het genereren van een jwt token
        private string GenerateJwt(string username, string naam, string role)
        { 
            var tokenkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var SignIn = new SigningCredentials(tokenkey, SecurityAlgorithms.HmacSha256);
            var currentTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();
            var token = new JwtSecurityToken(
                _configuration["JWT:Issuer"],
                _configuration["JWT:Audience"],
                claims: new[]
                {
                        new Claim("Username", username),
                        new Claim("Naam", naam),
                        new Claim(ClaimTypes.Role, role),
                        new Claim("Role", role),
                        new Claim("Login", currentTime),
                },
                expires: DateTime.Now.AddSeconds(Convert.ToDouble(_configuration["JWT:ExpirationInSeconds"])),
                signingCredentials: SignIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // Dto voor het authenticeren op de API
    public class Credentials
    {
        [Required]
        public string Emailadres { get; set; }
        [Required]
        public string Wachtwoord { get; set; }
    }
}
