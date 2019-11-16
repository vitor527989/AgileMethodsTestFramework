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
    public class StudentController : ControllerBase
    {
        private readonly AMContext _context;

        public StudentController(AMContext context)
        {
            _context = context;
        }

        // GET: api/Student
        [HttpGet]
        public IEnumerable<Student> GetStudents()
        {
            return _context.Students;
        }

        // GET: api/Student/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Student = await _context.Students.FindAsync(id);

            if (Student == null)
            {
                return NotFound();
            }

            return Ok(Student);
        }

        // PUT: api/Student/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent([FromRoute] long id, [FromBody] Student Student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Student.Id)
            {
                return BadRequest();
            }

            _context.Entry(Student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // POST: api/Student
        [HttpPost]
        public async Task<IActionResult> PostStudent([FromBody] Student Student)
        {
            bool guardar = true;
            if (!ModelState.IsValid || Student == null || Student.User == null)
            {
                return BadRequest(ModelState);
            }
            
           /*  if(Student.StudentPaiId != null){
                Student pai = await _context.Students.FindAsync(Student.StudentPaiId);
                Dimensao dimensaoPai = await _context.Dimensoes.FindAsync(pai.IdDimensao);
                Dimensao dimensaoFilho = await _context.Dimensoes.FindAsync(Student.IdDimensao);
                guardar = dimensaoFilho.compararDimensoes(dimensaoPai);
            } */
            
            if(guardar){
                _context.Students.Add(Student);
                await _context.SaveChangesAsync();

                return Ok(Student);
            } else {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Student = await _context.Students.FindAsync(id);
            if (Student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(Student);
            await _context.SaveChangesAsync();

            return Ok(Student);
        }
        private bool StudentExists(long id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}