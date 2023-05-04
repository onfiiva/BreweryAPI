using BreweryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BreweryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreweriesListsController : ControllerBase
    {
        private readonly BreweryContext _context;

        public BreweriesListsController(BreweryContext context)
        {
            _context = context;
        }
        // GET: api/VisitorTicketViews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BreweryList>>> GetBreweriessLists()
        {
            if (_context.BreweryLists == null)
            {
                return NotFound();
            }
            return await _context.BreweryLists.ToListAsync();
        }
    }
}
