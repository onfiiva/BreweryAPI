using BreweryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BreweryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngridientsListsController : ControllerBase
    {
        private readonly BreweryContext _context;

        public IngridientsListsController(BreweryContext context)
        {
            _context = context;
        }
        // GET: api/VisitorTicketViews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngridientsList>>> GetIngridientsLists()
        {
            if (_context.IngridientsLists == null)
            {
                return NotFound();
            }
            return await _context.IngridientsLists.ToListAsync();
        }
    }
}
