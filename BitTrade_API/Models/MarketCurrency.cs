using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BitTrade_API.Models
{
    public class MarketCurrency
    {
        [NotMapped]
        public bool success { get; set; }
        [NotMapped]
        public string message { get; set; }
        [NotMapped]
        public List<CurrencyDetail> result { get; set; }
                
    }

    public class CurrencyDetail
    {
        [NotMapped]
        public string MarketName { get; set; }
        [NotMapped]
        public double High { get; set; }
        [NotMapped]
        public double Low { get; set; }
        [NotMapped]
        public double Volume { get; set; }
        [NotMapped]
        public double Last { get; set; }
        [NotMapped]
        public double BaseVolume { get; set; }
        [NotMapped]
        public DateTime TimeStamp { get; set; }
        [NotMapped]
        public double Bid { get; set; }
        [NotMapped]
        public double Ask { get; set; }
        [NotMapped]
        public int OpenBuyOrders { get; set; }
        [NotMapped]
        public int OpenSellOrders { get; set; }
        [NotMapped]
        public double PrevDay { get; set; }
        [NotMapped]
        public DateTime Created { get; set; }
    }

}



