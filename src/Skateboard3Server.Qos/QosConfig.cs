using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618

namespace Skateboard3Server.Qos;

public class QosConfig
{
    [Required]
    public string QosIp { get; set; }
    [Required]
    public string FirewallPrimaryIp { get; set; }
    [Required]
    public string FirewallSecondaryIp { get; set; }
}