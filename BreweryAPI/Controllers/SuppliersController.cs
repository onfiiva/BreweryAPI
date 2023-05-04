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
    public class SuppliersController : ControllerBase
    {
        private readonly BreweryContext _context;

        public SuppliersController(BreweryContext context)
        {
            _context = context;
        }

        // GET: api/Suppliers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
        {
          if (_context.Suppliers == null)
          {
              return NotFound();
          }
            return await _context.Suppliers.ToListAsync();
        }

        // GET: api/Suppliers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplier(int? id)
        {
          if (_context.Suppliers == null)
          {
              return NotFound();
          }
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }

            return supplier;
        }

        // PUT: api/Suppliers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplier(int? id, Supplier supplier)
        {
            if (id != supplier.IdSupplier)
            {
                return BadRequest();
            }

            _context.Entry(supplier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
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

        // POST: api/Suppliers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Supplier>> PostSupplier(Supplier supplier)
        {
          if (_context.Suppliers == null)
          {
              return Problem("Entity set 'BreweryContext.Suppliers'  is null.");
          }
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSupplier", new { id = supplier.IdSupplier }, supplier);
        }

        // DELETE: api/Suppliers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int? id)
        {
            if (_context.Suppliers == null)
            {
                return NotFound();
            }
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("LogicalDelete")]
        public IActionResult LogicalDeleteSupplier(int[] ids)
        {
            foreach (int id in ids)
            {
                var supplier = _context.Suppliers.FirstOrDefault(t => t.IdSupplier == id);
                if (supplier != null)
                {
                    supplier.IsDeleted = true;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("LogicalRestore")]
        public IActionResult LogicalRestoreSupplier(int[] ids)
        {
            foreach (int id in ids)
            {
                var supplier = _context.Suppliers.FirstOrDefault(t => t.IdSupplier == id);
                if (supplier != null)
                {
                    supplier.IsDeleted = false;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("paged_supplier_logs")]
        public async Task<ActionResult<List<List<Supplier>>>> GetPagedBeerCheque(int Pages, int Entities)
        {
            var totalLogsCount = await _context.Suppliers.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / Entities);

            var answer = new List<List<Supplier>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.Suppliers
                    .Where(log => log.IdSupplier >= currentId)
                    .OrderBy(log => log.IdSupplier)
                    .Take(Entities)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<Supplier>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < Entities)
                {
                    break;
                }

                currentId = logs.Last().IdSupplier + 1;
            }

            return answer;
        }

        private bool SupplierExists(int? id)
        {
            return (_context.Suppliers?.Any(e => e.IdSupplier == id)).GetValueOrDefault();
        }
    }
}
