using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Analog_Tamigo_API.Logic;
using Analog_Tamigo_API.Models;

namespace Analog_Tamigo_API.Controllers
{
    [RoutePrefix("api/volunteers")]
    public class VolunteersController : ApiController
    {
        private readonly ITamigoClient _tamigoClient;

        public VolunteersController(ITamigoClient tamigoClient)
        {
            _tamigoClient = tamigoClient;
        }

        [HttpGet]
        //[CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public async Task<IEnumerable<VolunteerDto>> Get()
        {
            return await _tamigoClient.GetEmployees();
        }
    }
}