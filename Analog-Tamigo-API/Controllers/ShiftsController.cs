using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Analog_Tamigo_API.Mappers;
using Analog_Tamigo_API.Models;
using TamigoServices;
using TamigoServices.Models.Responses;
using WebApi.OutputCache.V2;

namespace Analog_Tamigo_API.Controllers
{
    [RoutePrefix("api/shifts")]
    public class ShiftsController : ApiController
    {
        private readonly ITamigoUserClient _client;
        private readonly IMapper<Shift, ShiftDto> _shiftMapper;

        public ShiftsController(ITamigoUserClient client, IMapper<Shift, ShiftDto> shiftMapper)
        {
            _client = client;
            _shiftMapper = shiftMapper;
        }

        // GET: api/shifts
        [CacheOutput(ClientTimeSpan = 1800, ServerTimeSpan = 1800)]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var shifts = _shiftMapper.Map(await _client.GetShifts()).Where(shift => shift.Close > DateTime.Today);
            return Ok(shifts);
        }

        // GET: api/shifts/today
        [HttpGet, Route("today")]
        public async Task<IHttpActionResult> GetToday()
        {
            var shifts = _shiftMapper.Map(await _client.GetShifts(DateTime.Today));
            return Ok(shifts);
        }

        [HttpGet, Route("day/{date}")]
        public async Task<IHttpActionResult> GetDate(string date)
        {
            DateTime d;
            if (DateTime.TryParse(date, out d))
            {
                var shifts = _shiftMapper.Map(await _client.GetShifts(d));
                return Ok(shifts);
            }
            return BadRequest("Date format should be yyyy-MM-dd");
        }

        [HttpGet, Route("day/{from}/{to}")]
        public async Task<IHttpActionResult> GetDate(string from, string to)
        {
            DateTime dFrom;
            if (DateTime.TryParse(from, out dFrom))
            {
                DateTime dTo;
                if (DateTime.TryParse(to, out dTo))
                {
                    var shifts = _shiftMapper.Map(await _client.GetShifts(dFrom, dTo));
                    return Ok(shifts);
                }
            }
            return BadRequest("Date format should be yyyy-MM-dd");
        }
    }
}