using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReactApp.Server.Contracts.DTOs.Glossaries;
using ReactApp.Server.Contracts.Paginations;
using ReactApp.Server.DTO.Glossary;
using ReactApp.Server.Services.Interface;

namespace ReactApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //[Consumes("application/json")]
    [Produces("application/json")]
    public class GlossariesController(IGlossaryService glossaryService, UserManager<IdentityUser> userManager) : ControllerBase
    {
        private readonly IGlossaryService _glossaryService = glossaryService;
        private readonly UserManager<IdentityUser> _userManager = userManager;
        [HttpGet]
        public async Task<IResult> GetGlossariesAsync([FromQuery] FilterDto filterDto)
        {
            var glossaryDtos = await _glossaryService.GetGlossariesAsync(filterDto, HttpContext.RequestAborted);
            return TypedResults.Ok(glossaryDtos);
        }
        [HttpPost]
        public async Task<IResult> AddGlossaryAsync([FromBody] AddGlossaryDto createDTO)
        {
            var guid = await _glossaryService.AddGlossaryAsync(createDTO, HttpContext.RequestAborted);
            return TypedResults.Ok(guid);
        }
        [HttpDelete]
        public async Task<IResult> DeleteGlossaryAsync([FromBody] Guid id)
        {
            await _glossaryService.DeleteGlossaryAsync(id, HttpContext.RequestAborted);
            return TypedResults.Ok();
        }
        [HttpPatch]
        public async Task<IResult> UpdateGlossaryAsync(Guid id, [FromBody] UpdateGlossaryDto updateDTO)
        {
            if(id != updateDTO.Id )
            {
                return TypedResults.BadRequest();
            }
            var result = await _glossaryService.UpdateGlossaryAsync(updateDTO, HttpContext.RequestAborted);
            return TypedResults.Ok(result);
        }
    }
}
