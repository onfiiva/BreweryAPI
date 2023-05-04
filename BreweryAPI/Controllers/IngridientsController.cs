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
    public class IngridientsController : ControllerBase
    {
        private readonly BreweryContext _context;

        public IngridientsController(BreweryContext context)
        {
            _context = context;
        }

        // GET: api/Ingridients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingridient>>> GetIngridients()
        {
          if (_context.Ingridients == null)
          {
              return NotFound();
          }
            return await _context.Ingridients.ToListAsync();
        }

        // GET: api/Ingridients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ingridient>> GetIngridient(int? id)
        {
          if (_context.Ingridients == null)
          {
              return NotFound();
          }
            var ingridient = await _context.Ingridients.FindAsync(id);

            if (ingridient == null)
            {
                return NotFound();
            }

            return ingridient;
        }

        // PUT: api/Ingridients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngridient(int? id, Ingridient ingridient)
        {
            if (id != ingridient.IdIngridient)
            {
                return BadRequest();
            }

            _context.Entry(ingridient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngridientExists(id))
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

        // POST: api/Ingridients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ingridient>> PostIngridient(Ingridient ingridient)
        {
          if (_context.Ingridients == null)
          {
              return Problem("Entity set 'BreweryContext.Ingridients'  is null.");
          }
            _context.Ingridients.Add(ingridient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIngridient", new { id = ingridient.IdIngridient }, ingridient);
        }

        // DELETE: api/Ingridients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngridient(int? id)
        {
            if (_context.Ingridients == null)
            {
                return NotFound();
            }
            var ingridient = await _context.Ingridients.FindAsync(id);
            if (ingridient == null)
            {
                return NotFound();
            }

            _context.Ingridients.Remove(ingridient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("LogicalDelete")]
        public IActionResult LogicalDeleteIngridients(int[] ids)
        {
            foreach (int id in ids)
            {
                var ingridients = _context.Ingridients.FirstOrDefault(t => t.IdIngridient == id);
                if (ingridients != null)
                {
                    ingridients.IsDeleted = true;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("LogicalRestore")]
        public IActionResult LogicalRestoreIngridients(int[] ids)
        {
            foreach (int id in ids)
            {
                var ingridients = _context.Ingridients.FirstOrDefault(t => t.IdIngridient == id);
                if (ingridients != null)
                {
                    ingridients.IsDeleted = false;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("paged_ingridient_logs")]
        public async Task<ActionResult<List<List<Ingridient>>>> GetPagedBeerCheque(int Pages, int Entities)
        {
            var totalLogsCount = await _context.Ingridients.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / Entities);

            var answer = new List<List<Ingridient>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.Ingridients
                    .Where(log => log.IdIngridient >= currentId)
                    .OrderBy(log => log.IdIngridient)
                    .Take(Entities)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<Ingridient>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < Entities)
                {
                    break;
                }

                currentId = logs.Last().IdIngridient + 1;
            }

            return answer;
        }

        private bool IngridientExists(int? id)
        {
            return (_context.Ingridients?.Any(e => e.IdIngridient == id)).GetValueOrDefault();
        }
    }
}
