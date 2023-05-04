using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BreweryAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace BreweryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly BreweryContext _context;

        public AdminsController(BreweryContext context)
        {
            _context = context;
        }

        // GET: api/Admins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmins()
        {
          if (_context.Admins == null)
          {
              return NotFound();
          }
            return await _context.Admins.ToListAsync();
        }

        // GET: api/Admins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdmin(int id)
        {
          if (_context.Admins == null)
          {
              return NotFound();
          }
            var admin = await _context.Admins.FindAsync(id);

            if (admin == null)
            {
                return NotFound();
            }

            return admin;
        }

        // PUT: api/Admins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmin(int id, Admin admin)
        {
            if (id != admin.IdAdmin)
            {
                return BadRequest();
            }

            _context.Entry(admin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(id))
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

        // POST: api/Admins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Admin>> PostAdmin(Admin admin)
        {
            if (await _context.Admins.AnyAsync(u => u.LoginAdmin == admin.LoginAdmin))
            {
                return BadRequest("Identical Admin login detected");
            }

            byte[] Salt = GenerateSalt(20);
            admin.PasswordSalt = Convert.ToBase64String(Salt);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(admin.PasswordAdmin);
            byte[] hashedBytes = new Rfc2898DeriveBytes(passwordBytes, Salt, 10000).GetBytes(32);
            admin.PasswordAdmin = Convert.ToBase64String(hashedBytes);

            if (_context.Admins == null)
            {
                return Problem("Entity set 'MyDbContext.Admins'  is null.");
            }
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdmin", new { id = admin.IdAdmin }, admin);
        }

        // AUTH: api/Admins/5
        [HttpGet("{PhoneAdmin}/{Password}")]
        public async Task<ActionResult<string>> Authorization(string PhoneAdmin, string Password)
        {
            var admins = await _context.Admins.Where(u => u.PhoneAdmin == PhoneAdmin).ToListAsync();

            if (admins.Count == 0)
            {
                // пользователь не найден
                return NotFound();
            }
            else if (admins.Count > 1)
            {
                // обнаружено несколько пользователей с таким именем
                return BadRequest("Multiple usernames detected");
            }

            var admin = admins[0];
            // преобразовываем строку Salt в массив байтов
            byte[] saltBytes = Convert.FromBase64String(admin.PasswordSalt);

            byte[] reverseSalt = saltBytes.Reverse().ToArray();

            string hashedReverse = Convert.ToBase64String(reverseSalt);

            // преобразовываем строку Password в массив байтов
            byte[] passwordBytes = Encoding.UTF8.GetBytes(Password);

            // вычисляем хеш пароля с помощью соли и 10000 итераций
            byte[] hashBytes = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 10000).GetBytes(32);
            string hashedPassword = Convert.ToBase64String(hashBytes);

            if (hashedPassword == admin.PasswordAdmin)
            {
                // пароль совпадает, генерируем случайный токен и добавляем его в базу данных
                string token;
                Token existingToken;

                do
                {
                    token = Guid.NewGuid().ToString();
                    existingToken = await _context.Tokens.FirstOrDefaultAsync(t => t.TokenValue == token);
                }
                while (existingToken != null);

                // создаем новую запись Token и сохраняем ее в базу данных
                Token tok = new Token();
                tok.TokenValue = token;
                tok.TokenDateTime = DateTime.Now;
                _context.Tokens.Add(tok);
                await _context.SaveChangesAsync();

                return token;
            }
            else
            {
                // пароль не совпадает
                return BadRequest("Неправильный пароль");
            }
        }

        // GET: api/Admins
        [HttpGet("auth_key")]
        public async Task<ActionResult<string>> GetAuthKey(string PhoneAdmin)
        {
            var admin = await _context.Admins.SingleOrDefaultAsync(u => u.PhoneAdmin == PhoneAdmin);
            if (admin == null)
            {
                return NotFound($"Admin with login '{PhoneAdmin}' was not found.");
            }
            else if (_context.Admins.Count(u => u.LoginAdmin == PhoneAdmin) > 1)
            {
                return BadRequest($"Multiple admins with login '{PhoneAdmin}' were found.");
            }

            string salt = admin.PasswordSalt;
            if (string.IsNullOrEmpty(salt))
            {
                return BadRequest($"Salt for user with login '{PhoneAdmin}' is missing or empty.");
            }

            byte[] saltBytes = Encoding.UTF8.GetBytes(salt.Substring(0, Math.Min(salt.Length, 5)));
            byte[] reverseSalt = saltBytes.Reverse().ToArray();
            string hashedReverse = Convert.ToBase64String(reverseSalt);

            return hashedReverse;
        }

        // GET: api/Admins
        [HttpGet("authentication")]
        public async Task<ActionResult<string>> GetAuthentication(string PhoneAdmin, string AuthKey)
        {
            // Retrieve the user's password salt from the database
            var admin = await _context.Admins.FirstOrDefaultAsync(u => u.PhoneAdmin == PhoneAdmin);
            if (admin == null)
            {
                return BadRequest("Invalid LoginAdmin");
            }
            var salt = admin.PasswordSalt;

            // Compute the AuthKey from the password salt
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt.Substring(0, Math.Min(salt.Length, 5)));
            byte[] reverseSalt = saltBytes.Reverse().ToArray();
            string hashedReverse = Convert.ToBase64String(reverseSalt);

            // Check if the computed AuthKey matches the provided AuthKey
            if (hashedReverse != AuthKey)
            {
                return BadRequest("Invalid AuthKey");
            }

            // Generate a random token and add it to the database
            string token;
            Token existingToken;

            do
            {
                token = Guid.NewGuid().ToString();
                existingToken = await _context.Tokens.FirstOrDefaultAsync(t => t.TokenValue == token);
            }
            while (existingToken != null);

            Token tok = new Token();
            tok.TokenValue = token;
            tok.TokenDateTime = DateTime.Now;
            _context.Tokens.Add(tok);
            await _context.SaveChangesAsync();

            return token;
        }



        [HttpPut("Password_Change")]
        public async Task<IActionResult> ChangePassword(int? id, string New_password)
        {
            var admin = await _context.Admins.FindAsync(id);

            if (admin == null)
            {
                return NotFound();
            }

            // хешируем новый пароль
            byte[] Salt = GenerateSalt(20);
            admin.PasswordSalt = Convert.ToBase64String(Salt);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(New_password);
            byte[] hashBytes = new Rfc2898DeriveBytes(passwordBytes, Salt, 10000).GetBytes(32);
            admin.PasswordAdmin = Convert.ToBase64String(hashBytes);

            _context.Entry(admin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(id))
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

        // DELETE: api/Admins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            if (_context.Admins == null)
            {
                return NotFound();
            }
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPut("LogicalDelete")]
        public IActionResult LogicalDeleteAdmin(int[] ids)
        {
            foreach (int id in ids)
            {
                var admin = _context.Admins.FirstOrDefault(t => t.IdAdmin == id);
                if (admin != null)
                {
                    admin.IsDeleted = true;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("LogicalRestore")]
        public IActionResult LogicalRestoreAdmin(int[] ids)
        {
            foreach (int id in ids)
            {
                var admin = _context.Admins.FirstOrDefault(t => t.IdAdmin == id);
                if (admin != null)
                {
                    admin.IsDeleted = false;
                }
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("paged_admin_logs")]
        public async Task<ActionResult<List<List<Admin>>>> GetPagedAdmin(int Pages, int Entities)
        {
            var totalLogsCount = await _context.Admins.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalLogsCount / Entities);

            var answer = new List<List<Admin>>();

            var currentId = 1;
            while (true)
            {
                var logs = await _context.Admins
                    .Where(log => log.IdAdmin >= currentId)
                    .OrderBy(log => log.IdAdmin)
                    .Take(Entities)
                    .ToListAsync();

                if (!logs.Any())
                {
                    break;
                }

                var pageLogs = new List<Admin>(logs);
                answer.Add(pageLogs);

                if (answer.Count == Pages || logs.Count < Entities)
                {
                    break;
                }

                currentId = logs.Last().IdAdmin + 1;
            }

            return answer;
        }

        private bool AdminExists(int? id)
        {
            return (_context.Admins?.Any(e => e.IdAdmin == id)).GetValueOrDefault();
        }

        public static byte[] GenerateSalt(int length)
        {
            byte[] salt = new byte[length];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
