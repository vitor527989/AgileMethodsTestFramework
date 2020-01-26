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
    public class TestAnswerController : ControllerBase
    {
        private readonly AMContext _context;

        public TestAnswerController(AMContext context)
        {
            _context = context;
        }

        // GET: api/TestAnswers
        [HttpGet]
        public IEnumerable<TestAnswer> GetTestAnswers()
        {
            return _context.TestAnswers;
        }

        // GET: api/TestAnswers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTestAnswers([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var TestAnswers = await _context.TestAnswers.FindAsync(id);

            if (TestAnswers == null)
            {
                return NotFound();
            }

            return Ok(TestAnswers);
        }

        // PUT: api/TestAnswers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTestAnswers([FromRoute] long id, [FromBody] TestAnswer TestAnswers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(TestAnswers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestAnswerExists(id))
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

        // POST: api/TestAnswers
        [HttpPost]
        public async Task<IActionResult> PostTestAnswers([FromBody] TestAnswer TestAnswer)
        {
            bool guardar = true;
            if (!ModelState.IsValid || TestAnswer == null)
            {
                return BadRequest(ModelState);
            }
            
           /*  if(TestAnswers.TestAnswersPaiId != null){
                TestAnswers pai = await _context.TestAnswerss.FindAsync(TestAnswers.TestAnswersPaiId);
                Dimensao dimensaoPai = await _context.Dimensoes.FindAsync(pai.IdDimensao);
                Dimensao dimensaoFilho = await _context.Dimensoes.FindAsync(TestAnswers.IdDimensao);
                guardar = dimensaoFilho.compararDimensoes(dimensaoPai);
            } */
            
            if(guardar){
                _context.TestAnswers.Add(TestAnswer);
                await _context.SaveChangesAsync();

                return Ok(TestAnswer);
            } else {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/TestAnswers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestAnswers([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var TestAnswers = await _context.TestAnswers.FindAsync(id);
            if (TestAnswers == null)
            {
                return NotFound();
            }

            _context.TestAnswers.Remove(TestAnswers);
            await _context.SaveChangesAsync();

            return Ok(TestAnswers);
        }
        private bool TestAnswerExists(long id)
        {
            return _context.TestAnswers.Any(e => e.Id == id);
        }
    }
}
