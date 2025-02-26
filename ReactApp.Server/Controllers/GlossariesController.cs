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
using ReactApp.Server.Services.Interface;

namespace ReactApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="User")]
    public class GlossariesController(IGlossaryService glossaryService,
        ILogger<GlossariesController> logger,
        UserManager<IdentityUser> userManager) : ControllerBase
    {
        private readonly IGlossaryService _glossaryService = glossaryService;
        private readonly ILogger<GlossariesController> _logger = logger;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        [HttpGet("{startIndex}/{count}")]
        public async Task<ActionResult<IEnumerable<Glossary>>> GetGlossariesAtRange(int startIndex, int count)
        {
            try
            {
                IEnumerable<Glossary> glossaries = await _glossaryService.GetGlossariesByRange(startIndex, count);
                int totalCount = await _glossaryService.@int();
                return Ok(new { total = totalCount, data = glossaries });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation on GetGlossariesAtRange: {Message}", ex.Message);
                return StatusCode(500, "An internal server error occurred due to an invalid operation.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on GetGlossariesAtRange: {Message}", ex.Message);
                return StatusCode(500, "An internal server error occurred.");
            }
        }
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Glossary>>> SearchGlossaries([FromQuery] string search, [FromQuery] int startIndex = 0, [FromQuery] int count =80)
        {
            try
            {
                if(count==0)
                {
                    IEnumerable<Glossary> glossaries = await _glossaryService.GetGlossariesBySearchAsync(search);
                    return Ok(glossaries);
                }
                IEnumerable<Glossary> glossaries = await _glossaryRepository.GetGlossaryByStringSearch(value);
                return Ok(glossaries);
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
                Glossary glossary = new Glossary(createDTO.TermOfPhrase,createDTO.Explaination)
                {
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
    }
}
