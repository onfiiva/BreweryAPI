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
    public class BeerChequesController : ControllerBase
    {
        private readonly BreweryContext _context;

        public BeerChequesController(BreweryContext context)
        {
            _context = context;
        }

        // GET: api/BeerCheques
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BeerCheque>>> GetBeerCheques()
        {
          if (_context.BeerCheques == null)
          {
              return NotFound();
          }
            return await _context.BeerCheques.ToListAsync();
        }

        // GET: api/BeerCheques/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BeerCheque>> GetBeerCheque(int id)
        {
          if (_context.BeerCheques == null)
          {
              return NotFound();
          }
            var beerCheque = await _context.BeerCheques.FindAsync(id);

            if (beerCheque == null)
            {
                return NotFound();
            }

            return beerCheque;
        }

        [HttpGet("/api/BeerCheques/getBeersByCheque/{ChequeId}")]
        public async Task<ActionResult<List<Beer>>> GetChequeId(int ChequeId)
        {
            if (_context.BeerCheques == null)
            {
                return NotFound();
            }

            var beerCheque = await _context.BeerCheques
                .Where(u => u.ChequeId == ChequeId)
                .ToListAsync();

            if (beerCheque == null)
            {
                return NotFound();
            }

            var beerIds = beerCheque.Select(bc => bc.BeerId).ToList();

            var beers = await _context.Beers
                .Where(b => beerIds.Contains(b.IdBeer))
                .ToListAsync();

            return beers;
        }

        // PUT: api/BeerCheques/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBeerCheque(int id, BeerCheque beerCheque)
        {
            if (id != beerCheque.IdBeerCheque)
            {
                return BadRequest();
            }

            _context.Entry(beerCheque).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeerChequeExists(id))
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

        // POST: api/BeerCheques
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BeerCheque>> PostBeerCheque(BeerCheque beerCheque)
        {
          if (_context.BeerCheques == null)
          {
              return Problem("Entity set 'BreweryContext.BeerCheques'  is null.");
          }
            _context.BeerCheques.Add(beerCheque);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBeerCheque", new { id = beerCheque.IdBeerCheque }, beerCheque);
        }

        // DELETE: api/BeerCheques/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBeerCheque(int id)
        {
            if (_context.BeerCheques == null)
            {
                return NotFound();
            }
            var beerCheque = await _context.BeerCheques.FindAsync(id);
            if (beerCheque == null)
            {
                return NotFound();
            }

            _context.BeerCheques.Remove(beerCheque);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("LogicalDelete")]
        public IActionResult LogicalDeleteBeerCheque(int[] ids)
        {
            foreach (int id in ids)
            {
                var beerCheque = _context.BeerCheques.FirstOrDefault(t => t.IdBeerCheque == id);
                if (beerCheque != null)
                {
                    beerCheque.IsDeleted = true;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("LogicalRestore")]
        public IActionResult LogicRestoreBeerCheque(int[] ids)
        {
            foreach (int id in ids)
            {
                var beerCheque = _context.BeerCheques.FirstOrDefault(t => t.IdBeerCheque == id);
                if (beerCheque != null)
                {
                    beerCheque.IsDeleted = false;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("paged_beer_cheque_logs")]
        public async Task<ActionResult<List<List<BeerCheque>>>> GetPagedBeerCheque(int Pages, int Entities)
        {
            var totalLogsCount = await _context.BeerCheques.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / Entities);

            var answer = new List<List<BeerCheque>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.BeerCheques
                    .Where(log => log.IdBeerCheque >= currentId)
                    .OrderBy(log => log.IdBeerCheque)
                    .Take(Entities)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<BeerCheque>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < Entities)
                {
                    break;
                }

                currentId = logs.Last().IdBeerCheque + 1;
            }

            return answer;
        }

        private bool BeerChequeExists(int id)
        {
            return (_context.BeerCheques?.Any(e => e.IdBeerCheque == id)).GetValueOrDefault();
        }
    }
}
