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
    public class BreweriesController : ControllerBase
    {
        private readonly BreweryContext _context;

        public BreweriesController(BreweryContext context)
        {
            _context = context;
        }

        // GET: api/Breweries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brewery>>> GetBreweries()
        {
          if (_context.Breweries == null)
          {
              return NotFound();
          }
            return await _context.Breweries.ToListAsync();
        }

        // GET: api/Breweries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Brewery>> GetBrewery(int? id)
        {
          if (_context.Breweries == null)
          {
              return NotFound();
          }
            var brewery = await _context.Breweries.FindAsync(id);

            if (brewery == null)
            {
                return NotFound();
            }

            return brewery;
        }

        // PUT: api/Breweries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrewery(int? id, Brewery brewery)
        {
            if (id != brewery.IdBrewery)
            {
                return BadRequest();
            }

            _context.Entry(brewery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BreweryExists(id))
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

        // POST: api/Breweries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Brewery>> PostBrewery(Brewery brewery)
        {
          if (_context.Breweries == null)
          {
              return Problem("Entity set 'BreweryContext.Breweries'  is null.");
          }
            _context.Breweries.Add(brewery);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBrewery", new { id = brewery.IdBrewery }, brewery);
        }

        // DELETE: api/Breweries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrewery(int? id)
        {
            if (_context.Breweries == null)
            {
                return NotFound();
            }
            var brewery = await _context.Breweries.FindAsync(id);
            if (brewery == null)
            {
                return NotFound();
            }

            _context.Breweries.Remove(brewery);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("LogicalDelete")]
        public IActionResult LogicalDeleteBrewery(int[] ids)
        {
            foreach (int id in ids)
            {
                var brewery = _context.Breweries.FirstOrDefault(t => t.IdBrewery == id);
                if (brewery != null)
                {
                    brewery.IsDeleted = true;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("LogicalRestore")]
        public IActionResult LogicalRestoreBrewery(int[] ids)
        {
            foreach (int id in ids)
            {
                var brewery = _context.Breweries.FirstOrDefault(t => t.IdBrewery == id);
                if (brewery != null)
                {
                    brewery.IsDeleted = false;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("paged_brewery_logs")]
        public async Task<ActionResult<List<List<Brewery>>>> GetPagedBeerCheque(int Pages, int Entities)
        {
            var totalLogsCount = await _context.Breweries.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / Entities);

            var answer = new List<List<Brewery>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.Breweries
                    .Where(log => log.IdBrewery >= currentId)
                    .OrderBy(log => log.IdBrewery)
                    .Take(Entities)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<Brewery>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < Entities)
                {
                    break;
                }

                currentId = logs.Last().IdBrewery + 1;
            }

            return answer;
        }

        private bool BreweryExists(int? id)
        {
            return (_context.Breweries?.Any(e => e.IdBrewery == id)).GetValueOrDefault();
        }
    }
}
