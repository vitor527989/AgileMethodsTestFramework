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
    public class TestQuestionController : ControllerBase
    {
        private readonly AMContext _context;

        public TestQuestionController(AMContext context)
        {
            _context = context;
        }

        // GET: api/TestQuestions
        [HttpGet]
        public IEnumerable<TestQuestion> GetTestQuestions()
        {
            return _context.TestQuestions;
        }

        // GET: api/TestQuestions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTestQuestions([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var TestQuestions = await _context.TestQuestions.FindAsync(id);

            if (TestQuestions == null)
            {
                return NotFound();
            }

            return Ok(TestQuestions);
        }

        // PUT: api/TestQuestions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTestQuestions([FromRoute] long id, [FromBody] TestQuestion TestQuestions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(TestQuestions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestQuestionExists(id))
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

        // POST: api/TestQuestions
        [HttpPost]
        public async Task<IActionResult> PostTestQuestions([FromBody] TestQuestion TestQuestion)
        {
            bool guardar = true;
            if (!ModelState.IsValid || TestQuestion == null)
            {
                return BadRequest(ModelState);
            }
            
           /*  if(TestQuestions.TestQuestionsPaiId != null){
                TestQuestions pai = await _context.TestQuestionss.FindAsync(TestQuestions.TestQuestionsPaiId);
                Dimensao dimensaoPai = await _context.Dimensoes.FindAsync(pai.IdDimensao);
                Dimensao dimensaoFilho = await _context.Dimensoes.FindAsync(TestQuestions.IdDimensao);
                guardar = dimensaoFilho.compararDimensoes(dimensaoPai);
            } */
            
            if(guardar){
                _context.TestQuestions.Add(TestQuestion);
                await _context.SaveChangesAsync();

                return Ok(TestQuestion);
            } else {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/TestQuestions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestQuestions([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var TestQuestions = await _context.TestQuestions.FindAsync(id);
            if (TestQuestions == null)
            {
                return NotFound();
            }

            _context.TestQuestions.Remove(TestQuestions);
            await _context.SaveChangesAsync();

            return Ok(TestQuestions);
        }
        private bool TestQuestionExists(long id)
        {
            return _context.TestQuestions.Any(e => e.Id == id);
        }
    }
}
