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
    public class SubjectUserController : ControllerBase
    {
        private readonly AMContext _context;

        public SubjectUserController(AMContext context)
        {
            _context = context;
        }

        // GET: api/SubjectUser
        [HttpGet]
        public IEnumerable<SubjectUser> GetSubjectUsers()
        {
            return _context.SubjectsUsers;
        }

        // GET: api/SubjectUser/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubjectUser([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var SubjectUser = await _context.SubjectsUsers.FindAsync(id);

            if (SubjectUser == null)
            {
                return NotFound();
            }

            return Ok(SubjectUser);
        }

        // PUT: api/SubjectUser/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubjectUser([FromRoute] long id, [FromBody] SubjectUser SubjectUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != SubjectUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(SubjectUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectUserExists(id))
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

        // POST: api/SubjectUser
        [HttpPost]
        public async Task<IActionResult> PostSubjectUser([FromBody] SubjectUser SubjectUser)
        {
            bool guardar = true;
            if (!ModelState.IsValid || SubjectUser == null)
            {
                return BadRequest(ModelState);
            }
            if(guardar){
                _context.SubjectsUsers.Add(SubjectUser);
                await _context.SaveChangesAsync();

                return Ok(SubjectUser);
            } else {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/SubjectUser/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubjectUser([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var SubjectUser = await _context.SubjectsUsers.FindAsync(id);
            if (SubjectUser == null)
            {
                return NotFound();
            }

            _context.SubjectsUsers.Remove(SubjectUser);
            await _context.SaveChangesAsync();

            return Ok(SubjectUser);
        }
        private bool SubjectUserExists(long id)
        {
            return _context.SubjectsUsers.Any(e => e.Id == id);
        }
    }
}
