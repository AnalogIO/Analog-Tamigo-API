using Analog_Tamigo_API.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Analog_Tamigo_API.Models
{
    public class OpeningHoursDTO
    {

            public int StartHour { get; set; }
            public int EndHour { get; set; }
            public int IntervalMinutes { get; set; }
            public Dictionary<string, List<OpeningHoursShift>> Shifts { get; set; }
    }
}