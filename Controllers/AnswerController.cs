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
    public class AnswerController : ControllerBase
    {
        private readonly AMContext _context;

        public AnswerController(AMContext context)
        {
            _context = context;
        }

        // GET: api/Answer
        [HttpGet]
        public IEnumerable<Answer> GetAnswers()
        {
            return _context.Answers;
        }

        // GET: api/Answer/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnswer([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Answer = await _context.Answers.FindAsync(id);

            if (Answer == null)
            {
                return NotFound();
            }

            return Ok(Answer);
        }

        // PUT: api/Answer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnswer([FromRoute] long id, [FromBody] Answer Answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Answer.Id)
            {
                return BadRequest();
            }

            _context.Entry(Answer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(id))
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

        // POST: api/Answer
        [HttpPost]
        public async Task<IActionResult> PostAnswer([FromBody] Answer Answer)
        {
            bool guardar = true;
            if (!ModelState.IsValid || Answer == null || Answer.Description == null)
            {
                return BadRequest(ModelState);
            }
            
           /*  if(Answer.AnswerPaiId != null){
                Answer pai = await _context.Answers.FindAsync(Answer.AnswerPaiId);
                Dimensao dimensaoPai = await _context.Dimensoes.FindAsync(pai.IdDimensao);
                Dimensao dimensaoFilho = await _context.Dimensoes.FindAsync(Answer.IdDimensao);
                guardar = dimensaoFilho.compararDimensoes(dimensaoPai);
            } */
            
            if(guardar){
                _context.Answers.Add(Answer);
                await _context.SaveChangesAsync();

                return Ok(Answer);
            } else {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Answer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnswer([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Answer = await _context.Answers.FindAsync(id);
            if (Answer == null)
            {
                return NotFound();
            }

            _context.Answers.Remove(Answer);
            await _context.SaveChangesAsync();

            return Ok(Answer);
        }
        private bool AnswerExists(long id)
        {
            return _context.Answers.Any(e => e.Id == id);
        }
    }
}
