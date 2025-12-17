using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Comunicacao;
using CidConnectada.Services.Intf.Comunicacao;
using CidConnectada.Webapi.Models.Common;
using CidConnectada.Webapi.Models.Comunicacao;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Comunicacao
{
    public class ComunicacaoEntityToDtoProfile : EntityToDtoBaseProfile
    {
        #region Services

        protected IEnqueteService EnqueteService => GetService<IEnqueteService>();
        #endregion



        public ComunicacaoEntityToDtoProfile(
            Func<ContextRequest<int, string>> contextFactory
        )
            : base(contextFactory)
        {
            CreateMap<Pesquisa, PesquisaDto>();

            CreateMap<Enquete, EnqueteDto>()
                .ForMember(dest => dest.enqueteOpcaoList, opt => opt.MapFrom(src => src.EnqueteOpcaoSet))
                .ForMember(dest => dest.enqueteResposta, opt => opt.MapFrom(src =>
                    EnqueteService.GetRespostasDoUsuario(src.Key, ((Usuario)Context.User).Key)));

            CreateMap<EnqueteOpcao, EnqueteOpcaoDto>();

            CreateMap<IList<EnqueteResposta>, EnqueteRespostaDto>()
                .ConvertUsing((src, dest) =>
                {
                    if (src.Any())
                    {
                        dest = new EnqueteRespostaDto
                        {
                            enqueteId = src.First().EnqueteOpcao.EnqueteId,
                            opcoes = src.Select(r => r.EnqueteOpcao.OpcaoIdx).ToList()
                        };
                    }
                    return dest;
                });

            CreateMap<AgendaCultural, AgendaCulturalDto>()
                .IncludeBase<S3FileGeneric, S3FileGenericDto>()
                .ForMember(dest => dest.extensaoImg, opt => opt.Ignore())
                .ForMember(dest => dest.base64Img, opt => opt.Ignore());

            //  EnqueteService = enqueteService;
        }

    }
}