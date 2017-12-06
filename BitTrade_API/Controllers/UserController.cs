using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BitTrade_API.Models;
using System.Net;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BitTrade_API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly BitTradeContext _context;


        public UserController(BitTradeContext context) {

            _context = context;

        }


        // GET: api/user   return list of Users
        [HttpGet]
        [Route("")]
        [Route("/")]
        public IEnumerable<User> Get() => _context.Users.ToList();



        // GET api/user/id return User if exist with Token, we connect him
        [HttpPost]
        [Route("/user/login")]
        public IActionResult Login([FromBody] User client)
        {
            client.Password = Models.User.MD5Hash(client.Password);

            var user = _context.Users.FirstOrDefault(u => u.Email == client.Email);

            if (user == null || user.StatutId == Models.User.REF_STATUT_DISABLE)
            {
                return NotFound();
            }

            if (user.Email != client.Email || user.Password != client.Password)
            {
                return BadRequest();
            }

            user.Token = Models.User.GetToken();

            _context.Users.Update(user);
            _context.SaveChanges();

            user.Password = "XXX";

            return Ok(user);
            
        }


        // GET api/user/id  return user who have field id equal param id
        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetById(long id)
        {
            var user = _context.Users.FirstOrDefault(t => t.Id == id);

            if (user == null) {
                return NotFound();
            }

            if (Models.User.TokenIsValid(user.Token) == false) {
                return BadRequest();
            } 

            user.Password = "XXX";

            Response.StatusCode = (int)HttpStatusCode.OK;

            return Ok( user );

        }


        // POST api/user 
        [HttpPost]
        [Route("/user/create")]
        public IActionResult Create([FromBody] User client)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == client.Email);

            if (client == null)
            {
                return BadRequest(new { result = -999, message = "Error Params" });
            }
            else if (user != null)
            {
                return BadRequest(new { result = -1, message = $"Email {client.Email} est deja utilisé." });
            }
            else {

                client.Password = Models.User.MD5Hash(client.Password);
                client.Token = Models.User.GetToken();

                _context.Users.Add(client);
                _context.SaveChanges();

                client.Password = "XXX";

                return new ObjectResult(new { result = 1, message = "ajout utilisateur [OK]", user = client });

                //return CreatedAtRoute("GetUser", new { id = client.Id }, client);
            }
        }


        // PUT api/user/2
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] User client)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            if (client == null || client.Id != id) {
                return BadRequest(new { result = -999, message = "Error Params" });
            }
            else if (user == null)
            {
                return NotFound(new { result = -1, message = "Utilisateur inconnus" });
            }
            else if (user.Email != client.Email)
            {
                return NotFound(new { result = -2, message = "Vous ne pouvez pas modifier le compte d'un autre utilisateur" });
            }
            else {

                if (Models.User.TokenIsValid(user.Token) == false)
                {
                    return new ObjectResult(new { result = -666, message = "Go Login Page" });
                }

                user.Firstname = client.Firstname;
                user.Surname = client.Surname;
                user.Password = Models.User.MD5Hash(client.Password);
                user.Apikey = client.Apikey;

                _context.Users.Update(user);
                _context.SaveChanges();

                user.Password = "XXX";

                return new ObjectResult(new { result = 1, message = $"modification de l'utilisateur {user.Firstname} [OK]", user = user });
            }
        }


        // delete (disable) User with his Id
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { result = -999, message = "Error Params" });
            }

            if (Models.User.TokenIsValid(user.Token) == false)
            {
                return new ObjectResult(new { result = -666, message = "Go Login Page" });
            }

            user.StatutId = Models.User.REF_STATUT_DISABLE;

            _context.Users.Update(user);
            _context.SaveChanges();

            user.Password = "XXX";

            return new ObjectResult(new { result = 1, message = $"suppression de l'utilisateur {user.Firstname} [OK]", user = user });
        }
    }
}
