using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Infos;
using CidConnectada.Services.Intf.Infos;
using CidConnectada.Webapi.Models.Common;
using CidConnectada.Webapi.Models.Infos;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Infos
{
    // (Entity => Dto)
    public class InfoEntityToDtoMapperProfile : EntityToDtoBaseProfile
    {
        #region Services
        protected IInfoService InfoService => GetService<IInfoService>();

        #endregion

        public InfoEntityToDtoMapperProfile(
            Func<ContextRequest<int, string>> contextFactory
        )
            : base(contextFactory)
        {
            CreateMap<Categoria, CategoriaDto>()
                .ForMember(dest => dest.corNome, opt => opt.MapFrom(src => src.Cor));

            CreateMap<Info, InfoDto>()
                .ConstructUsing(src => new InfoDto(true))
                .IncludeBase<HtmlContent, HtmlContentDto>()
                .ForMember(dest => dest.conteudo, opt => opt.PreCondition(ctx =>
                {
                    return ctx.Items.Values.Contains("InfoControllerProxy.GetOne");
                }))
                .ForMember(dest => dest.categoriaNome, opt => opt.MapFrom(src => src.Categoria.Nome))
                .ForMember(dest => dest.categoriaId, opt => opt.MapFrom(src => src.Categoria.Key));

            CreateMap<Categoria, InfoViewDto>()
                .AfterMap((src, dest, ctx) => { dest.infoList = ctx.Mapper.Map<IList<InfoDto>>(InfoService.GetAtivasByCategoria(dest.key)); })
                .IncludeBase<Categoria, CategoriaDto>();
            //.ForMember(dest => dest.infoList, opt => opt.MapFrom(src => src.InfoSet));

        }
    }
}