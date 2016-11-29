using Analog_Tamigo_API.Logic;
using Analog_Tamigo_API.Models;
using Analog_Tamigo_API.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Analog_Tamigo_API.Controllers
{
    public class OpeningHoursController : ApiController
    {
        private readonly ITamigoClient _client;

        public OpeningHoursController(ITamigoClient client)
        {
            _client = client;
        }

        // GET: api/openinghours
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var start = 8; // 8 am
            var end = 16; // 4 pm
            var interval = 30; // 30 minutes

            var shifts = (await _client.GetShifts()).Where(shift => shift.Close > DateTime.Today);

            var openingHoursDto = new OpeningHoursDTO { StartHour = start, EndHour = end, IntervalMinutes = interval, Shifts = new SortedDictionary<string, List<OpeningHoursShift>>() };

            var shiftDtos = shifts as IList<ShiftDTO> ?? shifts.ToList();
            if (shiftDtos.Count() == 0) return Ok(openingHoursDto); // if no shifts are available then return the opening hours dto with an empty dictionary for shifts.

            var startDate = shiftDtos.OrderBy(x => x.Open).FirstOrDefault();
            var endDate = shiftDtos.OrderBy(x => x.Open).LastOrDefault();

            if (start > startDate.Open.Hour) start = startDate.Open.Hour; // if there's a planned shift outside of the default 8-16 block (0-8)
            if (end < endDate.Open.Hour) end = endDate.Open.Hour; // if there's a planned shift outside of the default 8-16 block (16-24)

            for (int i = startDate.Open.DayOfYear; i <= endDate.Open.DayOfYear; i++)
            {
                var currentDate = new DateTime(DateTime.Now.Year, 1, 1, start, 0, 0, DateTimeKind.Local).AddDays(i - 1);
                var currentDateString = String.Format("{0:yyyy-MM-dd}", currentDate);
                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday) continue;
                while(currentDate.Hour < end)
                {
                    var openingHourShift = new OpeningHoursShift { ShiftStart = currentDate, Employees = new List<string>() };

                    foreach (ShiftDTO shift in shiftDtos.Where(x => x.Open <= currentDate && x.Close >= currentDate.AddMinutes(interval)).ToList())
                    {
                        openingHourShift.Open = (shift.Employees.Count() > 0);
                        openingHourShift.Employees = shift.Employees;
                    }
                    if (!openingHoursDto.Shifts.ContainsKey(currentDateString))
                    {
                        openingHoursDto.Shifts.Add(currentDateString, new List<OpeningHoursShift>());
                    }
                    openingHoursDto.Shifts[currentDateString].Add(openingHourShift);
                    currentDate = currentDate.AddMinutes(interval);
                }   
            }

            return Ok(openingHoursDto);
        }

    }
}