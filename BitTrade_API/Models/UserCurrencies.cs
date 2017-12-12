using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BitTrade_API.Models
{
    public class UserCurrencies
    {

        public long Id { get; set; }
        public string MarketName { get; set; }


        public long UserForeignKey { get; set; }
        [ForeignKey("UserForeignKey")]
        public User User { get; set; }
        
    }
}
