using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WinCordApi.Models;

namespace WinCordApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUser()
        {
            var users = await _context.Users
                .ToListAsync();

            var userDtos = new List<UserDto>();

            foreach (var user in users) 
            {
                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Points = user.Points,
                });
            }

            return userDtos;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.User == null)
          {
              return NotFound();
          }
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserLoginDto userLoginDto)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'AppDbContext.User'  is null.");
            }

            var user = new User()
            {
                Name = userLoginDto.Name,
                Password = SecureHasher.Hash(userLoginDto.Password),
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            userLoginDto.Id = user.Id;

            userLoginDto.Password = "";
            return CreatedAtAction("GetUser", new { id = user.Id }, userLoginDto);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return SecureHasher.Verify(password, hashedPassword);
        }

        // POST: api/Users/Login
        [HttpPost("Login")]
        public async Task<ActionResult<User>> PostUserLogin(UserLoginDto userLoginDto)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'AppDbContext.User'  is null.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == userLoginDto.Name);

            if (user == null)
            {
                return NotFound();
            }

            else if (VerifyPassword(userLoginDto.Password, user.Password))
            {
                userLoginDto.Id = user.Id;
                userLoginDto.Password = "";
                return CreatedAtAction("PostUserLogin", userLoginDto);
            }

            return NotFound();

        }
    }
}
