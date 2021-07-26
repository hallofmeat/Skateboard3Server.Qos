using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Skateboard3Server.Qos.Models;

namespace Skateboard3Server.Qos.Controllers
{
    [ApiController]
    [Route("qos")]
    public class QosController : ControllerBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [HttpGet("qos")]
        [Produces("application/xml")]
        public QosResponse Qos([FromQuery(Name = "vers")] int version,
            [FromQuery(Name = "qtyp")] QosType qosType,
            [FromQuery(Name = "prpt")] int port)
        {
            Logger.Info($"Qos Request, Type:{qosType} Port:{port}");
            //TODO: figure out what to do with prpt
            switch (qosType)
            {
                case QosType.PingTest:
                    return new QosResponse
                    {
                        NumProbes = 0,
                        ProbeSize = 0,
                        QosPort = 17499, //TODO: maybe dont hardcode?
                        QosIp = 16777343, //TODO: pull from config
                        RequestId = 1,
                        RequestSecret = 0
                    };
                case QosType.BandwidthTest:
                    return new QosResponse
                    {
                        NumProbes = 10,
                        ProbeSize = 1200,
                        QosPort = 17499, //TODO: maybe dont hardcode?
                        QosIp = 16777343, //TODO: pull from config
                        RequestId = 1234, //TODO: generate and store?
                        RequestSecret = 5678 //TODO: generate and store?
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(qosType), qosType, null);
            }
        }

        [HttpGet("firewall")]
        [Produces("application/xml")]
        public FirewallResponse Firewall([FromQuery(Name = "vers")] int version,
            [FromQuery(Name = "nint")] int numInterfaces)
        {
            Logger.Info($"Firewall Request, NumInterfaces:{numInterfaces}");
            //TODO pull from config
            return new FirewallResponse
            {
                Ips = new List<int> { 16777343, 16777343 }, //TODO: pull from config
                NumInterfaces = 2, //TODO: we should return 2 because we need 
                Ports = new List<int> { 17500, 17501 }, //TODO do these need to be different?
                RequestId = 1234, //TODO: generate and store?
                RequestSecret = 5678 //TODO: generate and store?
            };
        }

        [HttpGet("firetype")]
        [Produces("application/xml")]
        public FireTypeResponse FireType([FromQuery(Name = "vers")] int version,
            [FromQuery(Name = "rqid")] int requestId,
            [FromQuery(Name = "rqsc")] int requestSecret,
            [FromQuery(Name = "inip")] int inIp,
            [FromQuery(Name = "inpt")] int inPort)
        {
            //Dont parse inIp because it is garbage from rpcs3
            Logger.Info($"FireType Request, RequestId:{requestId} RequestSecret:{requestSecret} InIp:{inIp} InPort:{inPort}");
            //TODO should be blocking (dont return til udp things are sent)
            return new FireTypeResponse
            {
                //FirewallType = (int)FirewallType.PortRestrictedCone //TODO detect all types
                FirewallType = 2 //Figure out what the values should be
            };
        }
    }
}