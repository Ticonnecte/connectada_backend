using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Comercios;
using CidConnectada.Webapi.Models.Comercios;
using CidConnectada.Webapi.Models.Common;
using CidConnectada.Webapi.Models.Infos;
using CidConnectada.Webapi.Models.Organograma;
using System;
using System.Linq;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Comercios
{
    public class ComercioEntityToDtoProfile : EntityToDtoBaseProfile
    {
        public ComercioEntityToDtoProfile(Func<ContextRequest<int, string>> contextFactory)
            :base(contextFactory)
        {
            CreateMap<Comercio, ComercioDto>()
                .IncludeBase<S3FileGeneric, S3FileGenericDto>()
                .ConstructUsing(src => new ComercioDto(true))
                .ForMember(dest => dest.categorias, opt => opt.MapFrom(src => src.ComercioCategoriaVinculoSet.Select(cc => cc.Categoria)))
                .ForMember(dest => dest.tipoComercioId, opt => opt.MapFrom(src => src.TipoComercio.Key))
                .ForMember(dest => dest.tipoComercioNome, opt => opt.MapFrom(src => src.TipoComercio.Nome))
                .ForMember(dest => dest.enderecoId, opt => opt.MapFrom(src => src.Endereco.Key))
                .ForMember(dest => dest.placeId, opt => opt.MapFrom(src => src.Endereco.GoogleMapsPlaceId))
                .ForMember(dest => dest.numeroWhatsAppMask, opt => opt.MapFrom(src => Convert.ToUInt64(src.NumeroWhatsApp.Trim().Length > 10 ? src.NumeroWhatsApp.Substring(2) : src.NumeroWhatsApp).ToString(@"(00) 0 0000-0000")))
                .ForMember(dest => dest.enderecoCompleto, opt => opt.MapFrom(src => src.Endereco.EnderecoCompleto));

            CreateMap<TipoComercio, TipoComercioDto>()
                .ForMember(dest => dest.categorias, opt =>
                {
                    opt.PreCondition(ctx => IgnoreMethods(ctx, "TipoComercioController", "GetHome"));
                    opt.MapFrom(src => src.CategoriaTipoComercioSet);
                });

            CreateMap<CategoriaTipoComercio, CategoriaTipoComercioDto>()
                .ForMember(dest => dest.tipoId, opt => opt.MapFrom(src => src.TipoComercio.Key));
            
            CreateMap<TipoComercio, OrdemHomeDto<int>>();

            CreateMap<Produto, ProdutoDto>()
                .ConstructUsing(src => new ProdutoDto(true))
                .IncludeBase<S3FileGeneric, S3FileGenericDto>()
                .ForMember(dest => dest.comercioId, opt => opt.MapFrom(src => src.Comercio.Key));
        }
    }
}
