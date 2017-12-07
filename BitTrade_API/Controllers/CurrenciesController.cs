using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BitTrade_API.Models;

using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace BitTrade_API.Controllers
{
    [Produces("application/json")]
    [Route("api/Currencies")]
    public class CurrenciesController : Controller
    {
        private HttpClient _client;

        public string DataCurrencies { get; set; }

        public CurrenciesController(){

            _client = new HttpClient();
        }

        [HttpGet]
        [Route("/api/currencies")]
        public IActionResult GetCurrencies()
        {
            var res = _client.GetAsync("https://bittrex.com/api/v1.1/public/getmarkets").Result;

            if (res.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<Currencies>(res.Content.ReadAsStringAsync().Result);

                return Ok(new { success = true, message = "Liste des Cryptomonaies !", result = data.result });
            }
            return BadRequest(new { success = false, message = " Erreur call bittrex for get all Currencies" });
        }
        
    }
}
