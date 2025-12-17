using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Saude;
using CidConnectada.Services.Intf.Local;
using CidConnectada.Services.Intf.Organograma;
using CidConnectada.Services.Intf.Saude;
using CidConnectada.Webapi.Models.Common;
using CidConnectada.Webapi.Models.Saude;
using System;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Saude
{
    // (Dto => Entity)

    public class SaudeDtoToEntityMapperProfile : DtoToEntityBaseProfile
    {
        IUnidadeBasicaSaudeService UbsService => GetService<IUnidadeBasicaSaudeService>();

        IPrefeituraService PrefeituraService => GetService<IPrefeituraService>();

        IEnderecoService EnderecoService => GetService<IEnderecoService>();
        public SaudeDtoToEntityMapperProfile(Func<ContextRequest<int, string>> contextFactory)
            : base(contextFactory)
        {
            CreateMap<UbsBaseDto, UnidadeBasicaSaude>()
                .IncludeBase<S3FileGenericDto, S3FileGeneric>();
                //.ForMember(dest => dest.UbsEspecialidadeMedicaSet, opt => opt.MapFrom(src => src.especialidadeMedicaList))
                //.ForMember(dest => dest.UbsServicoSaudeSet, opt => opt.MapFrom(src => src.servicoSaudeList));

            CreateMap<DetailDto, UbsEspecialidadeMedica>()
                .ForMember(dest => dest.UnidadeBasicaSaude, opt => opt.MapFrom((src, dest, member, ctx) => ctx.Items["Ubs"]))
                .ForMember(dest => dest.EspecialidadeMedica, opt => opt.MapFrom(src => UbsService.GetEspecialidadeMedica(src.key)));

            CreateMap<DetailDto, UbsServicoSaude>()
                .ForMember(dest => dest.UnidadeBasicaSaude, opt => opt.MapFrom((src, dest, member, ctx) => ctx.Items["Ubs"]))
                .ForMember(dest => dest.ServicoSaude, opt => opt.MapFrom(src => UbsService.GetServicoSaude(src.key)));

            CreateMap<UbsDto, UnidadeBasicaSaude>()
                .IncludeBase<UbsBaseDto, UnidadeBasicaSaude>()
                .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src.enderecoId.HasValue ? EnderecoService.Obter(src.enderecoId.Value, null) : null))
                .ForMember(dest => dest.Prefeitura, opt => opt.MapFrom(src => PrefeituraService.Obter(Context.TenantKey, null)))
                .ForMember(dest => dest.ImagemUrl, opt => opt.Ignore());

        }

    }
}