using Analog_Tamigo_API.Logic;
using CacheCow.Server.CacheControlPolicy;
using System.Threading.Tasks;
using System.Web.Http;

namespace Analog_Tamigo_API.Controllers
{
    public class OpenController : ApiController
    {
        private readonly ITamigoClient _client;

        public OpenController(ITamigoClient client)
        {
            _client = client;
        }

        //[HttpCacheControlPolicy(true, 0, true)]
        //[HttpGet]
        //public async Task<IHttpActionResult> GetIsOpen()
        //{
        //    var isOpen = await _client.IsOpen();
        //    return Ok(new { open =  isOpen});
        //}

        // GET: api/open // NEW SHIFTPLANNING FIX
        public IHttpActionResult GetIsOpen()
        {
            return Redirect("https://analogio.dk/publicshiftplanning/api/open/analog");
        }
    }
}