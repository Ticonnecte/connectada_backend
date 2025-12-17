using CidConnectada.Entities.Model.Emprego;
using CidConnectada.Webapi.Models.Emprego;
using System;
using System.Linq;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Emprego
{
    // (Entity => Dto)
    public class EmpregoEntityToDtoMapperProfile : EntityToDtoBaseProfile
    {
        public EmpregoEntityToDtoMapperProfile(Func<ContextRequest<int, string>> contextFactory)
            : base(contextFactory)
        {

            #region OfertaVaga

            CreateMap<OfertaVaga, OfertaVagaBaseDto>()
                .ForMember(dest => dest.funcao, opt => opt.MapFrom(src => src.Funcao.Nome))
                .ForMember(dest => dest.faixaSalarialId, opt => opt.MapFrom(src => src.FaixaSalarial.Key))
                .ForMember(dest => dest.faixaSalarialValorMin, opt => opt.MapFrom(src => src.FaixaSalarial.ValorMin))
                .ForMember(dest => dest.faixaSalarialValorMax, opt => opt.MapFrom(src => src.FaixaSalarial.ValorMax))
                .ForMember(dest => dest.statusEnumNome, opt => opt.MapFrom(src => src.StatusEnum));

            CreateMap<OfertaVaga, OfertaVagaDto>()
                .IncludeBase<OfertaVaga, OfertaVagaBaseDto>()
                .ForMember(dest => dest.setorMercado, opt => opt.MapFrom(src => src.SetorMercado.Nome))
                .ForMember(dest => dest.enderecoId, opt => opt.MapFrom(src => src.Endereco.Key))
                .ForMember(dest => dest.enderecoCompleto, opt => opt.MapFrom(src => src.Endereco.EnderecoCompleto))
                .ForMember(dest => dest.competenciaList, opt => opt.MapFrom(src => src.OfertaVagaCompetenciaSet.Select(oc => oc.Competencia.Nome)))
                .ForMember(dest => dest.habilidadeList, opt => opt.MapFrom(src => src.OfertaVagaHabilidadeSet.Select(oc => oc.Habilidade.Nome)));

            #endregion

            #region CV

            CreateMap<CurriculumVitae, CVDto>()
                .ForMember(dest => dest.cidadaoId, opt => opt.MapFrom(src => src.Cidadao.Key))
                .ForMember(dest => dest.cidadaoNome, opt => opt.MapFrom(src => src.Cidadao.NomeCompleto))
                .ForMember(dest => dest.funcao, opt => opt.MapFrom(src => src.Funcao.Nome))
                .ForMember(dest => dest.setorMercado, opt => opt.MapFrom(src => src.SetorMercado.Nome))
                .ForMember(dest => dest.experienciaList, opt => opt.MapFrom(src => src.CVExperienciaSet))
                .ForMember(dest => dest.competenciaList, opt => opt.MapFrom(src => src.CVCompetenciaSet.Select(oc => oc.Competencia.Nome)))
                .ForMember(dest => dest.habilidadeList, opt => opt.MapFrom(src => src.CVHabilidadeSet.Select(oc => oc.Habilidade.Nome)));

            CreateMap<CVExperiencia, CVExperienciaDto>()
                .ForMember(dest => dest.funcao, opt => opt.MapFrom(src => src.Funcao.Nome));

            CreateMap<CurriculumVitae, CVViewDto>()
                .ForMember(dest => dest.nome, opt => opt.MapFrom(src => src.Cidadao.NomeCompleto))
                .ForMember(dest => dest.funcao, opt => opt.MapFrom(src => src.Funcao.Nome))
                .ForMember(dest => dest.setorMercado, opt => opt.MapFrom(src => src.SetorMercado.Nome))
                .ForMember(dest => dest.experiencia, opt => opt.MapFrom((src, dest, member, ctx) =>
                {
                    int diasTotal = src.CVExperienciaSet.Select(cve => (cve.PeriodoFinal ?? DateTime.Now).Subtract(cve.PeriodoInicio).Days).Sum(e => e);
                    string anos = $"{diasTotal / 360} anos";
                    string meses = $"{Math.Floor((float)diasTotal / 360 % 1 * 12)} meses";
                    return $"{anos} {meses}";
                }));

            #endregion

        }
    }
}
