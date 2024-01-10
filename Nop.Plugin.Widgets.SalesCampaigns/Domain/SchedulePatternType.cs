using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.SalesCampaigns.Domain
{
    /// <summary>
    /// Represents schedule pattern type enumeration
    /// </summary>
    public enum SchedulePatternType
    {
        Everyday = 10,
        EveryMonth = 20,
        OnOddDays = 30,
        OnEvenDays = 40,
        OnExactDay = 50,
        OnMondays = 60,
        OnTuesdays = 70,
        OnWednesdays = 80,
        OnThursdays = 90,
        OnFridays = 100,
        OnSaturdays = 110,
        OnSundays = 120,
        FromMondayToFriday = 130,
        OnSaturdaysAndSundays = 140
    }
}
