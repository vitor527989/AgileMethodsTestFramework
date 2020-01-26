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
            if (!ModelState.IsValid || Test == null)
            {
                return BadRequest(ModelState);
            }
            
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

        // GET: api/Test/GenerateTest
        [HttpGet("GenerateTest")]
        public async Task<IActionResult> GenerateTest(string subject, int nQuestions, string login)
        {
            if (!ModelState.IsValid || subject == null || nQuestions == 0)
            {
                return BadRequest(ModelState);
            }

            IEnumerable<Question> questions = _context.Questions;
            IEnumerable<QuestionPossibleAnswer> possibleAnswers = _context.QuestionPossibleAnswers;
            IEnumerable<QuestionCorrectAnswer> correctAnswers = _context.QuestionCorrectAnswers;
            IEnumerable<Answer> answers = _context.Answers;
            IEnumerable<Subject> subjects = _context.Subjects;
            Subject sub = new Subject();

            foreach (Subject s in subjects){
                if(s.Name == subject){
                    sub = s;
                }
            }
            List<Question> questionsSubject = new List<Question>();
            List<Question> questionsTest = new List<Question>();
            TestDTO test = new TestDTO();
            test.questions = new List<QuestionDTO>();
            foreach (Question q in questions)
            {
                if(q.IdSubject == sub.Id){
                    questionsSubject.Add(q);
                }
            }

            var rand = new Random();
            var randomList = questionsSubject.OrderBy (x => rand.Next()).ToList();
            int i = 0;
            List<TestQuestion> questionsOfTheTest = new List<TestQuestion>();
            foreach(Question q in randomList){
                if(i < nQuestions){
                    questionsTest.Add(q);
                    i++;
                }
            }
            if(questionsTest.Count() < nQuestions){
                return BadRequest("Not Enough Questions");
            }
            Test t = new Test();
            t.IdSubject = sub.Id;
            t.IdTeacher = await getIdTeacher(login);
            _context.Tests.Add(t);
            await _context.SaveChangesAsync();
            foreach(Question q in questionsTest){
                TestQuestion tq = new TestQuestion();
                tq.IdQuestion = q.Id;
                tq.IdTest = t.Id;
                 _context.TestQuestions.Add(tq);
                QuestionDTO qDto = await buildQuestionDTO(q, possibleAnswers, correctAnswers,
                    answers);
                test.questions.Add(qDto);
            }
            await _context.SaveChangesAsync();

            return Ok(test);
        }

        private async Task<QuestionDTO> buildQuestionDTO(Question q, IEnumerable<QuestionPossibleAnswer> possibleAnswers,
        IEnumerable<QuestionCorrectAnswer> correctAnswers, IEnumerable<Answer> answers){
            QuestionDTO dto = new QuestionDTO();
            dto.Title = q.Title;
            dto.PossibleAnswers = new List<AnswerDTO>();
            Subject s = await _context.Subjects.FindAsync(q.IdSubject); 
            dto.Subject = s.Name;
            foreach (QuestionPossibleAnswer qpa in possibleAnswers){
                if (qpa.IdQuestion == q.Id){
                    AnswerDTO ansDto = new AnswerDTO();
                    foreach (Answer a in answers){
                        if (a.Id == qpa.IdPossibleAnswer){
                            ansDto.Description = a.Description;
                            break;
                        }
                    }
                    dto.PossibleAnswers.Add(ansDto);
                }
            }
            foreach (QuestionCorrectAnswer qca in correctAnswers){
                if (qca.IdQuestion == q.Id){
                    AnswerDTO ansDto = new AnswerDTO();
                    foreach (Answer a in answers){
                        if (a.Id == qca.IdCorrectAnswer){
                            ansDto.Description = a.Description;
                            break;
                        }
                    }
                    dto.CorrectAnswer = ansDto;
                }
            }
            return dto;
        }
    
        private async Task<long> getIdTeacher(string login){
            IEnumerable<Teacher> teachers = _context.Teachers;
            foreach(Teacher t in teachers){
                User u = await _context.Users.FindAsync(t.IdUser);
                if(u.Login == login){
                    return u.Id;
                }
            }
            return -1;
        }
    }
}
