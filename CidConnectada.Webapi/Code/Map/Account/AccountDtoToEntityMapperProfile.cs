using CidConnectada.Entities.Model.Account;
using CidConnectada.Services.Intf.Local;
using CidConnectada.Services.Intf.Organograma;
using CidConnectada.Webapi.Models;
using CidConnectada.Webapi.Models.Account;
using System;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Account
{
    // Dto => Entity
    public class AccountDtoToEntityMapperProfile : DtoToEntityBaseProfile
    {

        #region Services

        protected IPrefeituraService PrefeituraService => GetService<IPrefeituraService>();
        protected IEnderecoService EnderecoService => GetService<IEnderecoService>();


        #endregion

        public AccountDtoToEntityMapperProfile(Func<ContextRequest<int, string>> contextFactory
        )
            : base(contextFactory)
        {

            CreateMap<UsuarioDto, Usuario>()
                .ForMember(dest => dest.TenantKey, opt =>
                {
                    opt.PreCondition(ctx => !ctx.Items.TryGetValue("Caller", out object caller)
                        || (string)caller != "PrefeituraController.Post");
                    opt.MapFrom((src, dest, member, ctx) => src.isNew ? src.tenantId : dest.TenantKey);
                })
                .ForMember(dest => dest.Prefeitura, opt =>
                {
                    opt.PreCondition(ctx => !ctx.Items.TryGetValue("Caller", out object caller)
                        || (string)caller != "PrefeituraController.Post");
                    opt.MapFrom((src, dest, member, ctx) =>
                        src.isNew ? PrefeituraService.Obter(src.tenantId) : dest.Prefeitura);
                })
                .ForMember(dest => dest.AceitaMsgWhastApp, opt => opt.MapFrom(src => src.isNew || src.aceitaMsgWhastApp))
                .ForMember(dest => dest.ConcordaTermosDeUso, opt => opt.MapFrom(src => true));

            CreateMap<CidadaoDto, Cidadao>()
                .IncludeBase<UsuarioDto, Usuario>()
                .ForMember(dest => dest.Bairro, opt =>
                {
                    opt.PreCondition(src => src.bairroId != 0);
                    opt.MapFrom(src => EnderecoService.GetBairro(src.bairroId));
                });

            CreateMap<CidadaoEditDto, Cidadao>()
                .IncludeBase<UsuarioDto, Usuario>()
                .ForMember(dest => dest.Bairro, opt =>
                {
                    opt.PreCondition(src => src.bairroId != 0);
                    opt.MapFrom(src => EnderecoService.GetBairro(src.bairroId));
                });

            CreateMap<FuncionarioDto, Funcionario>()
                .IncludeBase<UsuarioDto, Usuario>();

            CreateMap<FuncionarioEditDto, Funcionario>()
                .IncludeBase<UsuarioDto, Usuario>();

            CreateMap<VerifyAccountModel, Device>()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.deviceId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.deviceName))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.deviceType));
        }

    }
}