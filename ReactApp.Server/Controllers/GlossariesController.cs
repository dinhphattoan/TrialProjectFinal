using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactApp.Server.Data;
using ReactApp.Server.DTO;
using ReactApp.Server.Entity;
using ReactApp.Server.Repository;

namespace ReactApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlossariesController(GlossaryRepository glossaryRepository,
        ILogger<GlossariesController> logger,
        UserManager<IdentityUser> userManager) : ControllerBase
    {
        private readonly GlossaryRepository _glossaryRepository = glossaryRepository;
        private readonly ILogger<GlossariesController> _logger = logger;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        [HttpGet("{startIndex}/{count}")]
        public async Task<ActionResult<IEnumerable<Glossary>>> GetGlossariesAtRange(int startIndex, int count)
        {
            try
            {
                IEnumerable<Glossary> glossaries = await _glossaryRepository
                    .GetGlossariesAtRangeOrderByTermOfPhrase(startIndex, count);
                return Ok(glossaries);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError("Error on GetGlossariesAtRange with {Message}", e.Message);
                return Ok(Enumerable.Empty<Glossary>());
            }
        }
        [HttpGet("totalcount")]
        public async Task<ActionResult<int>> GetTotalCountAsync()
        {
            return await _glossaryRepository.GetTotalGlossaryAsync();
        }
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Glossary>>> SearchGlossaries([FromQuery] string value)
        {
            try
            {
                IEnumerable<Glossary> glossaries = await _glossaryRepository.GetGlossaryByStringSearch(value);
                return Ok(glossaries);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError("Error on SearchGlossaries with {Message}", e.Message);
                return Ok(Enumerable.Empty<Glossary>());
            }
        }
        [HttpGet("search/index/")]
        public async Task<ActionResult<IEnumerable<Glossary>>> SearchGlossaries([FromQuery] string value, [FromQuery] int index, [FromQuery] int count)
        {
            try
            {
                IEnumerable<Glossary> glossaries = await _glossaryRepository.GetGlossaryByStringSearch(value);
                return Ok(glossaries.Skip(index).Take(count).OrderBy(p=>p.TermOfPhrase));
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError("Error on SearchGlossaries with {Message}", e.Message);
                return Ok(Enumerable.Empty<Glossary>());
            }
        }
        [HttpGet("search/total")]
        public async Task<ActionResult<IEnumerable<Glossary>>> GetTotalSearchGlossary([FromQuery] string value)
        {
            try
            {
                IEnumerable<Glossary> glossaries = await _glossaryRepository.GetGlossaryByStringSearch(value);
                return Ok(glossaries.Count());
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError("Error on GetTotalSearchGlossary with {Message}", e.Message);
                return Ok(0);
            }
        }


        [Authorize]
        [HttpGet("current-user")]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
            var userName = User.Identity?.Name; 
            var email = User.FindFirst(ClaimTypes.Email)?.Value; 

            return Ok(new { UserId = userId, Username = userName, Email = email });
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<ActionResult> AddGlossaryAsync([FromBody] GlossaryCreateDTO createDTO)
        {
            try
            {
                Glossary glossary = new Glossary()
                {
                    Guid = Guid.NewGuid(),
                    TermOfPhrase = createDTO.TermOfPhrase,
                    GlossaryExplaination = createDTO.Explaination,
                    DateAdded = DateTime.Now,
                    UserCreatedBy = _userManager.GetUserAsync(User).Result
                };
                if (!await _glossaryRepository.AddNewGlossaryAsync(glossary))
                {
                    throw new Exception("Error on AddNewGlossaryAsync");
                }
                return Created();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on AddGlossaryAsync");
                return BadRequest();
            }
        }


        //// PUT: api/Glossaries/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutGlossary(Guid id, Glossary glossary)
        //{
        //    if (id != glossary.Guid)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(glossary).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!GlossaryExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Glossaries
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Glossary>> PostGlossary(Glossary glossary)
        //{
        //    _context.Glossaries.Add(glossary);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetGlossary", new { id = glossary.Guid }, glossary);
        //}

        //// DELETE: api/Glossaries/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteGlossary(Guid id)
        //{
        //    var glossary = await _context.Glossaries.FindAsync(id);
        //    if (glossary == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Glossaries.Remove(glossary);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool GlossaryExists(Guid id)
        //{
        //    return _context.Glossaries.Any(e => e.Guid == id);
        //}
    }
}
