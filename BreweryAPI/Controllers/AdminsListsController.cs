using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BreweryAPI.Models;

namespace BreweryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsListsController : ControllerBase
    {
        private readonly BreweryContext _context;

        public AdminsListsController(BreweryContext context)
        {
            _context = context;
        }
        // GET: api/VisitorTicketViews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminList>>> GetAdminsLists()
        {
            if (_context.AdminLists == null)
            {
                return NotFound();
            }
            return await _context.AdminLists.ToListAsync();
        }
    }
}