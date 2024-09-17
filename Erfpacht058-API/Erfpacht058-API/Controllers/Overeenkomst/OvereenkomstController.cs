using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erfpacht058_API.Data;

namespace Erfpacht058_API.Controllers.Overeenkomst
{
    using Erfpacht058_API.Models.OvereenkomstNS;
    using Microsoft.AspNetCore.Authorization;
    using Erfpacht058_API.Models.Eigendom;
    using Erfpacht058_API.Repositories.Interfaces;
    using AutoMapper;

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OvereenkomstController : ControllerBase
    {
        private readonly IOvereenkomstRepository _overeenkomstRepo;
        private readonly IMapper _mapper;

        public OvereenkomstController(IOvereenkomstRepository overeenkomstRepository, IMapper mapper)
        {
            _overeenkomstRepo = overeenkomstRepository;
            _mapper = mapper;
        }

        // GET: api/Overeenkomst
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Overeenkomst>>> GetOvereenkomst()
        {
            return Ok(await _overeenkomstRepo.GetOvereenkomsten());
        }

        // GET: api/Overeenkomst/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Overeenkomst>> GetOvereenkomst(int id)
        {
            var result = await _overeenkomstRepo.GetOvereenkomst(id);

            if (result != null) return Ok(result);
            else return NotFound();
        }

        // PUT: api/Overeenkomst/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOvereenkomst(int id, OvereenkomstDto overeenkomstDto)
        {
            var result = await _overeenkomstRepo.EditOvereenkomst(id, overeenkomstDto);

            if (result != null) return Ok(result);
            else return NotFound();
        }

        // DELETE: api/Overeenkomst/5
        /// <summary>
        /// Verwijder een bestaande overeenkomst en relaties
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Eigendom>> DeleteOvereenkomst(int id)
        {
            var result = await _overeenkomstRepo.DeleteOvereenkomst(id);

            if (result != null) return NoContent();
            else return NotFound();
        }
    }
}
