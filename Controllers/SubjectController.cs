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
    public class SubjectController : ControllerBase
    {
        private readonly AMContext _context;

        public SubjectController(AMContext context)
        {
            _context = context;
        }

        // GET: api/Subject
        [HttpGet]
        public IEnumerable<Subject> GetSubjects()
        {
            return _context.Subjects;
        }

        // GET: api/Subject/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubject([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Subject = await _context.Subjects.FindAsync(id);

            if (Subject == null)
            {
                return NotFound();
            }

            return Ok(Subject);
        }

        // PUT: api/Subject/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubject([FromRoute] long id, [FromBody] Subject Subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Subject.Id)
            {
                return BadRequest();
            }

            _context.Entry(Subject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(id))
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

        // POST: api/Subject
        [HttpPost]
        public async Task<IActionResult> PostSubject([FromBody] Subject Subject)
        {
            bool guardar = true;
            if (!ModelState.IsValid || Subject == null || Subject.Name == null)
            {
                return BadRequest(ModelState);
            }
            
           /*  if(Subject.SubjectPaiId != null){
                Subject pai = await _context.Subjects.FindAsync(Subject.SubjectPaiId);
                Dimensao dimensaoPai = await _context.Dimensoes.FindAsync(pai.IdDimensao);
                Dimensao dimensaoFilho = await _context.Dimensoes.FindAsync(Subject.IdDimensao);
                guardar = dimensaoFilho.compararDimensoes(dimensaoPai);
            } */
            
            if(guardar){
                _context.Subjects.Add(Subject);
                await _context.SaveChangesAsync();

                return Ok(Subject);
            } else {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Subject/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Subject = await _context.Subjects.FindAsync(id);
            if (Subject == null)
            {
                return NotFound();
            }

            _context.Subjects.Remove(Subject);
            await _context.SaveChangesAsync();

            return Ok(Subject);
        }
        private bool SubjectExists(long id)
        {
            return _context.Subjects.Any(e => e.Id == id);
        }
    }
}
