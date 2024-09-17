using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erfpacht058_API.Data;
using Erfpacht058_API.Models.Eigendom;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Authorization;
using Erfpacht058_API.Repositories.Interfaces;
using AutoMapper;

namespace Erfpacht058_API.Controllers.Eigendom
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EigenaarController : ControllerBase
    {
        private readonly IEigenaarRepository _eigenaarRepo;
        private readonly IMapper _mapper;

        public EigenaarController(IEigenaarRepository eigenaarRepository, IMapper mapper)
        {
            _eigenaarRepo = eigenaarRepository;
            _mapper = mapper;
        }

        // GET: api/Eigenaar
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Eigenaar>>> GetEigenaar()
        {
            return Ok(await _eigenaarRepo.GetEigenaren());
        }

        // GET: api/Eigenaar/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Eigenaar>> GetEigenaar(int id)
        {
           var eigenaar = await _eigenaarRepo.GetEigenaar(id);

            if (eigenaar != null) 
                return Ok(eigenaar);
            else
                return BadRequest();
        }

        // POST: api/Eigenaar
        /// <summary>
        /// Voeg een nieuwe eigenaar toe
        /// </summary>
        /// <param name="id"></param>
        /// <param name="eigenaarDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Eigenaar>> PostEigenaar(EigenaarDto eigenaarDto)
        {
            // Map Dto naar object
            var eigenaar = _mapper.Map<Eigenaar>(eigenaarDto);

            await _eigenaarRepo.AddEigenaar(eigenaar);

            return Ok(eigenaar);
        }

        // PUT: api/Eigenaar/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Eigenaar>> PutEigenaar(int id, EigenaarDto eigenaarDto)
        {
            var result = await _eigenaarRepo.EditEigenaar(id, eigenaarDto);

            if (result != null)
                return Ok(result);
            else
                return BadRequest();
        }

        // DELETE: api/Eigenaar/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEigenaar(int id)
        {
            await _eigenaarRepo.DeleteEigenaar(id);

            return NoContent();
        }
    }
}
 