using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using ReactApp.Server.Contracts.DTOs;
using ReactApp.Server.Contracts.DTOs.Glossaries;
using ReactApp.Server.Contracts.Exceptions;
using ReactApp.Server.Contracts.Paginations;
using ReactApp.Server.DTO.Glossary;
using ReactApp.Server.Entity;
using ReactApp.Server.Repository.Interface;
using ReactApp.Server.Services.Interface;
using System.Data.Common;
using System.Security.Claims;

namespace ReactApp.Server.Services
{
    public class GlossaryService : BaseApplicationService, IGlossaryService
    {
        private readonly ILogger<GlossaryService> _logger;
        private readonly IGenericRepository<Glossary,Guid> _glossaryRepository;
        private readonly IMapper _mapper;

        public GlossaryService(ILogger<GlossaryService> logger, IGenericRepository<Glossary,Guid> glossaryRepository, IMapper mapper) : base(logger, mapper)
        {
            _logger = logger;
            _glossaryRepository = glossaryRepository;
            _mapper = mapper;
        }
        static async Task<bool> ExistGlossaryLocalAsync(IGenericRepository<Glossary, Guid> glossaryRepository, string termOfPhrase, Guid? id)
        {
            if (id is null)
            {
                return await glossaryRepository.AnyAsync(Glossary => Glossary.TermOfPhrase.Equals(termOfPhrase));
            }
            return await glossaryRepository.AnyAsync(g => g.Id != id && g.TermOfPhrase.Equals(termOfPhrase));
        }

        public async Task<ResultDto<Guid?>> AddGlossaryAsync(AddGlossaryDto createDTO, CancellationToken cancellationToken = default)
        {

            

            ArgumentNullException.ThrowIfNull(createDTO);
            var resultDto = new ResultDto<Guid?>();
            try
            {
                if (await ExistGlossaryLocalAsync(_glossaryRepository, createDTO.TermOfPhrase, null))
                {
                    resultDto.Message.Add("Term Phase name already exist");
                    return resultDto;
                }
                Glossary glossary = Mapper.Map<Glossary>(createDTO);
                await _glossaryRepository.AddAsync(glossary);
                resultDto.Data = glossary.Id;
                return resultDto;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                resultDto.Message.Add(ex.Message);
                return resultDto;
            }
        }

        public async Task DeleteGlossaryAsync(Guid id, CancellationToken requestAborted)
        {
            try
            {
               await _glossaryRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw new PortalException(ex.Message, ex);
            }
        }

        public async Task<PaginatedResultDto<GlossaryDto>> GetGlossariesAsync(FilterDto filterDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var resultDto = new PaginatedResultDto<GlossaryDto>();
                if (filterDto == null)
                {
                    var allGlossariesResult = await _glossaryRepository.GetPaginatedAsync();

                    return new PaginatedResultDto<GlossaryDto>
                    {
                        CurrentPage = allGlossariesResult.PageIndex,
                        PageSize = allGlossariesResult.PageSize,
                        TotalItems = allGlossariesResult.Count,
                        PageItems = _mapper.Map<IEnumerable<GlossaryDto>>(allGlossariesResult.Items)
                    };
                }
                var paginatedFilter = new PaginatedFilter<Glossary>
                {
                    Filter = !string.IsNullOrEmpty(filterDto.Keyword) ? x => x.TermOfPhrase.Contains(filterDto.Keyword) || x.GlossaryExplaination.Contains(filterDto.Keyword) : default,
                    OrderBy = x => x.OrderBy(x => x.TermOfPhrase.ToLower()),
                    PageIndex = filterDto.Page ?? default,
                    PageSize = filterDto.PageSize ?? default,
                };
                var paginatedResult = await _glossaryRepository.GetPaginatedAsync(paginatedFilter);
                if (paginatedResult == null)
                {
                    return resultDto;
                }

                return new PaginatedResultDto<GlossaryDto>
                {
                    CurrentPage = paginatedResult.PageIndex,
                    PageSize = paginatedResult.PageSize,
                    TotalItems = paginatedResult.Count,
                    PageItems = _mapper.Map<IEnumerable<GlossaryDto>>(paginatedResult.Items)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new PortalException(ex.Message, ex);
            }

        }

        public async Task<ResultDto<Guid?>> UpdateGlossaryAsync(UpdateGlossaryDto updateDTO, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDTO);
            var resultDto = new ResultDto<Guid?>();
            try
            {
                if(await ExistGlossaryLocalAsync(_glossaryRepository,updateDTO.Name,updateDTO.Id))
                {
                    resultDto.Message.Add("Term Phase name already exist");
                    return resultDto;
                }
                var glossary = Mapper.Map<Glossary>(updateDTO);
                var updatedId = await _glossaryRepository.UpdateAsync(glossary,cancellationToken);
                if(updatedId == 0)
                {
                    resultDto.Message.Add("Updating the glossary failed");
                    return resultDto;
                }
                resultDto.Data = glossary.Id;
                resultDto.Success = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, exception.Message);
                resultDto.Message.Add(exception.Message);
                return resultDto;
            }
        }
    }
}
