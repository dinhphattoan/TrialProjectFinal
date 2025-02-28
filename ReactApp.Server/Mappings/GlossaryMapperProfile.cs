using ReactApp.Server.Contracts.DTOs.Glossaries;
using ReactApp.Server.DTO.Glossary;
using ReactApp.Server.Entity;
using ReactApp.Server.Mappings;

namespace ReactApp.Server.Contracts.Mappings
{
    public class GlossaryMapperProfile : ApplicationProfile
    {
        public GlossaryMapperProfile()
        {
            CreateMap<GlossaryDto, Glossary>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TermOfPhrase, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.GlossaryExplaination, opt => opt.MapFrom(src => src.Definition))
                //.ForMember(dest => dest.UserCreatedBy, opt => opt.MapFrom(src => src.CreateBy))
                .ForMember(dest => dest.DateAdded, opt => opt.MapFrom(src => src.CreateDate))
                .ReverseMap();
            CreateMap<AddGlossaryDto, Glossary>()
                .ForMember(dest => dest.TermOfPhrase, opt => opt.MapFrom(src => src.TermOfPhrase))
                .ForMember(dest => dest.GlossaryExplaination, opt => opt.MapFrom(src => src.Explaination))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.DateAdded, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
