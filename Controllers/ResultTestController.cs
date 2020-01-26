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
    public class ResultTestController : ControllerBase
    {
        private readonly AMContext _context;

        public ResultTestController(AMContext context)
        {
            _context = context;
        }

        // GET: api/ResultsTest
        [HttpGet]
        public IEnumerable<ResultTest> GetResultTests()
        {
            return _context.ResultsTests;
        }

        // GET: api/ResultsTest/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResultsTest([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ResultsTest = await _context.ResultsTests.FindAsync(id);

            if (ResultsTest == null)
            {
                return NotFound();
            }

            return Ok(ResultsTest);
        }

        // PUT: api/ResultsTest/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResultsTest([FromRoute] long id, [FromBody] ResultTest ResultsTest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ResultsTest.Id)
            {
                return BadRequest();
            }

            _context.Entry(ResultsTest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResultsTestExists(id))
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

        // POST: api/ResultsTest
        [HttpPost]
        public async Task<IActionResult> PostResultsTest([FromBody] ResultTest ResultsTest)
        {
            bool guardar = true;
            if (!ModelState.IsValid || ResultsTest == null)
            {
                return BadRequest(ModelState);
            }
            
            if(guardar){
                _context.ResultsTests.Add(ResultsTest);
                await _context.SaveChangesAsync();

                return Ok(ResultsTest);
            } else {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/ResultsTest/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResultsTest([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ResultsTest = await _context.ResultsTests.FindAsync(id);
            if (ResultsTest == null)
            {
                return NotFound();
            }

            _context.ResultsTests.Remove(ResultsTest);
            await _context.SaveChangesAsync();

            return Ok(ResultsTest);
        }
        private bool ResultsTestExists(long id)
        {
            return _context.ResultsTests.Any(e => e.Id == id);
        }
    }
}
