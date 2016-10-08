using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Analog_Tamigo_API.Models
{
    public class OpeningHoursShift
    {
        public DateTime ShiftStart { get; set; }
        public bool Open { get; set; }
        public IEnumerable<string> Employees { get; set; }
    }
}