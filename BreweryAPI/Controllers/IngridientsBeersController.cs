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
    public class IngridientsBeersController : ControllerBase
    {
        private readonly BreweryContext _context;

        public IngridientsBeersController(BreweryContext context)
        {
            _context = context;
        }

        // GET: api/IngridientsBeers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngridientsBeer>>> GetIngridientsBeers()
        {
          if (_context.IngridientsBeers == null)
          {
              return NotFound();
          }
            return await _context.IngridientsBeers.ToListAsync();
        }

        // GET: api/IngridientsBeers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IngridientsBeer>> GetIngridientsBeer(int? id)
        {
          if (_context.IngridientsBeers == null)
          {
              return NotFound();
          }
            var ingridientsBeer = await _context.IngridientsBeers.FindAsync(id);

            if (ingridientsBeer == null)
            {
                return NotFound();
            }

            return ingridientsBeer;
        }

        // PUT: api/IngridientsBeers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngridientsBeer(int? id, IngridientsBeer ingridientsBeer)
        {
            if (id != ingridientsBeer.IdUsersBeer)
            {
                return BadRequest();
            }

            _context.Entry(ingridientsBeer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngridientsBeerExists(id))
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

        // POST: api/IngridientsBeers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IngridientsBeer>> PostIngridientsBeer(IngridientsBeer ingridientsBeer)
        {
          if (_context.IngridientsBeers == null)
          {
              return Problem("Entity set 'BreweryContext.IngridientsBeers'  is null.");
          }
            _context.IngridientsBeers.Add(ingridientsBeer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIngridientsBeer", new { id = ingridientsBeer.IdUsersBeer }, ingridientsBeer);
        }

        // DELETE: api/IngridientsBeers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngridientsBeer(int? id)
        {
            if (_context.IngridientsBeers == null)
            {
                return NotFound();
            }
            var ingridientsBeer = await _context.IngridientsBeers.FindAsync(id);
            if (ingridientsBeer == null)
            {
                return NotFound();
            }

            _context.IngridientsBeers.Remove(ingridientsBeer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("LogicalDelete")]
        public IActionResult LogicalDeleteIngridientsBeer(int[] ids)
        {
            foreach (int id in ids)
            {
                var ingridientsBeer = _context.IngridientsBeers.FirstOrDefault(t => t.IdUsersBeer == id);
                if (ingridientsBeer != null)
                {
                    ingridientsBeer.IsDeleted = true;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("LogicalRestore")]
        public IActionResult LogicalRestoreIngridientsBeer(int[] ids)
        {
            foreach (int id in ids)
            {
                var ingridientsBeer = _context.IngridientsBeers.FirstOrDefault(t => t.IdUsersBeer == id);
                if (ingridientsBeer != null)
                {
                    ingridientsBeer.IsDeleted = false;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("paged_ingridients_beer_logs")]
        public async Task<ActionResult<List<List<IngridientsBeer>>>> GetPagedBeerCheque(int Pages, int Entities)
        {
            var totalLogsCount = await _context.IngridientsBeers.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / Entities);

            var answer = new List<List<IngridientsBeer>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.IngridientsBeers
                    .Where(log => log.IdUsersBeer >= currentId)
                    .OrderBy(log => log.IdUsersBeer)
                    .Take(Entities)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<IngridientsBeer>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < Entities)
                {
                    break;
                }

                currentId = logs.Last().IdUsersBeer + 1;
            }

            return answer;
        }

        private bool IngridientsBeerExists(int? id)
        {
            return (_context.IngridientsBeers?.Any(e => e.IdUsersBeer == id)).GetValueOrDefault();
        }
    }
}
