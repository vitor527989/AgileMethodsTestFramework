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

        [HttpPost("SaveAnswers")]
        public async Task<IActionResult> saveAnswers([FromBody] TestAnswerDTO testAnswers){
            IEnumerable<Question> questions = _context.Questions;
            IEnumerable<QuestionPossibleAnswer> possibleAnswers = _context.QuestionPossibleAnswers;
            IEnumerable<QuestionCorrectAnswer> correctAnswers = _context.QuestionCorrectAnswers;
            IEnumerable<Answer> answers = _context.Answers;
            IEnumerable<Subject> subjects = _context.Subjects;
            IEnumerable<Student> students = _context.Students;
            IEnumerable<Test> tests = _context.Tests;
            
            int numQuestions = testAnswers.AnswersToQuestions.Count();
            int valueOfEachQuestion = 100/numQuestions;
            int maxResult = 0;
            Test test = new Test();
            foreach(Test t in tests){
                bool areEqual = await testCompare(t, testAnswers.Test);
                if(!areEqual){
                    test = t;
                }
            }
            Student student = new Student();
            foreach(Student s in students){
                if(s.Number == testAnswers.Student.Number){
                    student = s;
                    break;
                }
            }
            List<string> questionTitles = new List<string>();
            foreach(QuestionDTO q in testAnswers.Test.questions){
                questionTitles.Add(q.Title);
            }
            List<Question> questionsFromTest = new List<Question>();
            foreach(string s in questionTitles){
                foreach(Question q in questions){
                    if(q.Title == s){
                        questionsFromTest.Add(q);
                    }
                }
            }
            List<TestAnswer> testAnswersToAdd = new List<TestAnswer>();
            foreach(Question q in questionsFromTest){
                Answer correctAnswer = new Answer();
                List<Answer> answersFromQuestion = new List<Answer>();
                foreach(QuestionPossibleAnswer qpa in possibleAnswers){
                    if(qpa.IdQuestion == q.Id){
                        Answer a = await _context.Answers.FindAsync(qpa.IdPossibleAnswer);
                        answersFromQuestion.Add(a);
                    }
                }
                foreach(QuestionCorrectAnswer qca in correctAnswers){
                    if(qca.IdQuestion == q.Id){
                        correctAnswer = await _context.Answers.FindAsync(qca.IdCorrectAnswer);
                    }
                }
                AnswerDTO answerChosenDto = new AnswerDTO();
                foreach(AnswerToQuestionDTO atq in testAnswers.AnswersToQuestions){
                    if(q.Title == atq.Question.Title){
                        answerChosenDto = atq.Answer;
                    }
                }
                Answer answerChosen = new Answer();
                foreach(Answer a in answers){
                    if(a.Description == answerChosenDto.Description){
                        answerChosen = a;
                    }
                }
                TestAnswer ta = new TestAnswer();
                ta.IdQuestion = q.Id;
                ta.IdTest = test.Id;
                ta.IdTestAnswers = answerChosen.Id;
                ta.IdStudent = student.Id;
                testAnswersToAdd.Add(ta);
                if(correctAnswer.Description == answerChosen.Description){
                    maxResult = maxResult + valueOfEachQuestion;
                }
            }

            foreach(TestAnswer ta in testAnswersToAdd){
                _context.TestAnswers.Add(ta);
            }
            TestResult tr = new TestResult();
            tr.IdStudent = student.Id;
            tr.Grade = maxResult;
            _context.TestResults.Add(tr);
            await _context.SaveChangesAsync();
            return Ok(tr.Grade);
        }

        private async Task<bool> testCompare(Test test1, TestDTO test2){
            IEnumerable<Question> questions = _context.Questions;
            IEnumerable<TestQuestion> testQuestions = _context.TestQuestions;
            List<string> questionsTest1 = new List<string>();
            List<string> questionsTest2 = new List<string>();

            foreach(TestQuestion tq in testQuestions){
                if(tq.IdTest  == test1.Id){
                    Question q = await _context.Questions.FindAsync(tq.IdQuestion);
                    questionsTest1.Add(q.Title);
                }
            }
            foreach(Question q in questions){
                foreach(QuestionDTO qdto in test2.questions){
                    if(q.Title  == qdto.Title){
                        questionsTest2.Add(q.Title);
                    }
                }
            }
            return Enumerable.SequenceEqual(questionsTest1, questionsTest2);
        }
    }
}
