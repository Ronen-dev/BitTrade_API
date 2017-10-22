using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitTrade_API.Models
{
    public class User
    {
        public const int REF_STATUT_ENABLE  = 1;
        public const int REF_STATUT_DISABLE = 2;

        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Apikey { get; set; }
        public int StatutiId { get; set; }
    }
}
