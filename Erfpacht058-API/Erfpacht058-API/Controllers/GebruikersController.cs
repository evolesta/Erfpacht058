using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models;
using Microsoft.AspNetCore.Authorization;
using Erfpacht058_API.Repositories.Interfaces;

namespace Erfpacht058_API.Controllers
{
    // Usermanagement van de applicatie, alleen toegankelijk voor gebruikers met de rol 'Beheerder'
    [Route("api/[controller]")]
    [Authorize(Roles = "Beheerder")]
    [ApiController]
    public class GebruikersController : ControllerBase
    {
        private readonly Erfpacht058_APIContext _context;
        private readonly IGebruikerRepository _gebruikerRepository;

        public GebruikersController(Erfpacht058_APIContext context, IGebruikerRepository gebruikerRepository)
        {
            _context = context;
            _gebruikerRepository = gebruikerRepository;
        }

        // GET: api/Gebruikers
        /// <summary>
        /// Verkrijg alle gebruikers (admin rol)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GebruikerDto>>> GetGebruiker()
        {
            return Ok(await _gebruikerRepository.GetGebruikers());
        }

        // GET: api/Gebruikers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GebruikerDto>> GetGebruiker(int id)
        {
            return Ok(await _gebruikerRepository.GetGebruiker(id));
        }

        // PUT: api/Gebruikers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Wijzig een bestaande gebruiker (admin rol) (let op: geen wachtwoord!)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gebruikerDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGebruiker(int id, GebruikerDto gebruikerDto)
        {
            var result = await _gebruikerRepository.EditGebruiker(id, gebruikerDto);

            if (result != null) return Ok(result);
            else return NotFound();
        }

        // PUT: api/Gebruikers/wachtwoord/5
        /// <summary>
        /// Wijzig het wachtwoord van een gebruiker (admin rol)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="wachtwoord"></param>
        /// <returns></returns>
        [HttpPut("wachtwoord/{id}")]
        public async Task<ActionResult> UpdateWachtwoord(int id, WachtwoordDto wachtwoord)
        {
            var result = await _gebruikerRepository.EditWachtwoord(id, wachtwoord);

            if (result != null) return Ok(result);
            else return NotFound();
        }

        // POST: api/Gebruikers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Gebruiker>> PostGebruiker(GebruikerDto gebruikerDto)
        {
            return Ok(await _gebruikerRepository.AddGebruiker(gebruikerDto));
        }

        // DELETE: api/Gebruikers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGebruiker(int id)
        {
            var result = await _gebruikerRepository.DeleteGebruiker(id);

            if (result != null) return Ok();
            else return NotFound(); 
        }
    }
}
