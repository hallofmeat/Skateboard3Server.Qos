using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Skateboard3Server.Qos.Servers;

namespace Skateboard3Server.Qos
{
    public class QosService : BackgroundService
    {
        private readonly QosServer _qosServer;
        private readonly FirewallServer _firewallServer;
        private readonly FirewallServer _firewallServer2;

        public QosService()
        {
            var qosPort = 17499; //TODO: pull from config
            _qosServer = new QosServer(IPAddress.Any, qosPort); //TODO DI?
            var firewallPort = 17500; //TODO: pull from config, also 17501 is also used
            _firewallServer = new FirewallServer(IPAddress.Any, firewallPort); //TODO DI?
            var firewallPort2 = 17501; //TODO: remove
            _firewallServer2 = new FirewallServer(IPAddress.Any, firewallPort2); //TODO DI?
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _qosServer.Start();
            _firewallServer.Start();
            _firewallServer2.Start();
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _qosServer.Stop();
            _firewallServer.Stop();
            _firewallServer2.Stop();
            return Task.CompletedTask;
        }
    }
}
