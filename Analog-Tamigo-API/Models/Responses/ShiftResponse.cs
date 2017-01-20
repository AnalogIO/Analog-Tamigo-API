using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Analog_Tamigo_API.Models.Responses
{
    public class ShiftResponse
    {

        public int Id { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public IEnumerable<EmployeeDTO> Employees { get; set; }

        public IEnumerable<OpeningHourEmployeeDTO> CheckedInEmployees { get; set; }
    }

}