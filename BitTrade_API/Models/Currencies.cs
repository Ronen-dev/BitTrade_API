using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BitTrade_API.Models
{
    [NotMapped]
    public class Currencies
    {
        [NotMapped]
        public bool success { get; set; }
        [NotMapped]
        public string message { get; set; }
        [NotMapped]
        public List<Currency> result { get; set; }
    }


    [NotMapped]
    public class Currency
    {
        [NotMapped]
        public string MarketCurrency { get; set; }
        [NotMapped]
        public string BaseCurrency { get; set; }
        [NotMapped]
        public string MarketCurrencyLong { get; set; }
        [NotMapped]
        public string BaseCurrencyLong { get; set; }
        [NotMapped]
        public double MinTradeSize { get; set; }
        [NotMapped]
        public string MarketName { get; set; }
        [NotMapped]
        public bool IsActive { get; set; }
        [NotMapped]
        public DateTime Created { get; set; }
        [NotMapped]
        public string Notice { get; set; }
        [NotMapped]
        public bool? IsSponsored { get; set; }
        [NotMapped]
        public string LogoUrl { get; set; }
    }

}
