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
    public class QuestionCorrectAnswerController : ControllerBase
    {
        private readonly AMContext _context;

        public QuestionCorrectAnswerController(AMContext context)
        {
            _context = context;
        }

        // GET: api/QuestionCorrectAnswer
        [HttpGet]
        public IEnumerable<QuestionCorrectAnswer> GetQuestionCorrectAnswers()
        {
            return _context.QuestionCorrectAnswers;
        }

        // GET: api/QuestionCorrectAnswer/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestionCorrectAnswer([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var QuestionCorrectAnswer = await _context.QuestionCorrectAnswers.FindAsync(id);

            if (QuestionCorrectAnswer == null)
            {
                return NotFound();
            }

            return Ok(QuestionCorrectAnswer);
        }

        // PUT: api/QuestionCorrectAnswer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestionCorrectAnswer([FromRoute] long id, [FromBody] QuestionCorrectAnswer QuestionCorrectAnswer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != QuestionCorrectAnswer.Id)
            {
                return BadRequest();
            }

            _context.Entry(QuestionCorrectAnswer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionCorrectAnswerExists(id))
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

        // POST: api/QuestionCorrectAnswer
        [HttpPost]
        public async Task<IActionResult> PostQuestionCorrectAnswer([FromBody] QuestionCorrectAnswer QuestionCorrectAnswer)
        {
            bool guardar = true;
            if (!ModelState.IsValid || QuestionCorrectAnswer == null)
            {
                return BadRequest(ModelState);
            }
            
           /*  if(QuestionCorrectAnswer.QuestionCorrectAnswerPaiId != null){
                QuestionCorrectAnswer pai = await _context.QuestionCorrectAnswers.FindAsync(QuestionCorrectAnswer.QuestionCorrectAnswerPaiId);
                Dimensao dimensaoPai = await _context.Dimensoes.FindAsync(pai.IdDimensao);
                Dimensao dimensaoFilho = await _context.Dimensoes.FindAsync(QuestionCorrectAnswer.IdDimensao);
                guardar = dimensaoFilho.compararDimensoes(dimensaoPai);
            } */
            
            if(guardar){
                _context.QuestionCorrectAnswers.Add(QuestionCorrectAnswer);
                await _context.SaveChangesAsync();

                return Ok(QuestionCorrectAnswer);
            } else {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/QuestionCorrectAnswer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestionCorrectAnswer([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var QuestionCorrectAnswer = await _context.QuestionCorrectAnswers.FindAsync(id);
            if (QuestionCorrectAnswer == null)
            {
                return NotFound();
            }

            _context.QuestionCorrectAnswers.Remove(QuestionCorrectAnswer);
            await _context.SaveChangesAsync();

            return Ok(QuestionCorrectAnswer);
        }
        private bool QuestionCorrectAnswerExists(long id)
        {
            return _context.QuestionCorrectAnswers.Any(e => e.Id == id);
        }
    }
}
