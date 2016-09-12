using Analog_Tamigo_API.Logic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.OutputCache.V2;

namespace Analog_Tamigo_API.Controllers
{
    public class OpenController : ApiController
    {
        private readonly ITamigoClient _client;

        public OpenController(ITamigoClient client)
        {
            _client = client;
        }
        public OpenController()
        {
            _client = new TamigoClient(ConfigurationManager.AppSettings["TamigoUsername"], ConfigurationManager.AppSettings["TamigoPassword"]);
        }

        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IHttpActionResult> GetIsOpen()
        {
            var isOpen = await _client.IsOpen();
            return Ok(new { open =  isOpen});
        }
    }
}