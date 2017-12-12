using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BitTrade_API.Models;

namespace BitTrade_API.Controllers
{
    [Produces("application/json")]
    [Route("api/UserCurrencies")]
    public class UserCurrenciesController : Controller
    {
        private readonly BitTradeContext _context;

        public UserCurrenciesController(BitTradeContext context)
        {
            _context = context;
        }
        
        // GET: api/UserCurrencies/5
        [HttpGet("{id}", Name = "GetFavoris")]
        public IActionResult GetById(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = " Error Params !" });
            }

            List<UserCurrencies> listeFavoris = _context.UserCurrencies.Where(m => m.UserForeignKey == id).GroupBy(m => m.MarketName).Select(x => x.First()).ToList();

            if ( listeFavoris == null || listeFavoris.Count() < 1 )
            {
                return NotFound(new { success = false, message = "L'utilisateur ne possede pas de favoris" });
            }
            
            return Ok(new { success = true, message = "Liste des favoris de l'utilisateur", result = listeFavoris });
        }


        // POST: api/UserCurrencies
        [HttpPost]
        public IActionResult PostUserCurrencies([FromBody] UserCurrencies userCurrencies)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = " Error Params Favoris !" });
            }

            _context.UserCurrencies.Add(userCurrencies);
            _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Crypto ajouté aux Favoris", result = userCurrencies });
        }


        
        // DELETE: api/UserCurrencies/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUserCurrencies([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = " Error Params !" });
            }

            UserCurrencies userCurrencies = _context.UserCurrencies.SingleOrDefault(m => m.Id == id);

            if (userCurrencies == null)
            {
                return NotFound(new { success = false, message = "Erreur cette Crypto n'est pas en favoris pour l'utilisateur" });
            }

            _context.UserCurrencies.Remove(userCurrencies);
            _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Crypto supprimé des Favoris", result = userCurrencies });
        }

        private bool UserCurrenciesExists(long id)
        {
            return _context.UserCurrencies.Any(e => e.Id == id);
        }
    }
}