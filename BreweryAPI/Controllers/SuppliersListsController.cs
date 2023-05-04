using BreweryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BreweryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersListsController : ControllerBase
    {
        private readonly BreweryContext _context;

        public SuppliersListsController(BreweryContext context)
        {
            _context = context;
        }
        // GET: api/VisitorTicketViews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SuppliersList>>> GetSuppliersLists()
        {
            if (_context.SuppliersLists == null)
            {
                return NotFound();
            }
            return await _context.SuppliersLists.ToListAsync();
        }
    }
}
