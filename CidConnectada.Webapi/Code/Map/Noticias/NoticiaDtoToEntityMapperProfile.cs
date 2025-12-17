using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Dto;
using CidConnectada.Entities.Model.Noticias;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Webapi.Models.Common;
using CidConnectada.Webapi.Models.Noticias;
using System;
using System.Linq;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Noticias
{
    // (Dto => Entity)
    public class NoticiaDtoToEntityMapperProfile : DtoToEntityBaseProfile
    {

        #region Services

        protected IUsuarioService UsuarioService => GetService<IUsuarioService>();


        #endregion

        public NoticiaDtoToEntityMapperProfile(Func<ContextRequest<int, string>> contextFactory
        )
            : base(contextFactory)
        {
            CreateMap<NoticiaCategoriaDto, NoticiaCategoria>();

            CreateMap<NoticiaBaseDto, Noticia>()
                .IncludeBase<HtmlContentDto, HtmlContent>();

            CreateMap<NoticiaViewDto, Noticia>()
                .IncludeBase<NoticiaBaseDto, Noticia>()
                .ForMember(dest => dest.NoticiaCategoriaVincSet, opt => opt.MapFrom((src, dest, member, ctx) =>
                {
                    if (!src.isNew) ctx.Items["Noticia"] = dest;
                    return src.categorias;
                }));

            CreateMap<NoticiaDto, HtmlContent>();

            CreateMap<NoticiaDto, Noticia>()
                .IncludeBase<NoticiaViewDto, Noticia>()
                //.ForMember(dest => dest.NoticiaCategoriaVincSet, opt => opt.MapFrom((src, dest, member, ctx) =>
                //{
                //    if (!src.isNew) ctx.Items["Noticia"] = dest;
                //    return src.categorias;
                //}))
                //.ForMember(dest => dest.Conteudo, opt => opt.MapFrom(src => src.conteudo))
                .ForMember(dest => dest.NoticiaLogSet, opt => opt.Ignore())
                .ForMember(dest => dest.EnvioNoticiaSet, opt =>
                {
                    opt.PreCondition(src => src.isNew && src.enviarWhatsApp);
                    opt.MapFrom((src, dest, member, ctx) =>
                    {
                        return UsuarioService.GetWhatsAppEnabled().Select(u => new EnvioNoticia
                        {
                            NoticiaId = src.key,
                            UsuarioId = u.Key
                        });
                    });
                });

            CreateMap<NoticiaCategoriaDto, NoticiaCategoriaVinc>()
                .ForMember(dest => dest.NoticiaId, opt => opt.MapFrom((src, dest, member, ctx) =>
                {
                    string result = null;
                    if (ctx.Items.TryGetValue("Noticia", out object noticia))
                        result = ((Noticia)noticia).Key;

                    return result;
                }))
                .ForMember(dest => dest.CategoriaId, opt => opt.MapFrom(src => src.key));
        }

    }
}
