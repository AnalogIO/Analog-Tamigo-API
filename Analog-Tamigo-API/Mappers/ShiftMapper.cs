using System.Collections.Generic;
using System.Linq;
using Analog_Tamigo_API.Models;
using TamigoServices.Models.Responses;

namespace Analog_Tamigo_API.Mappers
{
    internal class ShiftMapper : IMapper<Shift, ShiftDto>
    {
        public IEnumerable<ShiftDto> Map(IEnumerable<Shift> t)
        {
            return t.GroupBy(shift => shift.StartTime)
                .Select(grouping => new ShiftDto
                {
                    Open = grouping.Key,
                    Close = grouping.First().EndTime,
                    Employees = grouping.Select(shift => shift.EmployeeName.Split(' ').First())
                })
                .OrderBy(shift => shift.Open)
                .Where(shift => shift.Employees.Any(employee => employee != "Vacant"));
        }
    }
}