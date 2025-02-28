using AutoMapper;
using ReactApp.Server.Contracts.DTOs.Glossaries;

namespace ReactApp.Server.Contracts.Mappings
{
    public class GlossaryMapperProfile : Profile
    {
        public GlossaryMapperProfile()
        {
            //CreateMap<GlossaryDto, Glossary>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.Term, opt => opt.MapFrom(src => src.Term))
            //    .ForMember(dest => dest.Definition, opt => opt.MapFrom(src => src.Definition))
            //    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            //    .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            //    .ReverseMap();
        }
    }
}
