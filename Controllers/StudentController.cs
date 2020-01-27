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
    public class StudentController : ControllerBase
    {
        private readonly AMContext _context;

        public StudentController(AMContext context)
        {
            _context = context;
        }

        // GET: api/Student
        [HttpGet]
        public IEnumerable<Student> GetStudents()
        {
            return _context.Students;
        }

        [HttpGet("DTO")]
        public async Task<IActionResult> GetStudentsDTO()
        {
            IEnumerable<Student> students = GetStudents();
            IEnumerable<SubjectUser> subjectUsers = _context.SubjectsUsers;
            List<StudentDTO> studentsDTOs = new List<StudentDTO>();
            foreach (Student s in students)
            {
                User u = await _context.Users.FindAsync(s.IdUser);
                SubjectUser temp = new SubjectUser();
                foreach(SubjectUser su in subjectUsers){
                    if(su.IdUser == u.Id){
                        temp = su;
                    }
                }
                StudentDTO dto = new StudentDTO();
                dto.Login = u.Login;
                dto.Password = u.Password;
                dto.Name = u.Name;
                dto.Number = s.Number;
                Subject sub = await _context.Subjects.FindAsync(temp.IdSubject);
                dto.Subject = sub.Name;
                studentsDTOs.Add(dto);
            }
            return Ok(studentsDTOs);
        }

        // GET: api/Student/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Student = await _context.Students.FindAsync(id);

            if (Student == null)
            {
                return NotFound();
            }

            return Ok(Student);
        }

        // PUT: api/Student/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent([FromRoute] long id, 
        [FromBody] Student Student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Student.Id)
            {
                return BadRequest();
            }

            _context.Entry(Student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // POST: api/Student
        [HttpPost]
        public async Task<IActionResult> PostStudent([FromBody] Student Student)
        {
            bool guardar = true;
            if (!ModelState.IsValid || Student == null)
            {
                return BadRequest(ModelState);
            }
            
            if(guardar){
                _context.Students.Add(Student);
                await _context.SaveChangesAsync();

                return Ok(Student);
            } else {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Student = await _context.Students.FindAsync(id);
            if (Student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(Student);
            await _context.SaveChangesAsync();

            return Ok(Student);
        }
        private bool StudentExists(long id)
        {
            return _context.Students.Any(e => e.Id == id);
        }

        [HttpGet("GetTests/{login}")]
        public async Task<IActionResult> getTestsFromSubject([FromRoute] string login){
            IEnumerable<Student> students = GetStudents();
            IEnumerable<User> users = _context.Users;
            IEnumerable<Test> tests = _context.Tests;
            IEnumerable<TestQuestion> testQuestions = _context.TestQuestions;
            IEnumerable<Question> questions = _context.Questions;
            IEnumerable<QuestionPossibleAnswer> possibleAnswers = _context.QuestionPossibleAnswers;
            IEnumerable<QuestionCorrectAnswer> correctAnswers = _context.QuestionCorrectAnswers;
            IEnumerable<Answer> answers = _context.Answers;
            IEnumerable<Subject> subjects = _context.Subjects;
            IEnumerable<SubjectUser> subjectUsers = _context.SubjectsUsers;

            User userSelected = new User();
            foreach(User u in users){
                if(u.Login == login){
                    userSelected = u;
                    break;
                }
            }
            Student student = new Student();
            foreach(Student s in _context.Students){
                if(s.IdUser == userSelected.Id){
                    student = s;
                    break;
                }
            }
            SubjectUser suSelected = new SubjectUser();
            foreach(SubjectUser su in subjectUsers){
                if(su.IdUser == userSelected.Id){
                    suSelected = su;
                    break;
                }
            }
            Subject chosenSubject = await _context.Subjects.FindAsync(suSelected.IdSubject);
            List<Test> testsFromSubject = new List<Test>();
            foreach(Test t in tests){
                if(t.IdSubject == chosenSubject.Id){
                    testsFromSubject.Add(t);
                }
            }
            List<TestDTO> testDTOs = new List<TestDTO>();
            foreach(Test t in tests){
                bool solved = false;
                foreach(TestAnswer ta in _context.TestAnswers){
                    if(t.Id == ta.IdTest &&
                    ta.IdStudent == student.Id){
                       solved = true;
                       break;
                    }
                }
                if(!solved){
                    List<TestQuestion> testQuestionsFromSubject = new List<TestQuestion>();
                    foreach(TestQuestion tq in testQuestions){
                        if(tq.IdTest == t.Id){
                            testQuestionsFromSubject.Add(tq);
                        }
                    }
                    TestDTO tDTO = new TestDTO();
                    tDTO.questions = new List<QuestionDTO>();
                    foreach(TestQuestion tq in testQuestionsFromSubject){
                        Question q = await _context.Questions.FindAsync(tq.IdQuestion);
                        QuestionDTO dto = await buildQuestionDTO(q, possibleAnswers, correctAnswers, answers);
                        tDTO.questions.Add(dto);
                    }
                    testDTOs.Add(tDTO);
                }
            }
            return Ok(testDTOs);
        }
        private async Task<QuestionDTO> buildQuestionDTO(Question q, IEnumerable<QuestionPossibleAnswer> possibleAnswers,
        IEnumerable<QuestionCorrectAnswer> correctAnswers, IEnumerable<Answer> answers){
            QuestionDTO dto = new QuestionDTO();
            dto.Title = q.Title;
            Subject s = await _context.Subjects.FindAsync(q.IdSubject); 
            dto.Subject = s.Name;
            dto.PossibleAnswers = new List<AnswerDTO>();
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

        [HttpGet("CheckGrades/{number}")]
        public async Task<IActionResult> checkGrades([FromRoute] int number){
            IEnumerable<Student> students = GetStudents();
            IEnumerable<TestResult> testResults = _context.TestResults;

            List<int> results = new List<int>();
            Student student = new Student();
            long studentId = 0;
            foreach(Student s in students){
                if(s.Number == number){
                    student = s;
                    studentId = s.Id;
                    break;
                }
            }
            foreach(TestResult tr in testResults){
                if(tr.IdStudent == studentId){
                    results.Add(tr.Grade);
                }
            }
            User u = await _context.Users.FindAsync(student.IdUser);
            StudentDTO sDTO = new StudentDTO();
            sDTO.Login = u.Login;
            sDTO.Name = u.Name;
            sDTO.Number = student.Number;
            sDTO.Password = u.Password;
            foreach(SubjectUser su in _context.SubjectsUsers){
                if(su.IdUser == u.Id){
                    Subject s = await _context.Subjects.FindAsync(su.IdSubject);
                    sDTO.Subject = s.Name;
                    break;
                }
            }
            TestResultDTO trDTO = new TestResultDTO();
            trDTO.Results = results;
            trDTO.Student = sDTO;
            return Ok(trDTO);
        }
    }
}
