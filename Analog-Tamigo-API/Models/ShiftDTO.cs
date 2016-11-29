using System;
using System.Collections.Generic;

namespace Analog_Tamigo_API.Models.Responses
{
    public class ShiftDTO
    {
        public DateTime Open { get; set; }
        public DateTime Close { get; set; }
        public IEnumerable<string> Employees { get; set; }
    }
}