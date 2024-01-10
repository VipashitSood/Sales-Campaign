using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.SalesCampaigns.Domain
{
    /// <summary>
    /// Represents campaign state type enumeration
    /// </summary>
    public enum CampaignStateType
    {
        Pending = 10,
        Scheduled = 20,
        Unscheduled = 30,
        Running = 40,
        Stop = 50     
    }
}
