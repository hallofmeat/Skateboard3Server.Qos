using System.Xml.Serialization;

namespace Skateboard3Server.Qos.Models;

[XmlRoot(ElementName = "qos")]
public class QosResponse
{

    [XmlElement(ElementName = "numprobes")]
    public int NumProbes { get; set; }

    [XmlElement(ElementName = "qosport")]
    public int QosPort { get; set; }

    [XmlElement(ElementName = "probesize")]
    public int ProbeSize { get; set; }

    [XmlElement(ElementName = "qosip")]
    public long QosIp { get; set; } //optional?

    [XmlElement(ElementName = "requestid")]
    public int RequestId { get; set; } 

    [XmlElement(ElementName = "reqsecret")]
    public int RequestSecret { get; set; }
}