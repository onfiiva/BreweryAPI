using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BreweryAPI.Models;

namespace BreweryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeersListsController : ControllerBase
    {
        private readonly BreweryContext _context;

        public BeersListsController(BreweryContext context)
        {
            _context = context;
        }
        // GET: api/VisitorTicketViews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BeerList>>> GetBeersLists()
        {
            if (_context.BeerLists == null)
            {
                return NotFound();
            }
            return await _context.BeerLists.ToListAsync();
        }
    }
}
