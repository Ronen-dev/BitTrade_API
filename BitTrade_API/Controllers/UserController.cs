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

                _context.Users.Add(new User {
                    Firstname = "Patrick",
                    Surname = "Abitbol",
                    Email = "patrick@abitbol.com",
                    Password = "qwerty",
                    Apikey = "xxxxx",
                    StatutiId = Models.User.REF_STATUT_ENABLE
                });
                _context.SaveChanges();
            }
        }

        // GET: api/user   return list of Users
        [HttpGet]
        [Route("")]
        [Route("/")]
        public IEnumerable<User> Get() => _context.Users.ToList();

        // GET api/user/id  return user who have field id equal param id
        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetById(long id)
        {
            var user = _context.Users.FirstOrDefault(t => t.Id == id);

            if (user == null)
                return NotFound();

            return new ObjectResult(user);  
        }

        // POST api/user
        [HttpPost]
        public IActionResult Create([FromBody] User client)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == client.Email);

            if (client == null)
            {
                return BadRequest(new { restult = -999, message = "Error Params" });
            }
            else if (user != null)
            {
                return BadRequest(new { restult = -1, message = $"Email {client.Email} est deja utilisé." });
            }
            else {
                _context.Users.Add(client);
                _context.SaveChanges();
                return CreatedAtRoute("GetUser", new { id = client.Id }, client);
            }
        }

        // PUT api/user/2
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] User client)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            if (client == null || client.Id != id) {
                return BadRequest(new { restult = -999, message = "Error Params" });
            }
            else if (user == null)
            {
                return NotFound(new { restult = -1, message = "utilisateur inconnus" });
            }
            else if (user.Email != client.Email)
            {
                return NotFound(new { restult = -2, message = "Vous ne pouvez pas modifier le compte d'un autre utilisateur" });
            }
            else {
                user.Firstname = client.Firstname;
                user.Surname = client.Surname;
                user.Password = client.Password;
                user.Apikey = client.Apikey;

                _context.Users.Update(user);
                _context.SaveChanges();

                return CreatedAtRoute("GetUser", new { id = user.Id }, user);
            }
        }

        // delete (disable) User with his Id
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            user.StatutiId = Models.User.REF_STATUT_DISABLE;

            _context.Users.Update(user);
            _context.SaveChanges();

            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }
    }
}
