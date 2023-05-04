using BreweryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BreweryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersListsController : ControllerBase
    {
        private readonly BreweryContext _context;

        public UsersListsController(BreweryContext context)
        {
            _context = context;
        }
        // GET: api/VisitorTicketViews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserList>>> GetUsersLists()
        {
            if (_context.UserLists == null)
            {
                return NotFound();
            }
            return await _context.UserLists.ToListAsync();
        }
    }
}
