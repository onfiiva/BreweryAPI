﻿using BreweryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BreweryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChequesListsController : ControllerBase
    {
        private readonly BreweryContext _context;

        public ChequesListsController(BreweryContext context)
        {
            _context = context;
        }
        // GET: api/VisitorTicketViews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChequeList>>> GetChequesLists()
        {
            if (_context.ChequeLists == null)
            {
                return NotFound();
            }
            return await _context.ChequeLists.ToListAsync();
        }
    }
}
