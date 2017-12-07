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
        public IActionResult GetListUser() {

            List<User> users = _context.Users.ToList();

            if (users != null) {
                return Ok(new { success = true, message = "Liste des Utilisateurs", result = users });
            }
            return BadRequest(new { success = false, message = " Error Params !" });
        }



        // GET api/user/id return User if exist with Token, we connect him
        [HttpPost]
        [Route("/user/login")]
        public IActionResult Login([FromBody] User client)
        {
            client.Password = Models.User.MD5Hash(client.Password);

            var user = _context.Users.FirstOrDefault(u => u.Email == client.Email);

            if (user == null || user.StatutId == Models.User.REF_STATUT_DISABLE)
            {
                return BadRequest(new { success = false, message = "Error Params" });
            }

            if (user.Password != client.Password)
            {
                return BadRequest(new { success = false, message = " Mot de passe incorrect !" });
            }

            user.Token = Models.User.GetToken();

            _context.Users.Update(user);
            _context.SaveChanges();

            user.Password = "";

            List<User> users = new List<User>{user};

            return Ok(new { success = true, message = " Utilisateur Connecté !", result = users });
            
        }

        // GET api/user/id  return user who have field id equal param id
        [HttpGet("{id}")]
        [Route("/user/checktoken")]
        public IActionResult GetTokenIsValid(long id)
        {
            var user = _context.Users.FirstOrDefault(t => t.Id == id);

            if (user == null)
            {
                return BadRequest(new { success = false, message = "Utilisateur inexistant !" });
            }

            if (Models.User.TokenIsValid(user.Token) == false)
            {
                return BadRequest(new { success = false, message = "Utilisateur Déconnecté !" });
            }

            user.Password = "";

            List<User> users = new List<User> { user };

            return Ok(new { success = true, message = " Le token est toujour valide", result = users });
        }

        // GET api/user/id  return user who have field id equal param id
        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetById(long id)
        {
            var user = _context.Users.FirstOrDefault(t => t.Id == id);

            if (user == null) {
                return BadRequest(new { success = false, message = "Utilisateur inexistant !" });
            }

            if (Models.User.TokenIsValid(user.Token) == false) {
                return BadRequest(new { success = false, message = "Utilisateur Déconnecté !" });
            } 

            user.Password = "";

            List<User> users = new List<User> { user };

            return Ok(new { success = true, message = " Information personnel Utilisateur!", result = users });
        }


        // POST api/user 
        [HttpPost]
        [Route("/user/create")]
        public IActionResult Create([FromBody] User client)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == client.Email);

            if (client == null)
            {
                return BadRequest(new { success = false, message = "Error Params !" });
            }
            else if (user != null)
            {
                return BadRequest(new { success = false, message = $"Email {client.Email} est deja utilisé." });
            }
            else {

                client.Password = Models.User.MD5Hash(client.Password);
                client.Token = Models.User.GetToken();

                _context.Users.Add(client);
                _context.SaveChanges();

                client.Password = "";

                List<User> users = new List<User> { user };

                return Ok(new { success = true, message = " Vous etes inscrit sur BitTrade", result = users });

                //return CreatedAtRoute("GetUser", new { id = client.Id }, client);
            }
        }


        // PUT api/user/2
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] User client)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            if (user == null || client == null || client.Id != id) {
                return BadRequest(new { success = false, message = "Error Params !" });
            }
            else if (user.Email != client.Email)
            {
                return BadRequest(new { success = false, message = "Vous ne pouvez pas modifier votre email !" });
            }
            else {

                if (Models.User.TokenIsValid(user.Token) == false)
                {
                    return BadRequest(new { success = false, message = "Utilisateur Déconnecté !" });
                }

                user.Firstname = client.Firstname;
                user.Surname = client.Surname;
                user.Password = Models.User.MD5Hash(client.Password);
                user.Apikey = client.Apikey;

                _context.Users.Update(user);
                _context.SaveChanges();

                user.Password = "";

                List<User> users = new List<User> { user };

                return Ok(new { success = true, message = $"Modification des informations enregistrées pour l'utilisateur {user.Email} [OK]", result = users });
            }
        }


        // delete (disable) User with his Id
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return BadRequest(new { success = false, message = "Error Params !" });
            }

            if (Models.User.TokenIsValid(user.Token) == false)
            {
                return BadRequest(new { success = false, message = "Utilisateur Déconnecté !" });
            }

            user.StatutId = Models.User.REF_STATUT_DISABLE;

            _context.Users.Update(user);
            _context.SaveChanges();

            user.Password = "";

            List<User> users = new List<User> { user };

            return Ok(new { success = true, message = $"Suppression l'utilisateur {user.Email} [OK]", result = users });
        }
    }
}
