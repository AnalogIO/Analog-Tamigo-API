using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Analog_Tamigo_API.Mappers;
using Analog_Tamigo_API.Models;
using TamigoServices;
using TamigoServices.Models.Responses;
using WebApi.OutputCache.V2;

namespace Analog_Tamigo_API.Controllers
{
    [RoutePrefix("api/volunteers")]
    public class VolunteersController : ApiController
    {
        private readonly ITamigoUserClient _tamigoClient;
        private readonly IMapper<Contact, VolunteerDto> _volunteerMapper;

        public VolunteersController(ITamigoUserClient tamigoClient, IMapper<Contact, VolunteerDto> volunteerMapper)
        {
            _tamigoClient = tamigoClient;
            _volunteerMapper = volunteerMapper;
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public async Task<IEnumerable<VolunteerDto>> Get()
        {
            return _volunteerMapper.Map(await _tamigoClient.GetContacts());
        }
    }
}