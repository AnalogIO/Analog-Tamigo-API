using System.Collections.Generic;

namespace Analog_Tamigo_API.Models
{
    public class OpeningHoursDTO
    {

            public int StartHour { get; set; }
            public int EndHour { get; set; }
            public int IntervalMinutes { get; set; }
            public SortedDictionary<string, List<OpeningHoursShift>> Shifts { get; set; }
    }
}