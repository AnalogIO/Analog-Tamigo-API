using Analog_Tamigo_API.Logic;
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
    [RoutePrefix("api/shifts")]
    public class ShiftsController : ApiController
    {
        private readonly ITamigoClient _client;

        public ShiftsController(ITamigoClient client)
        {
            _client = client;
        }
        public ShiftsController()
        {
            _client = new TamigoClient(ConfigurationManager.AppSettings["TamigoUsername"], ConfigurationManager.AppSettings["TamigoPassword"]);
        }

        // GET: api/shifts
        [CacheOutput(ClientTimeSpan = 1800, ServerTimeSpan = 1800)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IHttpActionResult> Get()
        {
            var shifts = (await _client.GetShifts()).Where(shift => shift.Close > DateTime.Today);
            return Ok(shifts);
        }

        // GET: api/shifts/today
        [HttpGet, Route("today")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IHttpActionResult> GetToday()
        {
            var shifts = await _client.GetShifts(DateTime.Today);
            return Ok(shifts);
        }

        [HttpGet, Route("day/{date}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
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