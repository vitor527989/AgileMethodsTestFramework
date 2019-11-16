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
    public class TestController : ControllerBase
    {
        private readonly AMContext _context;

        public TestController(AMContext context)
        {
            _context = context;
        }

        // GET: api/Test
        [HttpGet]
        public IEnumerable<Test> GetTests()
        {
            return _context.Tests;
        }

        // GET: api/Test/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTest([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Test = await _context.Tests.FindAsync(id);

            if (Test == null)
            {
                return NotFound();
            }

            return Ok(Test);
        }

        // PUT: api/Test/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTest([FromRoute] long id, [FromBody] Test Test)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Test.Id)
            {
                return BadRequest();
            }

            _context.Entry(Test).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestExists(id))
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

        // POST: api/Test
        [HttpPost]
        public async Task<IActionResult> PostTest([FromBody] Test Test)
        {
            bool guardar = true;
            if (!ModelState.IsValid || Test == null || Test.Subject == null)
            {
                return BadRequest(ModelState);
            }
            
           /*  if(Test.TestPaiId != null){
                Test pai = await _context.Tests.FindAsync(Test.TestPaiId);
                Dimensao dimensaoPai = await _context.Dimensoes.FindAsync(pai.IdDimensao);
                Dimensao dimensaoFilho = await _context.Dimensoes.FindAsync(Test.IdDimensao);
                guardar = dimensaoFilho.compararDimensoes(dimensaoPai);
            } */
            
            if(guardar){
                _context.Tests.Add(Test);
                await _context.SaveChangesAsync();

                return Ok(Test);
            } else {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Test/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTest([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Test = await _context.Tests.FindAsync(id);
            if (Test == null)
            {
                return NotFound();
            }

            _context.Tests.Remove(Test);
            await _context.SaveChangesAsync();

            return Ok(Test);
        }
        private bool TestExists(long id)
        {
            return _context.Tests.Any(e => e.Id == id);
        }
    }
}
