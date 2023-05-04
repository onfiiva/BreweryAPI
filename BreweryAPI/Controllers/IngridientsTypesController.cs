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
    public class IngridientsTypesController : ControllerBase
    {
        private readonly BreweryContext _context;

        public IngridientsTypesController(BreweryContext context)
        {
            _context = context;
        }

        // GET: api/IngridientsTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngridientsType>>> GetIngridientsTypes()
        {
          if (_context.IngridientsTypes == null)
          {
              return NotFound();
          }
            return await _context.IngridientsTypes.ToListAsync();
        }

        // GET: api/IngridientsTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IngridientsType>> GetIngridientsType(int? id)
        {
          if (_context.IngridientsTypes == null)
          {
              return NotFound();
          }
            var ingridientsType = await _context.IngridientsTypes.FindAsync(id);

            if (ingridientsType == null)
            {
                return NotFound();
            }

            return ingridientsType;
        }

        // PUT: api/IngridientsTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngridientsType(int? id, IngridientsType ingridientsType)
        {
            if (id != ingridientsType.IdIngridientType)
            {
                return BadRequest();
            }

            _context.Entry(ingridientsType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngridientsTypeExists(id))
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

        // POST: api/IngridientsTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IngridientsType>> PostIngridientsType(IngridientsType ingridientsType)
        {
          if (_context.IngridientsTypes == null)
          {
              return Problem("Entity set 'BreweryContext.IngridientsTypes'  is null.");
          }
            _context.IngridientsTypes.Add(ingridientsType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIngridientsType", new { id = ingridientsType.IdIngridientType }, ingridientsType);
        }

        // DELETE: api/IngridientsTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngridientsType(int? id)
        {
            if (_context.IngridientsTypes == null)
            {
                return NotFound();
            }
            var ingridientsType = await _context.IngridientsTypes.FindAsync(id);
            if (ingridientsType == null)
            {
                return NotFound();
            }

            _context.IngridientsTypes.Remove(ingridientsType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("LogicalDelete")]
        public IActionResult LogicalDeleteIngridientsTypes(int[] ids)
        {
            foreach (int id in ids)
            {
                var ingridientsTypes = _context.IngridientsTypes.FirstOrDefault(t => t.IdIngridientType == id);
                if (ingridientsTypes != null)
                {
                    ingridientsTypes.IsDeleted = true;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("LogicalRestore")]
        public IActionResult LogicalRestoreIngridientsTypes(int[] ids)
        {
            foreach (int id in ids)
            {
                var ingridientsTypes = _context.IngridientsTypes.FirstOrDefault(t => t.IdIngridientType == id);
                if (ingridientsTypes != null)
                {
                    ingridientsTypes.IsDeleted = false;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("paged_ingridient_type_logs")]
        public async Task<ActionResult<List<List<IngridientsType>>>> GetPagedBeerCheque(int Pages, int Entities)
        {
            var totalLogsCount = await _context.IngridientsTypes.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / Entities);

            var answer = new List<List<IngridientsType>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.IngridientsTypes
                    .Where(log => log.IdIngridientType >= currentId)
                    .OrderBy(log => log.IdIngridientType)
                    .Take(Entities)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<IngridientsType>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < Entities)
                {
                    break;
                }

                currentId = logs.Last().IdIngridientType + 1;
            }

            return answer;
        }

        private bool IngridientsTypeExists(int? id)
        {
            return (_context.IngridientsTypes?.Any(e => e.IdIngridientType == id)).GetValueOrDefault();
        }
    }
}
