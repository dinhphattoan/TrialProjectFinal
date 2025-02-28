using Microsoft.AspNetCore.Identity;
using ReactApp.Server.Contracts.DTOs;
using ReactApp.Server.Contracts.DTOs.Glossaries;
using ReactApp.Server.Contracts.Paginations;
using ReactApp.Server.DTO.Glossary;
using ReactApp.Server.Entity;

namespace ReactApp.Server.Services.Interface
{
    public interface IGlossaryService
    {
        Task<PaginatedResultDto<GlossaryDto>> GetGlossariesAsync(FilterDto filterDto, CancellationToken cancellationToken = default);
        Task<ResultDto<Guid?>> AddGlossaryAsync(AddGlossaryDto createDTO, CancellationToken cancellationToken = default);
        Task DeleteGlossaryAsync(Guid id, CancellationToken requestAborted);
        Task<ResultDto<Guid?>> UpdateGlossaryAsync(UpdateGlossaryDto updateDTO, CancellationToken cancellationToken = default);
    }
}
