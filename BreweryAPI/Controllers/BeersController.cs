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
    public class BeersController : ControllerBase
    {
        private readonly BreweryContext _context;

        public BeersController(BreweryContext context)
        {
            _context = context;
        }

        // GET: api/Beers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Beer>>> GetBeers()
        {
          if (_context.Beers == null)
          {
              return NotFound();
          }
            return await _context.Beers.ToListAsync();
        }

        // GET: api/Beers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Beer>> GetBeer(int id)
        {
          if (_context.Beers == null)
          {
              return NotFound();
          }
            var beer = await _context.Beers.FindAsync(id);

            if (beer == null)
            {
                return NotFound();
            }

            return beer;
        }

        [HttpGet("/api/Beers/getType/{id}")]
        public async Task<ActionResult<BeerType>> GetType(int id)
        {
            if (_context.Beers == null)
            {
                return NotFound();
            }
            var beer = await _context.Beers.FindAsync(id);

            if (beer == null)
            {
                return NotFound();
            }

            int idBeerType = beer.BeerTypeId;

            var beerType = await _context.BeerTypes.SingleOrDefaultAsync(u => u.IdBeerType == idBeerType);

            if (beerType == null)
            {
                return NotFound();
            }

            return beerType;
        }

        // PUT: api/Beers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBeer(int id, Beer beer)
        {
            if (id != beer.IdBeer)
            {
                return BadRequest();
            }

            _context.Entry(beer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeerExists(id))
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

        // POST: api/Beers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Beer>> PostBeer(Beer beer)
        {
          if (_context.Beers == null)
          {
              return Problem("Entity set 'BreweryContext.Beers'  is null.");
          }
            _context.Beers.Add(beer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBeer", new { id = beer.IdBeer }, beer);
        }

        // DELETE: api/Beers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBeer(int id)
        {
            if (_context.Beers == null)
            {
                return NotFound();
            }
            var beer = await _context.Beers.FindAsync(id);
            if (beer == null)
            {
                return NotFound();
            }

            _context.Beers.Remove(beer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("LogicalDelete")]
        public IActionResult LogicalDeleteBeer(int[] ids)
        {
            foreach (int id in ids)
            {
                var beer = _context.Beers.FirstOrDefault(t => t.IdBeer == id);
                if (beer != null)
                {
                    beer.IsDeleted = true;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("LogicalRestore")]
        public IActionResult LogicalRestoreBeer(int[] ids)
        {
            foreach (int id in ids)
            {
                var beer = _context.Beers.FirstOrDefault(t => t.IdBeer == id);
                if (beer != null)
                {
                    beer.IsDeleted = false;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("paged_beer_logs")]
        public async Task<ActionResult<IEnumerable<Beer>>> GetPagedBeerCheque(int Pages, int Entities)
        {
            var totalLogsCount = await _context.Beers.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / Entities);

            var answer = new List<Beer>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.Beers
                    .Where(log => log.IdBeer >= currentId)
                    .OrderBy(log => log.IdBeer)
                    .Take(Entities)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                answer.AddRange(logs);

                if (answer.Count == Pages * Entities || logs.Count < Entities)
                {
                    break;
                }

                currentId = logs.Last().IdBeer + 1;
            }

            return answer;
        }

        [HttpGet("/api/Beers/GetBeerByName/{BeerName}")]
        public async Task<ActionResult<Beer>> GetBeerByName(string? BeerName)
        {
            if (_context.Beers == null)
            {
                return NotFound();
            }
            var beer = await _context.Beers.SingleOrDefaultAsync(u => u.NameBeer == BeerName);

            if (beer == null)
            {
                return NotFound();
            }

            return beer;
        }

        [HttpGet("/api/BeerCheques/getBeerType/{BeerName}")]
        public async Task<ActionResult<List<BeerType>>> getBeerType(string BeerName)
        {
            if (_context.Beers == null)
            {
                return NotFound();
            }

            var beer = await _context.Beers
                .Where(u => u.NameBeer == BeerName)
                .ToListAsync();

            if (beer == null)
            {
                return NotFound();
            }

            var beerTypeId = beer.Select(bc => bc.BeerTypeId).ToList();

            var beersType = await _context.BeerTypes
                .Where(b => beerTypeId.Contains(b.IdBeerType))
                .ToListAsync();

            return beersType;
        }


        private bool BeerExists(int id)
        {
            return (_context.Beers?.Any(e => e.IdBeer == id)).GetValueOrDefault();
        }
    }
}
