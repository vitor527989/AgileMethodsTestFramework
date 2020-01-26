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
    public class QuestionPossibleAnswerController : ControllerBase
    {
        private readonly AMContext _context;

        public QuestionPossibleAnswerController(AMContext context)
        {
            _context = context;
        }

        // GET: api/QuestionPossibleAnswer
        [HttpGet]
        public IEnumerable<QuestionPossibleAnswer> GetQuestionPossibleAnswers()
        {
            return _context.QuestionPossibleAnswers;
        }

        // GET: api/QuestionPossibleAnswer/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestionPossibleAnswer([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var QuestionPossibleAnswer = await _context.QuestionPossibleAnswers.FindAsync(id);

            if (QuestionPossibleAnswer == null)
            {
                return NotFound();
            }

            return Ok(QuestionPossibleAnswer);
        }

        // PUT: api/QuestionPossibleAnswer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestionPossibleAnswer([FromRoute] long id, [FromBody] QuestionPossibleAnswer QuestionPossibleAnswer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != QuestionPossibleAnswer.Id)
            {
                return BadRequest();
            }

            _context.Entry(QuestionPossibleAnswer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionPossibleAnswerExists(id))
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

        // POST: api/QuestionPossibleAnswer
        [HttpPost]
        public async Task<IActionResult> PostQuestionPossibleAnswer([FromBody] QuestionPossibleAnswer QuestionPossibleAnswer)
        {
            bool guardar = true;
            if (!ModelState.IsValid || QuestionPossibleAnswer == null)
            {
                return BadRequest(ModelState);
            }
            
           /*  if(QuestionPossibleAnswer.QuestionPossibleAnswerPaiId != null){
                QuestionPossibleAnswer pai = await _context.QuestionPossibleAnswers.FindAsync(QuestionPossibleAnswer.QuestionPossibleAnswerPaiId);
                Dimensao dimensaoPai = await _context.Dimensoes.FindAsync(pai.IdDimensao);
                Dimensao dimensaoFilho = await _context.Dimensoes.FindAsync(QuestionPossibleAnswer.IdDimensao);
                guardar = dimensaoFilho.compararDimensoes(dimensaoPai);
            } */
            
            if(guardar){
                _context.QuestionPossibleAnswers.Add(QuestionPossibleAnswer);
                await _context.SaveChangesAsync();

                return Ok(QuestionPossibleAnswer);
            } else {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/QuestionPossibleAnswer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestionPossibleAnswer([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var QuestionPossibleAnswer = await _context.QuestionPossibleAnswers.FindAsync(id);
            if (QuestionPossibleAnswer == null)
            {
                return NotFound();
            }

            _context.QuestionPossibleAnswers.Remove(QuestionPossibleAnswer);
            await _context.SaveChangesAsync();

            return Ok(QuestionPossibleAnswer);
        }
        private bool QuestionPossibleAnswerExists(long id)
        {
            return _context.QuestionPossibleAnswers.Any(e => e.Id == id);
        }
    }
}
