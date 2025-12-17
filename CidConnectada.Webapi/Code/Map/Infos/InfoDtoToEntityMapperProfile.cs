using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Infos;
using CidConnectada.Services.Intf.Infos;
using CidConnectada.Webapi.Models.Common;
using CidConnectada.Webapi.Models.Infos;
using System;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Infos
{
    // (Dto => Entity)
    public class InfoDtoToEntityMapperProfile : DtoToEntityBaseProfile
    {
        #region Service 

        protected ICategoriaService CategoriaService => GetService<ICategoriaService>();

        #endregion

        public InfoDtoToEntityMapperProfile(Func<ContextRequest<int, string>> contextFactory
        )
            : base(contextFactory)
        {
            CreateMap<CategoriaDto, Categoria>();

            CreateMap<InfoDto, Info>()
                .IncludeBase<HtmlContentDto, HtmlContent>()
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => CategoriaService.Obter(src.categoriaId, null)));

        }

    }
}
