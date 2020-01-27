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
    public class TeacherController : ControllerBase
    {
        private readonly AMContext _context;

        public TeacherController(AMContext context)
        {
            _context = context;
        }

        // GET: api/Teacher
        [HttpGet]
        public IEnumerable<Teacher> GetTeachers()
        {
            return _context.Teachers;
        }

        [HttpGet("DTO")]
        public async Task<IActionResult> GetTeachersDTO()
        {
            IEnumerable<Teacher> teachers = GetTeachers();
            IEnumerable<SubjectUser> subjectUsers = _context.SubjectsUsers;
            List<TeacherDTO> teachersDTOs = new List<TeacherDTO>();
            foreach (Teacher t in teachers)
            {
                User u = await _context.Users.FindAsync(t.IdUser);
                SubjectUser temp = new SubjectUser();
                foreach(SubjectUser su in subjectUsers){
                    if(su.IdUser == u.Id){
                        temp = su;
                    }
                }
                TeacherDTO dto = new TeacherDTO();
                dto.Login = u.Login;
                dto.Password = u.Password;
                dto.Name = u.Name;
                dto.Office = t.Office;
                Subject s = await _context.Subjects.FindAsync(temp.IdSubject);
                dto.Subject = s.Name;
                teachersDTOs.Add(dto);
            }
            return Ok(teachersDTOs);
        }

        // GET: api/Teacher/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacher([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Teacher = await _context.Teachers.FindAsync(id);

            if (Teacher == null)
            {
                return NotFound();
            }

            return Ok(Teacher);
        }

        // PUT: api/Teacher/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher([FromRoute] long id, [FromBody] Teacher Teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Teacher.Id)
            {
                return BadRequest();
            }

            _context.Entry(Teacher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
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

        // POST: api/Teacher
        [HttpPost]
        public async Task<IActionResult> PostTeacher([FromBody] Teacher Teacher)
        {
            bool guardar = true;
            if (!ModelState.IsValid || Teacher == null)
            {
                return BadRequest(ModelState);
            }
            
            if(guardar){
                _context.Teachers.Add(Teacher);
                await _context.SaveChangesAsync();

                return Ok(Teacher);
            } else {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Teacher/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Teacher = await _context.Teachers.FindAsync(id);
            if (Teacher == null)
            {
                return NotFound();
            }

            _context.Teachers.Remove(Teacher);
            await _context.SaveChangesAsync();

            return Ok(Teacher);
        }
        private bool TeacherExists(long id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}
