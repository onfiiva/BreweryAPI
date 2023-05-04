using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BreweryAPI.Models;

namespace BreweryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly BreweryContext _context;

        public TokensController(BreweryContext context)
        {
            _context = context;
        }

        // GET: api/Tokens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Token>>> GetTokens()
        {
          if (_context.Tokens == null)
          {
              return NotFound();
          }
            return await _context.Tokens.ToListAsync();
        }

        // GET: api/Tokens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Token>> GetToken(int? id)
        {
          if (_context.Tokens == null)
          {
              return NotFound();
          }
            var token = await _context.Tokens.FindAsync(id);

            if (token == null)
            {
                return NotFound();
            }

            return token;
        }

        // PUT: api/Tokens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToken(int? id, Token token)
        {
            if (id != token.IdToken)
            {
                return BadRequest();
            }

            _context.Entry(token).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TokenExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tokens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Token>> PostToken(Token token)
        {
          if (_context.Tokens == null)
          {
              return Problem("Entity set 'BreweryContext.Token'  is null.");
          }
            _context.Tokens.Add(token);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToken", new { id = token.IdToken }, token);
        }

        // DELETE: api/Tokens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToken(int? id)
        {
            if (_context.Tokens == null)
            {
                return NotFound();
            }
            var token = await _context.Tokens.FindAsync(id);
            if (token == null)
            {
                return NotFound();
            }

            _context.Tokens.Remove(token);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> TokenActualise(string TV)
        {
            var existingTokens = await _context.Tokens.Where(t => t.TokenValue == TV).ToListAsync();

            if (existingTokens.Count == 0)
            {
                return NotFound("No token detected");
            }
            else if (existingTokens.Count > 1)
            {
                return BadRequest("Multiple tokens detected");
            }

            var existingToken = existingTokens[0];
            existingToken.TokenDateTime = DateTime.Now;

            _context.Entry(existingToken).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Actualise token error");
            }

            return NoContent();
        }


        private bool TokenExists(int? id)
        {
            return (_context.Tokens?.Any(e => e.IdToken == id)).GetValueOrDefault();
        }
    }
}
