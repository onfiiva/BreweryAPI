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
    public class BreweryIngridientsController : ControllerBase
    {
        private readonly BreweryContext _context;

        public BreweryIngridientsController(BreweryContext context)
        {
            _context = context;
        }

        // GET: api/BreweryIngridients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BreweryIngridient>>> GetBreweryIngridients()
        {
          if (_context.BreweryIngridients == null)
          {
              return NotFound();
          }
            return await _context.BreweryIngridients.ToListAsync();
        }

        // GET: api/BreweryIngridients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BreweryIngridient>> GetBreweryIngridient(int? id)
        {
          if (_context.BreweryIngridients == null)
          {
              return NotFound();
          }
            var breweryIngridient = await _context.BreweryIngridients.FindAsync(id);

            if (breweryIngridient == null)
            {
                return NotFound();
            }

            return breweryIngridient;
        }

        // PUT: api/BreweryIngridients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBreweryIngridient(int? id, BreweryIngridient breweryIngridient)
        {
            if (id != breweryIngridient.IdBreweryIngridients)
            {
                return BadRequest();
            }

            _context.Entry(breweryIngridient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BreweryIngridientExists(id))
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

        // POST: api/BreweryIngridients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BreweryIngridient>> PostBreweryIngridient(BreweryIngridient breweryIngridient)
        {
          if (_context.BreweryIngridients == null)
          {
              return Problem("Entity set 'BreweryContext.BreweryIngridients'  is null.");
          }
            _context.BreweryIngridients.Add(breweryIngridient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBreweryIngridient", new { id = breweryIngridient.IdBreweryIngridients }, breweryIngridient);
        }

        // DELETE: api/BreweryIngridients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBreweryIngridient(int? id)
        {
            if (_context.BreweryIngridients == null)
            {
                return NotFound();
            }
            var breweryIngridient = await _context.BreweryIngridients.FindAsync(id);
            if (breweryIngridient == null)
            {
                return NotFound();
            }

            _context.BreweryIngridients.Remove(breweryIngridient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("LogicalDelete")]
        public IActionResult LogicalDeleteBreweryIngridient(int[] ids)
        {
            foreach (int id in ids)
            {
                var breweryIngridients = _context.BreweryIngridients.FirstOrDefault(t => t.IdBreweryIngridients == id);
                if (breweryIngridients != null)
                {
                    breweryIngridients.IsDeleted = true;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("LogicalRestore")]
        public IActionResult LogicalRestoreBreweryIngridient(int[] ids)
        {
            foreach (int id in ids)
            {
                var breweryIngridients = _context.BreweryIngridients.FirstOrDefault(t => t.IdBreweryIngridients == id);
                if (breweryIngridients != null)
                {
                    breweryIngridients.IsDeleted = false;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("paged_brewery_ingridient_logs")]
        public async Task<ActionResult<List<List<BreweryIngridient>>>> GetPagedBeerCheque(int Pages, int Entities)
        {
            var totalLogsCount = await _context.BreweryIngridients.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / Entities);

            var answer = new List<List<BreweryIngridient>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.BreweryIngridients
                    .Where(log => log.IdBreweryIngridients >= currentId)
                    .OrderBy(log => log.IdBreweryIngridients)
                    .Take(Entities)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<BreweryIngridient>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < Entities)
                {
                    break;
                }

                currentId = logs.Last().IdBreweryIngridients + 1;
            }

            return answer;
        }

        private bool BreweryIngridientExists(int? id)
        {
            return (_context.BreweryIngridients?.Any(e => e.IdBreweryIngridients == id)).GetValueOrDefault();
        }
    }
}
