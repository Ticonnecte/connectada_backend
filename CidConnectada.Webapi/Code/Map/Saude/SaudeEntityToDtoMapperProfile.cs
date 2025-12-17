using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Dto.Location;
using CidConnectada.Entities.Model.Saude;
using CidConnectada.Services.Intf.Noticias;
using CidConnectada.Webapi.Models.Common;
using CidConnectada.Webapi.Models.Saude;
using System;
using System.Linq;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Saude
{
    // (Entity => Dto)
    public class SaudeEntityToDtoMapperProfile : EntityToDtoBaseProfile
    {
        public SaudeEntityToDtoMapperProfile(Func<ContextRequest<int, string>> contextFactory)
            : base(contextFactory)
        {
            CreateMap<UnidadeBasicaSaude, UbsBaseDto>()
                .IncludeBase<S3FileGeneric, S3FileGenericDto>()
                .ForMember(dest => dest.enderecoCompleto, opt => opt.MapFrom(src => src.Endereco != null ? src.Endereco.EnderecoCompleto : null))
                .ForMember(dest => dest.especialidadeMedicaList, opt => opt.MapFrom(src => src.UbsEspecialidadeMedicaSet))
                .ForMember(dest => dest.servicoSaudeList, opt => opt.MapFrom(src => src.UbsServicoSaudeSet));
                

            CreateMap<UbsEspecialidadeMedica, DetailDto>()
                .ForMember(dest => dest.key, opt => opt.MapFrom(src => src.EspecialidadeMedica.Key))
                .ForMember(dest => dest.nome, opt => opt.MapFrom(src => src.EspecialidadeMedica.Nome))
                .ForMember(dest => dest.descricao, opt => opt.MapFrom(src => src.EspecialidadeMedica.Descricao));

            CreateMap<EspecialidadeMedica, DetailDto>();

            CreateMap<UbsServicoSaude, DetailDto>()
                .ForMember(dest => dest.key, opt => opt.MapFrom(src => src.ServicoSaude.Key))
                .ForMember(dest => dest.nome, opt => opt.MapFrom(src => src.ServicoSaude.Nome))
                .ForMember(dest => dest.descricao, opt => opt.MapFrom(src => src.ServicoSaude.Descricao));

            CreateMap<ServicoSaude, DetailDto>();

            CreateMap<UnidadeBasicaSaude, UbsDto>()
                .IncludeBase<UnidadeBasicaSaude, UbsBaseDto>()
                .ForMember(dest => dest.enderecoId, opt => opt.MapFrom(src => src.Endereco != null ? src.Endereco.Key : int.Parse(null)));

            CreateMap<UnidadeBasicaSaude, UbsViewDto>()
                .IncludeBase<UnidadeBasicaSaude, UbsBaseDto>()
                .ForMember(dest => dest.coordenadas, opt => opt.MapFrom(src => src.Endereco != null ? LocationDto.FromDbGeo(src.Endereco.Coordenadas) : null))
                .ForMember(dest => dest.tipoUnidadeEnumNome, opt => opt.MapFrom(src => src.TipoUnidadeEnum))
                .ForMember(dest => dest.porteEnumNome, opt => opt.MapFrom(src => src.PorteEnum))
                .ForMember(dest => dest.regiaoAbrangenciaEnumNome, opt => opt.MapFrom(src => src.RegiaoAbrangenciaEnum))
                .ForMember(dest => dest.situacaoEnumNome, opt => opt.MapFrom(src => src.SituacaoEnum));
        }

    }
}
