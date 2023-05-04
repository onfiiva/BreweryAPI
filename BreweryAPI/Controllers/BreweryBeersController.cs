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
    public class BreweryBeersController : ControllerBase
    {
        private readonly BreweryContext _context;

        public BreweryBeersController(BreweryContext context)
        {
            _context = context;
        }

        // GET: api/BreweryBeers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BreweryBeer>>> GetBreweryBeers()
        {
          if (_context.BreweryBeers == null)
          {
              return NotFound();
          }
            return await _context.BreweryBeers.ToListAsync();
        }

        // GET: api/BreweryBeers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BreweryBeer>> GetBreweryBeer(int? id)
        {
          if (_context.BreweryBeers == null)
          {
              return NotFound();
          }
            var breweryBeer = await _context.BreweryBeers.FindAsync(id);

            if (breweryBeer == null)
            {
                return NotFound();
            }

            return breweryBeer;
        }

        // PUT: api/BreweryBeers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBreweryBeer(int? id, BreweryBeer breweryBeer)
        {
            if (id != breweryBeer.IdBreweryBeer)
            {
                return BadRequest();
            }

            _context.Entry(breweryBeer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BreweryBeerExists(id))
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

        // POST: api/BreweryBeers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BreweryBeer>> PostBreweryBeer(BreweryBeer breweryBeer)
        {
          if (_context.BreweryBeers == null)
          {
              return Problem("Entity set 'BreweryContext.BreweryBeers'  is null.");
          }
            _context.BreweryBeers.Add(breweryBeer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBreweryBeer", new { id = breweryBeer.IdBreweryBeer }, breweryBeer);
        }

        // DELETE: api/BreweryBeers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBreweryBeer(int? id)
        {
            if (_context.BreweryBeers == null)
            {
                return NotFound();
            }
            var breweryBeer = await _context.BreweryBeers.FindAsync(id);
            if (breweryBeer == null)
            {
                return NotFound();
            }

            _context.BreweryBeers.Remove(breweryBeer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("LogicalDelete")]
        public IActionResult LogicalDeleteBreweryBeer(int[] ids)
        {
            foreach (int id in ids)
            {
                var breweryBeer = _context.BreweryBeers.FirstOrDefault(t => t.IdBreweryBeer == id);
                if (breweryBeer != null)
                {
                    breweryBeer.IsDeleted = true;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("LogicalRestore")]
        public IActionResult LogicalRestoreBreweryBeer(int[] ids)
        {
            foreach (int id in ids)
            {
                var breweryBeer = _context.BreweryBeers.FirstOrDefault(t => t.IdBreweryBeer == id);
                if (breweryBeer != null)
                {
                    breweryBeer.IsDeleted = false;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("paged_brewery_beer_logs")]
        public async Task<ActionResult<List<List<BreweryBeer>>>> GetPagedBeerCheque(int Pages, int Entities)
        {
            var totalLogsCount = await _context.BreweryBeers.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / Entities);

            var answer = new List<List<BreweryBeer>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.BreweryBeers
                    .Where(log => log.IdBreweryBeer >= currentId)
                    .OrderBy(log => log.IdBreweryBeer)
                    .Take(Entities)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<BreweryBeer>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < Entities)
                {
                    break;
                }

                currentId = logs.Last().IdBreweryBeer + 1;
            }

            return answer;
        }

        private bool BreweryBeerExists(int? id)
        {
            return (_context.BreweryBeers?.Any(e => e.IdBreweryBeer == id)).GetValueOrDefault();
        }
    }
}
