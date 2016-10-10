using Analog_Tamigo_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Analog_Tamigo_API.Mappers;
using TamigoServices;
using TamigoServices.Models.Responses;
using WebApi.OutputCache.V2;

namespace Analog_Tamigo_API.Controllers
{
    public class OpeningHoursController : ApiController
    {
        private readonly ITamigoUserClient _client;
        private readonly IMapper<Shift, ShiftDto> _shiftMapper;

        public OpeningHoursController(ITamigoUserClient client, IMapper<Shift, ShiftDto> shiftMapper)
        {
            _client = client;
            _shiftMapper = shiftMapper;
        }

        // GET: api/openinghours
        [CacheOutput(ClientTimeSpan = 1800, ServerTimeSpan = 1800)]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var start = 8; // 8 am
            var end = 16; // 4 pm
            const int interval = 30; // 30 minutes

            var shifts = _shiftMapper.Map(await _client.GetShifts()).Where(shift => shift.Close > DateTime.Today).ToList();

            if (!shifts.Any())
            {
                return Ok(new OpeningHoursDto { StartHour = start, EndHour = end, IntervalMinutes = interval, Shifts = new SortedDictionary<string, List<OpeningHoursShift>>() });
            }

            start = shifts.Min(s => s.Open.Hour);
            end = shifts.Max(s => s.Close.Hour);

            var openingHoursDto = new OpeningHoursDto { StartHour = start, EndHour = end, IntervalMinutes = interval, Shifts = new SortedDictionary<string, List<OpeningHoursShift>>() };

            var startDate = shifts.Min(s => s.Open.DayOfYear);
            var endDate = shifts.Max(s => s.Open.DayOfYear);

            for (var i = startDate; i <= endDate; i++)
            {
                var currentDate = new DateTime(DateTime.Now.Year, 1, 1, start, 0, 0).AddDays(i - 1);
                var currentDateString = $"{currentDate:yyyy-MM-dd}";
                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday) continue;
                while(currentDate.Hour < end)
                {
                    var openingHourShift = new OpeningHoursShift { ShiftStart = currentDate, Employees = new List<string>() };

                    var date = currentDate;
                    foreach (var shift in shifts.Where(x => x.Open <= date && x.Close >= date.AddMinutes(interval)))
                    {
                        openingHourShift.Open = shift.Employees.Any();
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