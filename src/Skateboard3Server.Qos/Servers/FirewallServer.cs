using System.IO;
using System.Net;
using System.Net.Sockets;
using NLog;
using Skateboard3Server.Qos.Util;

namespace Skateboard3Server.Qos.Servers;

public class FirewallServer : UdpServer
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public FirewallServer(IPAddress endpoint, int port) : base(endpoint, port)
    {
        Logger.Info($"UDP Listening on {endpoint}:{port}");
    }

    protected override void OnStarted()
    {
        ReceiveAsync();
    }

    protected override void OnReceived(EndPoint endpoint, byte[] buffer, int offset, int size)
    {
        if (size != 8) //not a firewall request (probably dropbox)
        {
            return;
        }
        var remote = endpoint as IPEndPoint;
        if (remote == null)
        {
            Logger.Error($"Recived UDP message without remote endpoint");
            return;
        }
        Logger.Trace($"Firewall Incoming[{remote.Address}:{remote.Port}]: {BinaryUtil.FormatHex(buffer, offset, size)}");
        using (var reader = new BinaryReader(new MemoryStream(buffer, offset, size)))
        using (var writer = new MemoryStream())
        {
            var requestId = reader.ReadInt32Be();
            writer.Write(BinaryUtil.GetBigEndianBytes(requestId));
            var requestSecret = reader.ReadInt32Be();
            writer.Write(BinaryUtil.GetBigEndianBytes(requestSecret));
            Logger.Debug($"Firewall[{remote.Address}:{remote.Port}]: RequestId:{requestId} RequestSecret:{requestSecret}");

            //TODO: do real firewall detection

            var output = writer.ToArray();
            Logger.Trace($"Firewall Outgoing[{remote.Address}:{remote.Port}]: {BinaryUtil.FormatHex(output, 0, output.Length)}");
            SendAsync(endpoint, output, 0, output.Length);
        }

    }

    protected override void OnSent(EndPoint endpoint, int sent)
    {
        ReceiveAsync();
    }

    protected override void OnError(SocketError error)
    {
        Logger.Error($"Firewall UDP server caught an error with code {error}");
    }
}