using System.Collections.Generic;
using System.Xml.Serialization;

namespace Skateboard3Server.Qos.Models
{
    [XmlRoot(ElementName = "firewall")]
    public class FirewallResponse
    {

        [XmlElement(ElementName = "ips")]
        public List<int> Ips { get; set; }

        [XmlElement(ElementName = "numinterfaces")]
        public int NumInterfaces { get; set; }

        [XmlElement(ElementName = "ports")]
        public List<int> Ports { get; set; }

        [XmlElement(ElementName = "requestid")]
        public int RequestId { get; set; } 

        [XmlElement(ElementName = "reqsecret")]
        public int RequestSecret { get; set; }
    }


}
