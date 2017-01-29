using Analog_Tamigo_API.Logic;
using CacheCow.Server.CacheControlPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Analog_Tamigo_API.Models.Responses;

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
        /*
        [HttpCacheControlPolicy(true, 0, true)]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var shifts = (await _client.GetShifts()).Where(shift => shift.Close > DateTime.Today);
            return Ok(shifts);
        }
        */

        [HttpGet]
        public async Task<HttpResponseMessage> Get()
        {
            var client = new HttpClient();
            const string url = "https://analogio.dk/publicshiftplanning/api/shifts/analog";
            var responseMsg = await client.GetAsync(url);
            var shiftResponse = responseMsg.Content.ReadAsAsync<List<ShiftResponse>>().Result;
            client.Dispose();

            var shifts = shiftResponse.Select(shift => new ShiftDTO
            {
                Open = shift.Start.ToUniversalTime().ToLocalTime(), // hack to match old tamigo api response
                Close = shift.End.ToUniversalTime().ToLocalTime(), // hack to match old tamigo api response
                Employees = shift.CheckedInEmployees.Select(emp => emp.FirstName)
            });

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, shifts);

            response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                NoStore = true
            };

            return response;
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