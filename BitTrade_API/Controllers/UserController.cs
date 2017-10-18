using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BitTrade_API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BitTrade_API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly BitTradeContext _context;

        public UserController(BitTradeContext context) {

            _context = context;

            if (_context.Users.Count() == 0) {

                _context.Users.Add(new User{
                    Firstname = "Patrick",
                    Surname = "Abitbol",
                    Email = "patrick@abitbol.com",
                    Password = "qwerty",
                    Apikey = "xxxxx"
                });
                _context.SaveChanges();
            }
        }

        // GET: api/users
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _context.Users.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
