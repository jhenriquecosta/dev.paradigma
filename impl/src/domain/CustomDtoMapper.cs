using AutoMapper;
using System;
using System.Linq;

namespace Works.Paradigma
{
    public static class CustomDtoMapper
    {
        // private static readonly IMapper ObjectMapper = IocManager.Instance.Resolve<IMapper>();

        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            // configuration.Advanced.MaxExecutionPlanDepth = 1;
            configuration.AllowNullCollections = true;
            configuration.ForAllMaps((mapType, mapperExpression) =>
            {
                //  mapperExpression.MaxDepth(1);

            });
            //configuration.CreateMap<Contract, ContractDto>()
            //    .ForMember(dst => dst.SelfieOriginal, opt => opt.MapFrom(src => GetContent(src, "original")))
            //    .ForMember(dst => dst.SelfieRecognized, opt => opt.MapFrom(src => GetContent(src, "recognized")))
            //    .ForMember(dst => dst.DateProcessament, opt => opt.MapFrom(src => src.Processament.Date))
            //    .PreserveReferences().ReverseMap();
            //configuration.CreateMap<Attachment, AttachmentDto>().PreserveReferences().ReverseMap();
            //configuration.CreateMap<ContractProcessament, ContractProcessamentDto>().PreserveReferences().ReverseMap();

        }

        //private static byte[] GetContent(Contract arg,string tipo)
        //{
        //    var value = arg.Selfies.FirstOrDefault(f => f.Key.Equals(tipo));
        //    return value.Contents;
            

        //}
        //public byte[] GetContent()



    }



}
#region resolver


#endregion





///
/// exemplo de automapper
#region examples

//.ForMember(m => m.Children, o => o.Ignore()) // To avoid automapping attempt
//.AfterMap((p,o) => { o.Children = ToISet<ChildDto, Child>(p.Children); });

//configuration.CreateMap<MovimentoDto, Movimento>()
//.AfterMap((src, dest) =>
//{
//    foreach (var i in src.Lancamentos)
//    {
//        var lan = i.MapTo<Lancamento>();
//        dest.AddLancamento(lan);
//    }
//});


// configuration.CreateMap<Lancamento, LancamentoDto>().MaxDepth(1);
// configuration.CreateMap<LancamentoDto, Lancamento>().PreserveReferences();


//configuration.CreateMap<LancamentoItemContabil, LancamentoItemContabilDto>()
//    .ForMember(f => f.Lancamento, opt => opt.Ignore())
//    .ForMember(dest => dest.IdLancamento, opt => opt.MapFrom(src => src.Lancamento.Id))
//    .ForMember(dest => dest.Linha, opt => opt.MapFrom(src => src.Lancamento.Linha))
//    .ForMember(dest => dest.Historico, opt => opt.MapFrom(src => src.Lancamento.Historico))
//    .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.Lancamento.Valor)).ReverseMap();


//Mapper.Map<OrderLine, OrderLineDTO>()
//.ForMember(m => m.Order, opt => opt.Ignore());

//Mapper.Map<Order, OrderDTO>()
//.AfterMap((src, dest) => { 
//foreach(var i in dest.OrderLines) 
//i.Order = dest;
//});

//var config = new MapperConfiguration(cfg => {
//cfg.CreateMap<AuthorDTO, AuthorModel>()
//.ForMember(destination => destination.Address,
//map => map.MapFrom(
//source => new Address
//{
//    City = source .City,
//    State = source .State,
//    Country = source.Country
//}));


//AutoMapperSetup.CreateMapToEditableDto<Agency, AgencyProfileDto> ()
//.ForMember (
//dest => dest.StartDate,
//opt => opt.MapFrom ( src => src.AgencyProfile.EffectiveDateRange != null ? src.AgencyProfile.EffectiveDateRange.StartDate : null ) )
//.ForMember (
//dest => dest.EndDate,
//opt => opt.MapFrom ( src => src.AgencyProfile.EffectiveDateRange != null ? src.AgencyProfile.EffectiveDateRange.EndDate : null ) )
//.ForMember ( dest => dest.LegalName, opt => opt.MapFrom ( src => src.AgencyProfile.AgencyName.LegalName ) )
//.ForMember ( dest => dest.DisplayName, opt => opt.MapFrom ( src => src.AgencyProfile.AgencyName.DisplayName ) )
//.ForMember ( dest => dest.DoingBusinessAsName, opt => opt.MapFrom ( src => src.AgencyProfile.AgencyName.DoingBusinessAsName ) )
//.ForMember ( dest => dest.AgencyType, opt => opt.MapFrom ( src => src.AgencyProfile.AgencyType ) )
//.ForMember ( dest => dest.GeographicalRegion, opt => opt.MapFrom ( src => src.AgencyProfile.GeographicalRegion ) )
//.ForMember ( dest => dest.WebsiteUrlName, opt => opt.MapFrom ( src => src.AgencyProfile.WebsiteUrlName ) );


#endregion
