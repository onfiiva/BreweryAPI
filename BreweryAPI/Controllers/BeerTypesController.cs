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
    public class BeerTypesController : ControllerBase
    {
        private readonly BreweryContext _context;

        public BeerTypesController(BreweryContext context)
        {
            _context = context;
        }

        // GET: api/BeerTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BeerType>>> GetBeerTypes()
        {
          if (_context.BeerTypes == null)
          {
              return NotFound();
          }
            return await _context.BeerTypes.ToListAsync();
        }

        // GET: api/BeerTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BeerType>> GetBeerType(int? id)
        {
          if (_context.BeerTypes == null)
          {
              return NotFound();
          }
            var beerType = await _context.BeerTypes.FindAsync(id);

            if (beerType == null)
            {
                return NotFound();
            }

            return beerType;
        }

        // PUT: api/BeerTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBeerType(int? id, BeerType beerType)
        {
            if (id != beerType.IdBeerType)
            {
                return BadRequest();
            }

            _context.Entry(beerType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeerTypeExists(id))
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

        // POST: api/BeerTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BeerType>> PostBeerType(BeerType beerType)
        {
          if (_context.BeerTypes == null)
          {
              return Problem("Entity set 'BreweryContext.BeerTypes'  is null.");
          }
            _context.BeerTypes.Add(beerType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBeerType", new { id = beerType.IdBeerType }, beerType);
        }

        // DELETE: api/BeerTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBeerType(int? id)
        {
            if (_context.BeerTypes == null)
            {
                return NotFound();
            }
            var beerType = await _context.BeerTypes.FindAsync(id);
            if (beerType == null)
            {
                return NotFound();
            }

            _context.BeerTypes.Remove(beerType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("LogicalDelete")]
        public IActionResult LogicalDeleteBeerType(int[] ids)
        {
            foreach (int id in ids)
            {
                var beerType = _context.BeerTypes.FirstOrDefault(t => t.IdBeerType == id);
                if (beerType != null)
                {
                    beerType.IsDeleted = true;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("LogicalRestore")]
        public IActionResult LogicalRestoreBeerType(int[] ids)
        {
            foreach (int id in ids)
            {
                var beerType = _context.BeerTypes.FirstOrDefault(t => t.IdBeerType == id);
                if (beerType != null)
                {
                    beerType.IsDeleted = false;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("paged_beer_type_logs")]
        public async Task<ActionResult<List<List<BeerType>>>> GetPagedBeerCheque(int Pages, int Entities)
        {
            var totalLogsCount = await _context.BeerTypes.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / Entities);

            var answer = new List<List<BeerType>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.BeerTypes
                    .Where(log => log.IdBeerType >= currentId)
                    .OrderBy(log => log.IdBeerType)
                    .Take(Entities)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<BeerType>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < Entities)
                {
                    break;
                }

                currentId = logs.Last().IdBeerType + 1;
            }

            return answer;
        }

        private bool BeerTypeExists(int? id)
        {
            return (_context.BeerTypes?.Any(e => e.IdBeerType == id)).GetValueOrDefault();
        }
    }
}
