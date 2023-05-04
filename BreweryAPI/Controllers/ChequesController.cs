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
    public class ChequesController : ControllerBase
    {
        private readonly BreweryContext _context;

        public ChequesController(BreweryContext context)
        {
            _context = context;
        }

        // GET: api/Cheques
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cheque>>> GetCheques()
        {
          if (_context.Cheques == null)
          {
              return NotFound();
          }
            return await _context.Cheques.ToListAsync();
        }

        // GET: api/Cheques/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cheque>> GetCheque(int? id)
        {
          if (_context.Cheques == null)
          {
              return NotFound();
          }
            var cheque = await _context.Cheques.FindAsync(id);

            if (cheque == null)
            {
                return NotFound();
            }

            return cheque;
        }

        [HttpGet("/api/Cheques/getChequesByUser/{UserId}")]
        public async Task<ActionResult<IEnumerable<Cheque>>> GetChequesByUser(int? UserId)
        {
            if (_context.Cheques == null)
            {
                return NotFound();
            }
            var cheque = await _context.Cheques.Where(u => u.UserId == UserId).ToListAsync();

            if (cheque == null)
            {
                return NotFound();
            }

            return cheque;
        }

        // PUT: api/Cheques/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCheque(int? id, Cheque cheque)
        {
            if (id != cheque.IdCheque)
            {
                return BadRequest();
            }

            _context.Entry(cheque).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChequeExists(id))
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

        // POST: api/Cheques
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cheque>> PostCheque(Cheque cheque)
        {
          if (_context.Cheques == null)
          {
              return Problem("Entity set 'BreweryContext.Cheques'  is null.");
          }
            _context.Cheques.Add(cheque);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCheque", new { id = cheque.IdCheque }, cheque);
        }

        // DELETE: api/Cheques/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCheque(int? id)
        {
            if (_context.Cheques == null)
            {
                return NotFound();
            }
            var cheque = await _context.Cheques.FindAsync(id);
            if (cheque == null)
            {
                return NotFound();
            }

            _context.Cheques.Remove(cheque);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("LogicalDelete")]
        public IActionResult LogicalDeleteCheque(int[] ids)
        {
            foreach (int id in ids)
            {
                var cheque = _context.Cheques.FirstOrDefault(t => t.IdCheque == id);
                if (cheque != null)
                {
                    cheque.IsDeleted = true;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("LogicalRestore")]
        public IActionResult LogicalRestoreCheque(int[] ids)
        {
            foreach (int id in ids)
            {
                var cheque = _context.Cheques.FirstOrDefault(t => t.IdCheque == id);
                if (cheque != null)
                {
                    cheque.IsDeleted = false;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("paged_cheque_logs")]
        public async Task<ActionResult<List<List<Cheque>>>> GetPagedBeerCheque(int Pages, int Entities)
        {
            var totalLogsCount = await _context.Cheques.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / Entities);

            var answer = new List<List<Cheque>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.Cheques
                    .Where(log => log.IdCheque >= currentId)
                    .OrderBy(log => log.IdCheque)
                    .Take(Entities)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<Cheque>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < Entities)
                {
                    break;
                }

                currentId = logs.Last().IdCheque + 1;
            }

            return answer;
        }

        private bool ChequeExists(int? id)
        {
            return (_context.Cheques?.Any(e => e.IdCheque == id)).GetValueOrDefault();
        }
    }
}
