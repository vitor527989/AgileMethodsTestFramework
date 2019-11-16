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
    public class TestResultController : ControllerBase
    {
        private readonly AMContext _context;

        public TestResultController(AMContext context)
        {
            _context = context;
        }

        // GET: api/TestResult
        [HttpGet]
        public IEnumerable<TestResult> GetTestResults()
        {
            return _context.TestResults;
        }

        // GET: api/TestResult/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTestResult([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var TestResult = await _context.TestResults.FindAsync(id);

            if (TestResult == null)
            {
                return NotFound();
            }

            return Ok(TestResult);
        }

        // PUT: api/TestResult/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTestResult([FromRoute] long id, [FromBody] TestResult TestResult)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != TestResult.Id)
            {
                return BadRequest();
            }

            _context.Entry(TestResult).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestResultExists(id))
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

        // POST: api/TestResult
        [HttpPost]
        public async Task<IActionResult> PostTestResult([FromBody] TestResult TestResult)
        {
            bool guardar = true;
            if (!ModelState.IsValid || TestResult == null || TestResult.Student == null)
            {
                return BadRequest(ModelState);
            }
            
           /*  if(TestResult.TestResultPaiId != null){
                TestResult pai = await _context.TestResults.FindAsync(TestResult.TestResultPaiId);
                Dimensao dimensaoPai = await _context.Dimensoes.FindAsync(pai.IdDimensao);
                Dimensao dimensaoFilho = await _context.Dimensoes.FindAsync(TestResult.IdDimensao);
                guardar = dimensaoFilho.compararDimensoes(dimensaoPai);
            } */
            
            if(guardar){
                _context.TestResults.Add(TestResult);
                await _context.SaveChangesAsync();

                return Ok(TestResult);
            } else {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/TestResult/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestResult([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var TestResult = await _context.TestResults.FindAsync(id);
            if (TestResult == null)
            {
                return NotFound();
            }

            _context.TestResults.Remove(TestResult);
            await _context.SaveChangesAsync();

            return Ok(TestResult);
        }
        private bool TestResultExists(long id)
        {
            return _context.TestResults.Any(e => e.Id == id);
        }
    }
}
