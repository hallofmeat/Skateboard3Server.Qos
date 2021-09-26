using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Skateboard3Server.Qos.Controllers
{
    /// <summary>
    /// Not a real controller used by skate, used by admin or other services
    /// </summary>
    [Route("/debug")]
    [ApiController]
    public class DebugController : ControllerBase
    {
        [HttpGet("status")]
        public StatusUpdate Status()
        {
            var current = System.Diagnostics.Process.GetCurrentProcess();
            return new StatusUpdate
            {
                StartTime = current.StartTime
            };
        }
    }

    public class StatusUpdate //Keep in sync with Skateboard3Server
    {
        //TODO: maybe connected users?
        public DateTime StartTime { get; set; }
    }
}
