using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Organograma;
using CidConnectada.Services.Intf.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.Exceptions;
using Zenite.Pi.IoC;
using Zenite.Pi.Services.Impl;
using Zenite.Pi.Web.WebApi;
using Zenite.Wa;
using Zenite.Wa.Models.Zapi.Contact;
using Zenite.Wa.Models.Zapi.Instance;
using Zenite.Wa.Models.Zapi.Message;

namespace CidConnectada.Services.Impl.Messaging
{
    public class ZApiService : BaseService<int, string>, IZApiService
    {
        private int ConnectionLimitDefault => Int32.TryParse(ApplicationContext.AppSettings["Pi:HttpClient:ConnectionLimitDefault"], out int conLimit) ? conLimit : 100;
        private int MaxConnectionLifetimeDefault => Int32.TryParse(ApplicationContext.AppSettings["Pi:HttpClient:MaxConnectionLifetimeDefault"], out int maxConLifeTime) ? maxConLifeTime : 10;
        private int IdleConnectionTimeoutDefault => Int32.TryParse(ApplicationContext.AppSettings["Pi:HttpClient:IdleConnectionTimeoutDefault"], out int idleConTimeout) ? idleConTimeout : 5;
        private int MaxResponseContentBufferSizeDefault => Int32.TryParse(ApplicationContext.AppSettings["Pi:HttpClient:MaxResponseContentBufferSizeDefault"], out int maxRespContentBufferSize) ? maxRespContentBufferSize : 65536;
        // 180 segundos...
        private int TimeoutDefault => Int32.TryParse(ApplicationContext.AppSettings["Pi:HttpClient:TimeoutDefault"], out int timeout) ? timeout : 180;
        
        private readonly Func<Zapi> _zapiFactory;
        protected virtual Zapi Zapi {
            get
            {
                Zapi result = null;
                Usuario usuario = (Usuario)Context.User;
                string instancia = usuario.Prefeitura.ZApiIdInstancia;
                string token = usuario.Prefeitura.ZApiToken;
                string clientToken = usuario.Prefeitura.ZApiClientToken;
                if (!String.IsNullOrEmpty(instancia) && !String.IsNullOrEmpty(token) && !string.IsNullOrEmpty(clientToken))
                {
                    result = _zapiFactory();
                    result.InstanceId = instancia;
                    result.Token = token;
                    result.HttpClient = Context.HttpClient.GetHttpClient(result.BaseUrl, "Client-Token", clientToken);
                    return result;
                }
                else
                {
                    throw new PiInfraException("Não foi possível obter um socket<instância, token>. Por gentileza, contate o suporte.");
                }
            }
        }

        public ZApiService(Func<ContextRequest<int, string>> contextFactory, Func<Zapi> lazyZapi)
            : base(contextFactory)
        {
            _zapiFactory = lazyZapi;
        }
        public async Task<bool> ConnectedAsync()
        {
            return await Zapi.ConnectedAsync();
        }

        public async Task<bool> DisconnectAsync()
        {
            return await Zapi.DisconnectAsync();
        }

        public async Task<ZApiQrCode64ResultDto> GetQrCodeBase64Async()
        {
            ZApiQrCode64ResultDto result = await Zapi.GetQrCodeBase64Async();
            log.Info($"GetQrCodeBase64Async ({result.connected}): {(!String.IsNullOrEmpty(result.value) ? result.value.Length >= 50 ? result.value.Substring(0, 50) : result.value : "")}...");
            return result;
        }

        public async Task<ZApiStatusInstanceDto> GetStatusAsync()
        {
            return await Zapi.GetStatusAsync();
        }

        public async Task<bool> PhoneExistsAsync(string phone)
        {
            return await Zapi.PhoneExistsAsync(phone);
        }

        public async Task<ZApiMsgResultDto> SendMessageAsync(ZApiSendTextDto zap)
        {
            return await Zapi.PostMessageAsync(zap);
        }

        //public async Task<IList<ZApiGetContactResultDto>> GetContactAsync(int? page, int? pageSize)
        //{
        //    return await Zapi.GetContactAsync(page, pageSize);
        //}
    }
}