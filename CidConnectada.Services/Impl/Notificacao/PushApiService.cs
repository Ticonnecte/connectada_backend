using System;
using CidConnectada.Services.Intf.Notificacao;
using Expo.Server.Client;
using Zenite.Pi.IoC;

namespace CidConnectada.Services.Impl.Notificacao
{
    public class PushApiService : PushApiClient, IPushApiService
    {
        public object GetService(Type serviceType)
        {
            return ApplicationContext.Resolve<PushApiService>();
        }
    }
}