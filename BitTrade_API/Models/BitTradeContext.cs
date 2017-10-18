using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace BitTrade_API.Models
{
    public class BitTradeContext : DbContext
    {
        public BitTradeContext(DbContextOptions<BitTradeContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
    }
}
