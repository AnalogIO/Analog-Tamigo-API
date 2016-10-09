using Analog_Tamigo_API.Logic;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace Analog_Tamigo_API.Controllers
{
    [RoutePrefix("api/shifts")]
    public class ShiftsController : ApiController
    {
        private readonly ITamigoClient _client;

        public ShiftsController(ITamigoClient client)
        {
            _client = client;
        }

        // GET: api/shifts
        [CacheOutput(ClientTimeSpan = 1800, ServerTimeSpan = 1800)]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var shifts = (await _client.GetShifts()).Where(shift => shift.Close > DateTime.Today);
            return Ok(shifts);
        }

        // GET: api/shifts/today
        [HttpGet, Route("today")]
        public async Task<IHttpActionResult> GetToday()
        {
            var shifts = await _client.GetShifts(DateTime.Today);
            return Ok(shifts);
        }

        [HttpGet, Route("day/{date}")]
        public async Task<IHttpActionResult> GetDate(string date)
        {
            DateTime d;
            if (DateTime.TryParse(date, out d))
            {
                var shifts = await _client.GetShifts(d);
                return Ok(shifts);
            }
            return BadRequest("Date format should be yyyy-MM-dd");
        }
    }
}