using AutoMapper;
using YemekTarifleri.DTOs;
using YemekTarifleri.Models;

namespace YemekTarifleri.Mappings
{
    /// <summary>
    /// AutoMapper konfigürasyon profili - Entity ve DTO arasındaki mapping kuralları
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// MappingProfile constructor - mapping kurallarını tanımlar
        /// </summary>
        public MappingProfile()
        {
            // TarifModel -> RecipeDto
            CreateMap<TarifModel, RecipeDto>();

            // RecipeCreateDto -> TarifModel
            CreateMap<RecipeCreateDto, TarifModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // RecipeUpdateDto -> TarifModel
            CreateMap<RecipeUpdateDto, TarifModel>();
        }
    }
}

