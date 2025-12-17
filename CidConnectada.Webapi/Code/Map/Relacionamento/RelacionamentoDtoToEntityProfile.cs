using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Relacionamento;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Services.Intf.Local;
using CidConnectada.Services.Intf.Organograma;
using CidConnectada.Services.Intf.Relacionamento;
using CidConnectada.Webapi.Models.Common;
using CidConnectada.Webapi.Models.Relacionamento;
using System;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Relacionamento
{
    // (Dto => Entity)
    public class RelacionamentoDtoToEntityProfile : DtoToEntityBaseProfile
    {
        #region Services
        protected IDialogoService DialogoService => GetService<IDialogoService>();
        protected IPrefeituraService PrefeituraService => GetService<IPrefeituraService>();
        protected ICidadaoService CidadaoService => GetService<ICidadaoService>();
        protected IFuncionarioService FuncionarioService => GetService<IFuncionarioService>();
        protected ISecretariaService SecretariaService => GetService<ISecretariaService>();
        protected IEnderecoService EnderecoService => GetService<IEnderecoService>();


        #endregion
        public RelacionamentoDtoToEntityProfile(Func<ContextRequest<int, string>> contextFactory

        )
            : base(contextFactory)
        {
            CreateMap<DialogoBaseDto, Dialogo>()
                .IncludeBase<S3FileGenericDto, S3FileGeneric>()
                .ForMember(dest => dest.DhCriacao, opt =>
                {
                    opt.PreCondition(src => src.isNew);
                    opt.MapFrom(src => DateTime.Now);
                })
                .ForMember(dest => dest.Cidadao, opt =>
                {
                    opt.PreCondition(src => !src.isAnonymous);
                    opt.MapFrom(src => CidadaoService.Obter(((Usuario)Context.User).Key, null));
                })
                .ForMember(dest => dest.Secretaria, opt => opt.MapFrom(src => SecretariaService.Obter(src.secretariaId, null)));

            CreateMap<DialogoDto, Dialogo>()
                .IncludeBase<DialogoBaseDto, Dialogo>()
                .ForMember(dest => dest.DialogoStatusEnum, opt =>
                {
                    opt.PreCondition(src => src.isNew);
                    opt.MapFrom(src => DialogoStatusEnum.Novo);
                })
                .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => EnderecoService.Obter(src.enderecoId, null)));

            CreateMap<HistoricoDialogoDto, HistoricoDialogo>()
                .ForMember(dest => dest.DhTransicao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Funcionario, opt => opt.MapFrom(src => FuncionarioService.Obter(((Usuario)Context.User).Key, null)))
                .ForMember(dest => dest.Dialogo, opt => opt.MapFrom(src => DialogoService.Obter(src.dialogoId, new string[1] { "HistoricoDialogoSet" })));

            CreateMap<DialogoPreConfigDto, DialogoPreConfig>()
                .ForMember(dest => dest.Secretaria, opt => opt.MapFrom(src => SecretariaService.Obter(src.secretariaId, null)));
        }

    }
}