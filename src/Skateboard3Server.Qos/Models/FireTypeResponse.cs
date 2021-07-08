using System.Xml.Serialization;

namespace Skateboard3Server.Qos.Models
{
    [XmlRoot(ElementName = "firetype")]
    public class FireTypeResponse
    {

        [XmlElement(ElementName = "firetype")]
        public int FirewallType { get; set; }
    }
}
