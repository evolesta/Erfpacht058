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
    [Route("api/[controller]")]
    [Authorize(Roles = "Beheerder")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        // Deze controller beheert de generieke instellingen van de applicatie
        // De settings staan in een enkele rij met PK = 1
        // Daarom is er enkel een GET en PUT endpoint aanwezig

        private readonly ISettingsRepository _settingsRepo;

        public SettingsController(ISettingsRepository settingsRepo)
        {
            _settingsRepo = settingsRepo;
        }

        // GET: api/Settings
        [HttpGet]
        public async Task<ActionResult<Settings>> GetSettings()
        {
            // Geef altijd de eerste rij terug
            return await _settingsRepo.GetSettings();
        }

        // PUT: api/Settings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutSettings(SettingsDto settingsDto)
        {
            var result = await _settingsRepo.EditSettings(settingsDto);

            return Ok(result);
        }
    }
}
