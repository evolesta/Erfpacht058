using AutoMapper;
using Erfpacht058_API.Models;
using Erfpacht058_API.Models.Eigendom;
using Erfpacht058_API.Models.Facturen;
using Erfpacht058_API.Models.OvereenkomstNS;
using Erfpacht058_API.Models.Rapport;

namespace Erfpacht058_API
{
    public class MapperProfile : Profile
    {
        // Automapper Profiel klasse waarbij we aangeven welke Dto bij welk model hoort
        
        public MapperProfile()
        {
            // Dto => Model

            // Eigendom gerelateerde mappings
            CreateMap<EigendomDto, Eigendom>();
            CreateMap<AdresDto, Adres>();
            CreateMap<EigenaarDto, Eigenaar>();
            CreateMap<HerzieningDto, Herziening>();
            CreateMap<BestandDto, Bestand>();
            CreateMap<OvereenkomstDto, Overeenkomst>()
                .ForMember(dest => dest.Financien, opt => opt.MapFrom(src => src.Financien));
            CreateMap<FinancienDto, Financien>();
            CreateMap<GebruikerDto, Gebruiker>();
            CreateMap<Gebruiker, GebruikerDto>();
            CreateMap<SettingsDto, Settings>();
            CreateMap<ExportDto, Export>();
            CreateMap<TemplateDto, Template>()
                .ForMember(dest => dest.RapportData, opt => opt.MapFrom(src => src.RapportData))
                .ForMember(dest => dest.Filters, opt => opt.MapFrom(src => src.Filters));
            CreateMap<TranslateModelDto, TranslateModel>()
                .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations));
            CreateMap<FactuurJobDto, FactuurJob>();
        }
    }
}
