using System.Threading.Tasks;
using CidConnectada.Services.Intf.Notificacao;
using Quartz;
using Zenite.Pi.IoC;

namespace CidConnectada.Services.Impl.BackgroundJobs
{
    //TODO: Criar jobs para enviar notificações agendadas
    public class SendNotificationJob : IJob
    {
        private INotificationService NotificationService { get => ApplicationContext.Resolve<INotificationService>(); }

        public async Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.MergedJobDataMap;
            int id = dataMap.GetInt("NotificacaoId");

            await NotificationService.Send(id);
        }
    }
}