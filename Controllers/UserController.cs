using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgileMethodsTestFramework.Models;

namespace AgileMethodsTestFramework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AMContext _context;

        public UserController(AMContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _context.Users;
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByUsername([FromRoute] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IEnumerable<Teacher> teachers = await _context.Teachers.ToListAsync();
            IEnumerable<Student> students = await _context.Students.ToListAsync();

            foreach (Teacher t in teachers)
            {
                User u = await _context.Users.FindAsync(t.IdUser);
                if (u.Login.Equals(username))
                {
                    UserDTO dto = new UserDTO();
                    dto.Login = u.Login;
                    dto.Password = u.Password;
                    dto.Role = "Teacher";
                    return Ok(dto);
                }
            }
            foreach (Student s in students)
            {
                User u = await _context.Users.FindAsync(s.IdUser);
                if (u.Login.Equals(username))
                {
                    UserDTO dto = new UserDTO();
                    dto.Login = u.Login;
                    dto.Password = u.Password;
                    dto.Role = "Student";
                    return Ok(dto);
                }
            }
            return BadRequest("User Not Found");
        }
        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var User = await _context.Users.FindAsync(id);

            if (User == null)
            {
                return NotFound();
            }

            return Ok(User);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] long id, [FromBody] User User)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != User.Id)
            {
                return BadRequest();
            }

            _context.Entry(User).State = EntityState.Modified;

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

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] UserDTO user)
        {
            if (!ModelState.IsValid || user.Login == null || user.Password == null)
            {
                return BadRequest(ModelState);
            }
            IEnumerable<Teacher> teachers = await _context.Teachers.ToListAsync();
            IEnumerable<Student> students = await _context.Students.ToListAsync();

            foreach (Teacher t in teachers)
            {
                User u = await _context.Users.FindAsync(t.IdUser);
                if (u.Login.Equals(user.Login) &&
                    u.Password.Equals(user.Password))
                {
                    UserDTO dto = new UserDTO();
                    dto.Login = u.Login;
                    dto.Password = u.Password;
                    dto.Role = "Teacher";
                    return Ok(dto);
                }
            }
            foreach (Student s in students)
            {
                User u = await _context.Users.FindAsync(s.IdUser);
                if (u.Login.Equals(user.Login) &&
                    u.Password.Equals(user.Password))
                {
                    UserDTO dto = new UserDTO();
                    dto.Login = u.Login;
                    dto.Password = u.Password;
                    dto.Role = "Student";
                    return Ok(dto);
                }
            }
            return BadRequest("Invalid Credentials");

        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var User = await _context.Users.FindAsync(id);
            if (User == null)
            {
                return NotFound();
            }

            _context.Users.Remove(User);
            await _context.SaveChangesAsync();

            return Ok(User);
        }
        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
