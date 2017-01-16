using Analog_Tamigo_API.Logic;
using CacheCow.Server.CacheControlPolicy;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

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
        [HttpCacheControlPolicy(true, 0, true)]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var shifts = (await _client.GetShifts()).Where(shift => shift.Close > DateTime.Today);
            return Ok(shifts);
        }

        //// GET: api/shifts // NEW SHIFTPLANNING FIX
        //public IHttpActionResult Get()
        //{
        //    return Redirect("https://analogio.dk/publicshiftplanning/api/shifts/analog");
        //}

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