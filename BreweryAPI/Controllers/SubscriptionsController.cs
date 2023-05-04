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
    public class SubscriptionsController : ControllerBase
    {
        private readonly BreweryContext _context;

        public SubscriptionsController(BreweryContext context)
        {
            _context = context;
        }

        // GET: api/Subscriptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetSubscriptions()
        {
          if (_context.Subscriptions == null)
          {
              return NotFound();
          }
            return await _context.Subscriptions.ToListAsync();
        }

        // GET: api/Subscriptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subscription>> GetSubscription(int? id)
        {
          if (_context.Subscriptions == null)
          {
              return NotFound();
          }
            var subscription = await _context.Subscriptions.FindAsync(id);

            if (subscription == null)
            {
                return NotFound();
            }

            return subscription;
        }

        // PUT: api/Subscriptions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubscription(int? id, Subscription subscription)
        {
            if (id != subscription.IdSubscription)
            {
                return BadRequest();
            }

            _context.Entry(subscription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionExists(id))
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

        // POST: api/Subscriptions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Subscription>> PostSubscription(Subscription subscription)
        {
          if (_context.Subscriptions == null)
          {
              return Problem("Entity set 'BreweryContext.Subscriptions'  is null.");
          }
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubscription", new { id = subscription.IdSubscription }, subscription);
        }

        // DELETE: api/Subscriptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(int? id)
        {
            if (_context.Subscriptions == null)
            {
                return NotFound();
            }
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }

            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("LogicalDelete")]
        public IActionResult LogicalDeleteSubscription(int[] ids)
        {
            foreach (int id in ids)
            {
                var subscription = _context.Subscriptions.FirstOrDefault(t => t.IdSubscription == id);
                if (subscription != null)
                {
                    subscription.IsDeleted = true;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("LogicalRestore")]
        public IActionResult LogicalRestoreSubscription(int[] ids)
        {
            foreach (int id in ids)
            {
                var subscription = _context.Subscriptions.FirstOrDefault(t => t.IdSubscription == id);
                if (subscription != null)
                {
                    subscription.IsDeleted = false;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("paged_subsription_logs")]
        public async Task<ActionResult<List<List<Subscription>>>> GetPagedBeerCheque(int Pages, int Entities)
        {
            var totalLogsCount = await _context.Subscriptions.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / Entities);

            var answer = new List<List<Subscription>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.Subscriptions
                    .Where(log => log.IdSubscription >= currentId)
                    .OrderBy(log => log.IdSubscription)
                    .Take(Entities)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<Subscription>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < Entities)
                {
                    break;
                }

                currentId = logs.Last().IdSubscription + 1;
            }

            return answer;
        }

        private bool SubscriptionExists(int? id)
        {
            return (_context.Subscriptions?.Any(e => e.IdSubscription == id)).GetValueOrDefault();
        }
    }
}
