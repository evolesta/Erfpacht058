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

namespace Erfpacht058_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Beheerder")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        // Deze controller beheert de generieke instellingen van de applicatie
        // De settings staan in een enkele rij met PK = 1
        // Daarom is er enkel een GET en PUT endpoint aanwezig

        private readonly Erfpacht058_APIContext _context;

        public SettingsController(Erfpacht058_APIContext context)
        {
            _context = context;
        }

        // GET: api/Settings
        [HttpGet]
        public async Task<ActionResult<Settings>> GetSettings()
        {
            // Geef altijd de eerste rij terug
            return await _context.Settings.FindAsync(1);
        }

        // PUT: api/Settings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutSettings(SettingsDto settingsDto)
        {
            // Verkrijg de rij met settings, altijd met PK 1
            var settings = await _context.Settings.FindAsync(1);

            // werk object bij
            settings.BAGAPI = settingsDto.BAGAPIkey;

            // Werk bij in context
            _context.Entry(settings).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(settings);
        }
    }
}
