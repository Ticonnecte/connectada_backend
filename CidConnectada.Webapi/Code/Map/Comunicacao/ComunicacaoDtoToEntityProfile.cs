using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Comunicacao;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Services.Intf.Comunicacao;
using CidConnectada.Webapi.Models.Common;
using CidConnectada.Webapi.Models.Comunicacao;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Comunicacao
{
    public class ComunicacaoDtoToEntityProfile : DtoToEntityBaseProfile
    {
        #region Services
        protected IUsuarioService UsuarioService => GetService<IUsuarioService>();
        protected IEnqueteService EnqueteService => GetService<IEnqueteService>();


        #endregion
        public ComunicacaoDtoToEntityProfile(Func<ContextRequest<int, string>> contextFactory
        )
            : base(contextFactory)
        {

            CreateMap<PesquisaDto, Pesquisa>();

            CreateMap<EnqueteDto, Enquete>();

            CreateMap<EnqueteOpcaoDto, EnqueteOpcao>();

            CreateMap<EnqueteRespostaDto, IList<EnqueteResposta>>()
                .ConvertUsing((src, dest) =>
                {
                    IList<EnqueteResposta> result = new List<EnqueteResposta>();
                    var usuario = UsuarioService.Obter(((Usuario)Context.User).Key);
                    var enquete = EnqueteService.Obter(src.enqueteId);
                    foreach (var opcao in src.opcoes)
                    {
                        var resposta = new EnqueteResposta
                        {
                            Usuario = usuario,
                            EnqueteOpcao = enquete.EnqueteOpcaoSet.First(o => o.OpcaoIdx == opcao)
                        };

                        result.Add(resposta);
                    }

                    return result;
                });

            CreateMap<AgendaCulturalDto, AgendaCultural>()
                .IncludeBase<S3FileGenericDto, S3FileGeneric>()
                .ForMember(dest => dest.ImagemUrl, opt => opt.Ignore());
        }

    }
}