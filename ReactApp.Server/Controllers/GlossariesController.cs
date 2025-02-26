using System;
using System.Collections.Generic;
using System.Data.Common;
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
    [Authorize(Roles = "User")]
    public class GlossariesController(IGlossaryService glossaryService,
        ILogger<GlossariesController> logger,
        UserManager<IdentityUser> userManager) : ControllerBase
    {
        private readonly IGlossaryService _glossaryService = glossaryService;
        private readonly ILogger<GlossariesController> _logger = logger;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        [HttpGet]
        public async Task<ActionResult<GlossaryRecordResultDTO>> GetGlossariesByRange(
            [FromQuery] int startIndex = 0,
            [FromQuery] int count = 80,
            [FromQuery] string search= "")
        {
            try
            {
                return Ok(await _glossaryService.GetGlossariesByRangeAsync(startIndex, count, search));
            }
            catch(ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid parameters: search={Search}, startIndex={StartIndex}, count={Count}", search, startIndex, count);
                return BadRequest(ex.Message);
            }
            catch(DbException ex)
            {
                _logger.LogError(ex, "Database error occurred while searching glossaries with search={Search}, startIndex={StartIndex}, count={Count}", search ?? "", startIndex, count);
                return StatusCode(500, "An internal server error occurred.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while searching glossaries with search={Search}, startIndex={StartIndex}, count={Count}", search??"", startIndex, count);
                return StatusCode(500, "An internal server error occurred.");
            }
        }


        [HttpPost("add")]
        [Authorize]
        public async Task<ActionResult> AddGlossaryAsync([FromBody] GlossaryCreateDTO createDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Invalid argument");
            }
            try
            {
                if(await _glossaryService.AddGlossariesAsync(createDTO, await _userManager.GetUserAsync(User), HttpContext.RequestAborted))
                {
                    return Created();
                }
                else
                {
                    return StatusCode(500, "An internal server error occured.");
                }
            }
            catch (DbException e)
            {
                _logger.LogError(e, "Database error occured while add new glossary");
                return StatusCode(500, "An interal server error occured.");
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Unexpected error occured while add new glossary");
                return StatusCode(500, "An internal server error occured.");
            }
        }
    }
}
