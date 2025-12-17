using AutoMapper;
using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Dto;
using CidConnectada.Entities.Model.Noticias;
using CidConnectada.Services.Intf.Noticias;
using CidConnectada.Webapi.Models.Common;
using CidConnectada.Webapi.Models.Noticias;
using System;
using System.Linq;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Noticias
{
    // (Entity => Dto)
    public class NoticiaEntityToDtoMapperProfile : EntityToDtoBaseProfile
    {
        #region Services

        protected INoticiaCategoriaService NoticiaCategoriaService => GetService<INoticiaCategoriaService>();

        #endregion

        public NoticiaEntityToDtoMapperProfile(
            Func<ContextRequest<int, string>> contextFactory
        )
            : base(contextFactory)
        {
            CreateMap<NoticiaCategoria, NoticiaCategoriaDto>()
                .ForMember(dest => dest.corNome, opt => opt.MapFrom(src => src.Cor));

            CreateMap<Noticia, NoticiaBaseDto>()
                .IncludeBase<HtmlContent, HtmlContentDto>()
                //.ForMember(dest => dest.fotoCapa, opt => opt.MapFrom(src => src.FotoCapaUrl))
                .AfterMap<NoticiaToNoticiaBaseDtoAction>();

            CreateMap<Noticia, NoticiaViewDto>()
                .ConstructUsing(src => new NoticiaViewDto(true))
                .IncludeBase<Noticia, NoticiaBaseDto>()
                .ForMember(dest => dest.conteudo, opt => opt.Ignore())
                .ForMember(dest => dest.categorias,
                    opt => opt.MapFrom(src => NoticiaCategoriaService.GetByNoticiaId(src.Key)));

            CreateMap<Noticia, NoticiaDto>()
                .ConstructUsing(src => new NoticiaDto(true))
                .IncludeBase<Noticia, NoticiaViewDto>()
                .ForMember(dest => dest.conteudo, opt => opt.MapFrom(src => src.Conteudo));
        }
    }

    public class NoticiaToNoticiaBaseDtoAction : IMappingAction<Noticia, NoticiaBaseDto>
    {
        public void Process(Noticia src, NoticiaBaseDto dest)
        {
            NoticiaLog firstLog = src.NoticiaLogSet.OrderBy(l => l.DhUpdate).FirstOrDefault();
            NoticiaLog lastLog = src.NoticiaLogSet.OrderByDescending(l => l.DhUpdate).FirstOrDefault();

            dest.dhCriacao = firstLog?.DhUpdate;
            dest.dhUltimoUpdate = lastLog?.DhUpdate;
            dest.autor = firstLog?.Usuario?.NomeCompleto;
            dest.editor = lastLog?.Usuario?.NomeCompleto;
        }
    }
}