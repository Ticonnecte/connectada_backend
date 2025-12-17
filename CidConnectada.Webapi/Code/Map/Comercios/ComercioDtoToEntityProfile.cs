using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Comercios;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Services.Intf.Comercios;
using CidConnectada.Services.Intf.Local;
using CidConnectada.Services.Intf.Organograma;
using CidConnectada.Webapi.Models.Comercios;
using CidConnectada.Webapi.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenite.Pi.Context;
using Zenite.Wa;

namespace CidConnectada.Webapi.Code.Map.Comercios
{
    public class ComercioDtoToEntityProfile : DtoToEntityBaseProfile
    {

        #region Services
        protected ICidadaoService CidadaoService => GetService<ICidadaoService>();
        protected IEnderecoService EnderecoService => GetService<IEnderecoService>();
        protected ITipoComercioService TipoComercioService => GetService<ITipoComercioService>();
        protected IComercioService ComercioService => GetService<IComercioService>();
        protected IPrefeituraService PrefeituraService => GetService<IPrefeituraService>();


        #endregion

        public ComercioDtoToEntityProfile(Func<ContextRequest<int, string>> contextFactory
        )
            : base(contextFactory)
        {
            CreateMap<ComercioDto, Comercio>()
                .IncludeBase<S3FileGenericDto, S3FileGeneric>()
                .ForMember(dest => dest.OrdemHome, opt => opt.Ignore())
                .ForMember(dest => dest.ComercioCategoriaVinculoSet, opt => opt.MapFrom((src, dest, member, ctx) =>
                {
                    ISet<ComercioCategoriaVinculo> result = new HashSet<ComercioCategoriaVinculo>();
                    if (ctx.Items.TryGetValue("CategoriasFunc", out object funcObj))
                    {
                        IList<int> keys = src.categorias.Select(c => c.key).ToList();
                        result = ((Func<IList<int>, Comercio, ISet<ComercioCategoriaVinculo>>)funcObj)(keys, dest);
                    }
                    return result;
                }))
                .ForMember(dest => dest.NumeroWhatsApp, opt => opt.MapFrom(src => WhatsAppUtil.GetPhoneCleanUp(src.numeroWhatsApp)))
                .ForMember(dest => dest.Cidadao, opt => opt.MapFrom(src => CidadaoService.Obter(((Usuario)Context.User).Key, null)))
                .ForMember(dest => dest.TipoComercio, opt => opt.MapFrom(src => TipoComercioService.Obter(src.tipoComercioId, null)))
                .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => EnderecoService.Obter(src.enderecoId, null)));

            CreateMap<TipoComercioDto, TipoComercio>()
                .ForMember(dest => dest.OrdemHome, opt => opt.Ignore());

            CreateMap<CategoriaTipoComercioDto, CategoriaTipoComercio>();
            //.ForMember(dest => dest.TipoComercio, opt => opt.MapFrom(src => TipoComercioService.Obter(src.tipoId, null)));

            CreateMap<ProdutoDto, Produto>()
                .BeforeMap((src, dest) =>
                {
                    if(src.isNew)
                    {
                        dest.Comercio = ComercioService.Obter(src.comercioId, null);
                    }
                })
                .IncludeBase<S3FileGenericDto, S3FileGeneric>();

        }
    }
}