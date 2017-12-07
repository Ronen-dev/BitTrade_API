using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Net.Http;
using Newtonsoft.Json;
using BitTrade_API.Models;

namespace BitTrade_API.Controllers
{
    [Produces("application/json")]
    [Route("api/currency")]
    public class MarketCurrencyController : Controller
    {
        private HttpClient _client;

        public MarketCurrencyController() {

            _client = new HttpClient();
        }

        [HttpGet("{name}", Name = "Get")]
        public IActionResult GetCurrencyMarket(string name)
        {
            if (name != null)
            {
                string url = "https://bittrex.com/api/v1.1/public/getmarketsummary?market=" + name.ToLower();

                var res = _client.GetAsync(url).Result;

                if (res.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<MarketCurrency>(res.Content.ReadAsStringAsync().Result);

                    return Ok(new { success = true, message = " Detail de la Currency ", result = data.result });
                }
                return BadRequest(new { success = false, message = " Erreur call bittrex GetCurrencyMarket " });
            }
            return BadRequest(new { success = false, message = " Erreur Params" });
        }
        
    }
}
