using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Emprego;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Services.Intf.Emprego;
using CidConnectada.Services.Intf.Local;
using CidConnectada.Webapi.Models.Emprego;
using System;
using System.Linq;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Emprego
{
    // (Dto => Entity)
    public class EmpregoDtoToEntityMapperProfile : DtoToEntityBaseProfile
    {
        #region Services

        protected IOfertaVagaService OfertaVagaService => GetService<IOfertaVagaService>();
        protected IUsuarioService UsuarioService => GetService<IUsuarioService>();
        protected IEnderecoService EnderecoService => GetService<IEnderecoService>();

        #endregion

        public EmpregoDtoToEntityMapperProfile(Func<ContextRequest<int, string>> contextFactory
        )
            : base(contextFactory)
        {
            #region OfertaVaga

            CreateMap<OfertaVagaBaseDto, OfertaVaga>()
                .ForMember(dest => dest.FaixaSalarial, opt => opt.MapFrom(src => OfertaVagaService.GetFaixaSalarial(src.faixaSalarialId)))
                .ForMember(dest => dest.DhCriacao, opt =>
                {
                    opt.PreCondition(src => src.isNew);
                    opt.MapFrom(src => DateTime.Now);
                })
                .ForMember(dest => dest.StatusEnum, opt =>
                {
                    opt.PreCondition(src => src.isNew);
                    opt.MapFrom(src => OfertaVagaStatusEnum.Aberta);
                })
                .ForMember(dest => dest.Empregador, opt =>
                {
                    opt.PreCondition(src => src.isNew);
                    opt.MapFrom(src => UsuarioService.Obter(((Usuario)Context.User).Key, null));
                })
                .ForMember(dest => dest.Funcao, opt => opt.MapFrom(src => OfertaVagaService.GetTDetailByName<Funcao>(src.funcao)));

            CreateMap<OfertaVagaDto, OfertaVaga>()
                .IncludeBase<OfertaVagaBaseDto, OfertaVaga>()
                .ForMember(dest => dest.SetorMercado, opt => opt.MapFrom(src => OfertaVagaService.GetTDetailByName<SetorMercado>(src.setorMercado)))
                .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => EnderecoService.Obter(src.enderecoId, null)))
                .ForMember(dest => dest.OfertaVagaCompetenciaSet, opt => opt.MapFrom((src, dest, member, ctx) =>
                {
                    if (!ctx.Items.TryGetValue("OfertaVagaKey", out object item) && !src.isNew)
                        ctx.Items["OfertaVagaKey"] = dest.Key;
                    return src.competenciaList.Select(competenciaNome => OfertaVagaService.GetTDetailByName<Competencia>(competenciaNome))
                        .Where(competencia => competencia != null).ToList();
                }))
                .ForMember(dest => dest.OfertaVagaHabilidadeSet, opt => opt.MapFrom((src, dest, member, ctx) =>
                {
                    if (!ctx.Items.TryGetValue("OfertaVagaKey", out object item) && !src.isNew)
                        ctx.Items["OfertaVagaKey"] = dest.Key;
                    return src.habilidadeList.Select(habilidadeNome => OfertaVagaService.GetTDetailByName<Habilidade>(habilidadeNome))
                        .Where(competencia => competencia != null).ToList();
                }));

            CreateMap<Competencia, OfertaVagaCompetencia>()
                .ForMember(dest => dest.CompetenciaId, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.OfertaVagaId, opt => opt.MapFrom((src, dest, member, ctx) =>
                {
                    long result = 0;
                    if (ctx.Items.TryGetValue("OfertaVagaKey", out object ofertaVagaKey))
                        result = (long)ofertaVagaKey;

                    return result;
                }));

            CreateMap<Habilidade, OfertaVagaHabilidade>()
                .ForMember(dest => dest.HabilidadeId, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.OfertaVagaId, opt => opt.MapFrom((src, dest, member, ctx) =>
                {
                    long result = 0;
                    if (ctx.Items.TryGetValue("OfertaVagaKey", out object ofertaVagaKey))
                        result = (long)ofertaVagaKey;

                    return result;
                }));

            #endregion

            #region CV

            CreateMap<CVDto, CurriculumVitae>()
                .ForMember(dest => dest.SetorMercado, opt => opt.MapFrom(src => OfertaVagaService.GetTDetailByName<SetorMercado>(src.setorMercado)))
                .ForMember(dest => dest.Funcao, opt => opt.MapFrom(src => OfertaVagaService.GetTDetailByName<Funcao>(src.funcao)))
                .ForMember(dest => dest.Cidadao, opt =>
                {
                    opt.PreCondition(src => src.isNew);
                    opt.MapFrom(src => UsuarioService.Obter(((Usuario)Context.User).Key, null));
                })
                .ForMember(dest => dest.CVExperienciaSet, opt => opt.MapFrom((src, dest, member, ctx) =>
                {
                    if (!ctx.Items.TryGetValue("CVId", out object item) && !src.isNew)
                        ctx.Items["CVId"] = dest.Key;
                    return src.experienciaList;
                }))
                .ForMember(dest => dest.CVCompetenciaSet, opt => opt.MapFrom((src, dest, member, ctx) =>
                {
                    if (!ctx.Items.TryGetValue("CVId", out object item) && !src.isNew)
                        ctx.Items["CVId"] = dest.Key;
                    return src.competenciaList.Select(competenciaNome => OfertaVagaService.GetTDetailByName<Competencia>(competenciaNome))
                        .Where(competencia => competencia != null).ToList();
                }))
                .ForMember(dest => dest.CVHabilidadeSet, opt => opt.MapFrom((src, dest, member, ctx) =>
                {
                    if (!ctx.Items.TryGetValue("CVId", out object item) && !src.isNew)
                        ctx.Items["CVId"] = dest.Key;
                    return src.habilidadeList.Select(habilidadeNome => OfertaVagaService.GetTDetailByName<Habilidade>(habilidadeNome))
                        .Where(competencia => competencia != null).ToList();
                }));

            //TODO, ou não ne: Mapeamento esta recriando entidades 'detail', ao invés de atualizar existentes
            CreateMap<CVExperienciaDto, CVExperiencia>()
                .ForMember(dest => dest.Funcao, opt => opt.MapFrom(src => OfertaVagaService.GetTDetailByName<Funcao>(src.funcao)))
                .ForMember(dest => dest.CVId, opt => opt.MapFrom((src, dest, member, ctx) =>
                {
                    long result = 0;
                    if (ctx.Items.TryGetValue("CVId", out object cvId))
                        result = (int)cvId;

                    return result;
                }));

            CreateMap<Competencia, CVCompetencia>()
                .ForMember(dest => dest.CompetenciaId, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.CVId, opt => opt.MapFrom((src, dest, member, ctx) =>
                {
                    long result = 0;
                    if (ctx.Items.TryGetValue("CVId", out object cvId))
                        result = (int)cvId;

                    return result;
                }));

            CreateMap<Habilidade, CVHabilidade>()
                .ForMember(dest => dest.HabilidadeId, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.CVId, opt => opt.MapFrom((src, dest, member, ctx) =>
                {
                    long result = 0;
                    if (ctx.Items.TryGetValue("CVId", out object cvId))
                        result = (int)cvId;

                    return result;
                }));

            #endregion

        }

    }
}