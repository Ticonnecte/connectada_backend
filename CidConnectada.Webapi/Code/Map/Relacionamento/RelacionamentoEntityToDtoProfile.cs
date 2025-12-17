using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Relacionamento;
using CidConnectada.Webapi.Models.Common;
using CidConnectada.Webapi.Models.Relacionamento;
using System;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Relacionamento
{
    public class RelacionamentoEntityToDtoProfile : EntityToDtoBaseProfile
    {
        public RelacionamentoEntityToDtoProfile(Func<ContextRequest<int, string>> contextFactory)
            : base(contextFactory)
        {
            CreateMap<Dialogo, DialogoSimpleDto>()
                .IncludeBase<S3FileGeneric, S3FileGenericDto>()
                .ForMember(dest => dest.dialogoStatusEnumNome, opt => opt.MapFrom(src => src.DialogoStatusEnum));

            CreateMap<Dialogo, DialogoBaseDto>()
                .IncludeBase<S3FileGeneric, S3FileGenericDto>()
                .ForMember(dest => dest.isAnonymous, opt => opt.MapFrom(src => src.Cidadao == null))
                .ForMember(dest => dest.secretariaId, opt => opt.MapFrom(src => src.Secretaria.Key));

            CreateMap<Dialogo, DialogoViewDto>()
                .IncludeBase<Dialogo, DialogoBaseDto>()
                .ForMember(dest => dest.assuntoDialogoEnumNome, opt => opt.MapFrom(src => src.AssuntoDialogoEnum))
                .ForMember(dest => dest.dialogoStatusEnumNome, opt => opt.MapFrom(src => src.DialogoStatusEnum))
                .ForMember(dest => dest.secretariaNome, opt => opt.MapFrom(src => src.Secretaria.Nome))
                .ForMember(dest => dest.cidadaoId, opt => opt.MapFrom(src => src.Cidadao.Key))
                .ForMember(dest => dest.cidadaoNome, opt => opt.MapFrom(src => src.Cidadao == null ? "Anônimo" : src.Cidadao.NomeCompleto));

            CreateMap<Dialogo, DialogoHistoricoDto>()
                .ForMember(dest => dest.assuntoDialogoEnumNome, opt => opt.MapFrom(src => src.AssuntoDialogoEnum))
                .ForMember(dest => dest.dialogoStatusEnumNome, opt => opt.MapFrom(src => src.DialogoStatusEnum))
                .ForMember(dest => dest.historicoList, opt => opt.MapFrom(src => src.HistoricoDialogoSet));

            CreateMap<HistoricoDialogo, HistoricoDialogoViewDto>()
                .ForMember(dest => dest.statusEnumNome, opt => opt.MapFrom(src => src.StatusEnum));

            //CreateMap<Dialogo, HistoricoDialogoFullViewDto>()
            //    .ForMember(dest => dest.key, opt => opt.Ignore())
            //    .IncludeBase<Dialogo, DialogoBaseDto>();

            //CreateMap<HistoricoDialogo, DialogoBaseDto>();

            CreateMap<HistoricoDialogo, HistoricoDialogoFullViewDto>()
                //.IncludeBase<HistoricoDialogo, DialogoBaseDto>()
                //.ForMember(dest => dest.key, opt => opt)
                //.ForMember(dest => dest.key, opt => opt.MapFrom(src => new HistoricoDialogoKey() { }))
                //.ForMember(dest => dest.historico, opt => opt.MapFrom(src => src.Descricao))
                //.ForMember(dest => dest.secretariaNome, opt => opt.MapFrom(src => src.Dialogo.Secretaria.Nome))
                .ForMember(dest => dest.statusEnumNome, opt => opt.MapFrom(src => src.StatusEnum.ToString().Replace("_", " ")));
                //.ForSourceMember(src => src.Dialogo, opt => opt.GetType());

            CreateMap<DialogoPreConfig, DialogoPreConfigDto>()
                .ForMember(dest => dest.assuntoDialogoEnumNome, opt => opt.MapFrom(src => src.AssuntoDialogoEnum))
                .ForMember(dest => dest.secretariaId, opt => opt.MapFrom(src => src.Secretaria.Key));
        }
    }
}