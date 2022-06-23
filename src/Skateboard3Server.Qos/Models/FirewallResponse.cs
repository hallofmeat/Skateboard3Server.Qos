using System.Collections.Generic;
using System.Xml.Serialization;

namespace Skateboard3Server.Qos.Models;

[XmlRoot(ElementName = "firewall")]
public class FirewallResponse
{

    [XmlArray("ips")]
    [XmlArrayItem("ips")]
    public List<long> Ips { get; set; }

    [XmlElement(ElementName = "numinterfaces")]
    public int NumInterfaces { get; set; }

    [XmlArray("ports")]
    [XmlArrayItem("ports")]
    public List<int> Ports { get; set; }

    [XmlElement(ElementName = "requestid")]
    public int RequestId { get; set; } 

    [XmlElement(ElementName = "reqsecret")]
    public int RequestSecret { get; set; }
}


public class FirewallIp
{
    [XmlElement(ElementName = "ips")]
    public int Ip { get; set; }
}

public class FirewallPort
{
    [XmlElement(ElementName = "ports")]
    public int Port { get; set; }
}