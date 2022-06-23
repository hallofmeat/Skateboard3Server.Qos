using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using NLog;
using Skateboard3Server.Qos.Util;

namespace Skateboard3Server.Qos.Servers;

public class QosServer : UdpServer
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public QosServer(IPAddress endpoint, int port) : base(endpoint, port)
    {
        Logger.Info($"UDP Listening on {endpoint}:{port}");
    }

    protected override void OnStarted()
    {
        ReceiveAsync();
    }

    protected override void OnReceived(EndPoint endpoint, byte[] buffer, int offset, int size)
    {
        var remote = endpoint as IPEndPoint;
        if (remote == null)
        {
            Logger.Error($"Recived UDP message without remote endpoint");
            return;
        }
        if (remote.AddressFamily == AddressFamily.InterNetworkV6)
        {
            Logger.Error($"Recived UDP message with IPV6 address");
            return;
        }
        Logger.Trace($"Qos Incoming[{remote.Address}:{remote.Port}]: {BinaryUtil.FormatHex(buffer, offset, size)}");
        using (var reader = new BinaryReader(new MemoryStream(buffer, offset, size)))
        using (var writer = new MemoryStream())
        {
            var clientRequestCount = reader.ReadInt32Be();
            writer.Write(BinaryUtil.GetBigEndianBytes(clientRequestCount));
            var requestId = reader.ReadInt32Be();
            writer.Write(BinaryUtil.GetBigEndianBytes(requestId));
            var requestSecret = reader.ReadInt32Be();
            writer.Write(BinaryUtil.GetBigEndianBytes(requestSecret));
            if (requestId == 1 && requestSecret == 0) //ping test
            {
                Logger.Debug($"Qos Ping[{remote.Address}:{remote.Port}]: CRequestCount:{clientRequestCount} RequestId:{requestId} RequestSecret:{requestSecret}");
                var unknown1 = reader.ReadInt32Be(); //TODO figure this out
                writer.Write(BinaryUtil.GetBigEndianBytes(unknown1));
                var unknownCount = reader.ReadInt32Be(); //increments by 10 every request
                writer.Write(BinaryUtil.GetBigEndianBytes(unknownCount));
                //External IP
                writer.Write(BinaryUtil.GetBigEndianIpAddress(remote.Address));
                //External Port?
                writer.Write(BinaryUtil.GetBigEndianBytes((short)remote.Port));
                //Unknown
                writer.Write(BinaryUtil.GetBigEndianBytes(0));
            }
            else //bandwidth test
            {
                Logger.Debug($"Qos Bandwidth[{remote.Address}:{remote.Port}]: CRequestCount:{clientRequestCount} RequestId:{requestId} RequestSecret:{requestSecret}");
                var requestCount = reader.ReadInt32Be();
                writer.Write(BinaryUtil.GetBigEndianBytes(requestCount + 1));
                var totalCount = reader.ReadInt32Be();
                writer.Write(BitConverter.GetBytes(totalCount)); //little endian
                writer.Write(BinaryUtil.GetBigEndianBytes(123456789)); //TODO figure out what this value does
                writer.Write(new byte[] { 0x0e, 0x4b }); //TODO figure out what this value does
                var responseSize = 1200; //hardcoded TODO: from PS3.xml/qos endpoint
                var blank = new byte[responseSize - 26]; //26 is the size of the start of the response
                writer.Write(blank);
            }

            var output = writer.ToArray();
            Logger.Trace($"Qos Outgoing[{remote.Address}:{remote.Port}]: {BinaryUtil.FormatHex(output, 0, output.Length)}");
            SendAsync(endpoint, output, 0, output.Length);
        }

    }

    protected override void OnSent(EndPoint endpoint, int sent)
    {
        ReceiveAsync();
    }

    protected override void OnError(SocketError error)
    {
        Logger.Error($"Qos UDP server caught an error with code {error}");
    }
}