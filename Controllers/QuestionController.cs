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
    public class QuestionController : ControllerBase
    {
        private readonly AMContext _context;

        public QuestionController(AMContext context)
        {
            _context = context;
        }

        // GET: api/Question
        [HttpGet]
        public IEnumerable<Question> GetQuestions()
        {
            return _context.Questions;
        }

        // GET: api/Question/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestion([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Question = await _context.Questions.FindAsync(id);

            if (Question == null)
            {
                return NotFound();
            }

            return Ok(Question);
        }

        // PUT: api/Question/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion([FromRoute] long id, [FromBody] Question Question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Question.Id)
            {
                return BadRequest();
            }

            _context.Entry(Question).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
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

        // POST: api/Question
        [HttpPost]
        public async Task<IActionResult> PostQuestion([FromBody] Question Question)
        {
            bool guardar = true;
            if (!ModelState.IsValid || Question == null || Question.Title == null)
            {
                return BadRequest(ModelState);
            }
            
           /*  if(Question.QuestionPaiId != null){
                Question pai = await _context.Questions.FindAsync(Question.QuestionPaiId);
                Dimensao dimensaoPai = await _context.Dimensoes.FindAsync(pai.IdDimensao);
                Dimensao dimensaoFilho = await _context.Dimensoes.FindAsync(Question.IdDimensao);
                guardar = dimensaoFilho.compararDimensoes(dimensaoPai);
            } */
            
            if(guardar){
                _context.Questions.Add(Question);
                await _context.SaveChangesAsync();

                return Ok(Question);
            } else {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Question/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Question = await _context.Questions.FindAsync(id);
            if (Question == null)
            {
                return NotFound();
            }

            _context.Questions.Remove(Question);
            await _context.SaveChangesAsync();

            return Ok(Question);
        }
        private bool QuestionExists(long id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}
