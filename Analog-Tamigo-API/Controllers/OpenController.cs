using System.Threading.Tasks;
using System.Web.Http;
using TamigoServices;
using WebApi.OutputCache.V2;

namespace Analog_Tamigo_API.Controllers
{
    public class OpenController : ApiController
    {
        private readonly ITamigoUserClient _client;

        public OpenController(ITamigoUserClient client)
        {
            _client = client;
        }

        [CacheOutput(ClientTimeSpan = 60)]
        [HttpGet]
        public async Task<IHttpActionResult> GetIsOpen()
        {
            var isOpen = await _client.IsOpen();
            return Ok(new { open =  isOpen});
        }
    }
}