using System;
using System.IO;
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
using AutoMapper;

namespace Erfpacht058_API.Controllers.Eigendom
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BestandController : ControllerBase
    {
        private readonly IBestandRepository _bestandRepo;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public BestandController(IBestandRepository bestandRepository, IConfiguration configuration, IMapper mapper)
        {
            _bestandRepo = bestandRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        // GET: api/Bestand/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bestand>> GetBestand(int id)
        {
            var bestand = await _bestandRepo.GetBestand(id);

            if (bestand != null) return Ok(bestand);
            else return NotFound();
        }

        // GET: /api/bestand/download/5
        /// <summary>
        /// Download een bestand a.d.h.v. het ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>octet-stream</returns>
        /// <response code="200">Een octet-stream (download) met het gevraagde bestand</response>
        [HttpGet("download/{id}")]
        public async Task<ActionResult> DownloadBestand(int id)
        {
            // Verkrijg bestand van repo
            var bestand = await _bestandRepo.GetBestand(id);

            if (bestand == null)
                return NotFound();

            // Verkrijg bestandspad naar bestand
            var basePath = _configuration["Bestanden:OpslagPad"];
            var filepath = basePath + bestand.Pad;
            if (!System.IO.File.Exists(filepath)) return BadRequest();

            // Creeer een memoryStream om het bestand als download aan te bieden
            var memory = new MemoryStream();
            await using(var stream = new FileStream(filepath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            // Zet content-Disposition om een download te forceren in de response
            var contentDisposition = new System.Net.Mime.ContentDisposition
            {
                FileName = Path.GetFileName(filepath),
                Inline = false
            };
            Response.Headers.Add("Content-Disposition", contentDisposition.ToString()); // voeg toe aan headers

            return File(memory, "application/octet-stream");
        }

        // PUT: api/Bestand/5
        /// <summary>
        /// Wijzig een bestaand bestand
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bestandDto"></param>
        /// <returns></returns> 
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBestand(int id, BestandDto bestandDto)
        {
            var result = await _bestandRepo.UpdateBestand(id, bestandDto);

            if (result != null) return Ok(result);
            else return NotFound();
        }

        // DELETE: api/Bestand/5
        /// <summary>
        /// Verwijder een bestand
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBestand(int id)
        {
            var result = await _bestandRepo.DeleteBestand(id);

            if (result != null)
            {
                // Verwijder bestand uit storage
                var basePath = _configuration["Bestanden:OpslagPad"];
                var filepath = basePath + result.Pad;
                System.IO.File.Delete(filepath);

                return NoContent();
            }
            else
                return NotFound();
        }
    }
}
