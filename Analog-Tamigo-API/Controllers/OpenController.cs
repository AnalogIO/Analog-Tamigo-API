﻿using Analog_Tamigo_API.Logic;
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

        //[CacheOutput(ClientTimeSpan = 60)]
        [HttpCacheControlPolicy(true, 60)]
        [HttpGet]
        public async Task<IHttpActionResult> GetIsOpen()
        {
            var isOpen = await _client.IsOpen();
            return Ok(new { open =  isOpen});
        }
    }
}