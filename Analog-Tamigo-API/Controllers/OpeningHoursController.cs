using Analog_Tamigo_API.Logic;
using Analog_Tamigo_API.Models;
using Analog_Tamigo_API.Models.Responses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.OutputCache.V2;

namespace Analog_Tamigo_API.Controllers
{
    public class OpeningHoursController : ApiController
    {
        private readonly ITamigoClient _client;

        public OpeningHoursController(ITamigoClient client)
        {
            _client = client;
        }
        public OpeningHoursController()
        {
            _client = new TamigoClient(ConfigurationManager.AppSettings["TamigoUsername"], ConfigurationManager.AppSettings["TamigoPassword"]);
        }

        // GET: api/openinghours
        [CacheOutput(ClientTimeSpan = 1800, ServerTimeSpan = 1800)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IHttpActionResult> Get()
        {
            var start = 8; // 8 am
            var end = 16; // 4 pm
            var interval = 30; // 30 minutes

            var shifts = (await _client.GetShifts()).Where(shift => shift.Close > DateTime.Today);

            var openingHoursDto = new OpeningHoursDTO { StartHour = start, EndHour = end, IntervalMinutes = interval, Shifts = new Dictionary<string, List<OpeningHoursShift>>() };

            var startDate = shifts.OrderBy(x => x.Open).FirstOrDefault().Open.DayOfYear;
            var endDate = shifts.OrderBy(x => x.Open).LastOrDefault().Open.DayOfYear;

            for (int i = startDate; i <= endDate; i++)
            {
                var currentDate = new DateTime(DateTime.Now.Year, 1, 1, start, 0, 0).AddDays(i - 1);
                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday) continue;
                while(currentDate.Hour < end)
                {
                    var openingHourShift = new OpeningHoursShift { ShiftStart = currentDate, Employees = new List<string>() };

                    foreach (ShiftDTO shift in shifts.Where(x => x.Open <= currentDate && x.Close >= currentDate.AddMinutes(interval)).ToList())
                    {
                        openingHourShift.Open = (shift.Employees.Count() > 0);
                        openingHourShift.Employees = shift.Employees;
                    }
                    if (!openingHoursDto.Shifts.ContainsKey(currentDate.ToShortDateString()))
                    {
                        openingHoursDto.Shifts.Add(currentDate.ToShortDateString(), new List<OpeningHoursShift>());
                    }
                    openingHoursDto.Shifts[currentDate.ToShortDateString()].Add(openingHourShift);
                    currentDate = currentDate.AddMinutes(interval);
                }   
            }

            return Ok(openingHoursDto);
        }

    }
}