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

        [HttpGet("DTO")]
        public async Task<IActionResult> GetQuestionsDTO()
        {
            IEnumerable<Question> questions = GetQuestions();
            IEnumerable<QuestionPossibleAnswer> possibleAnswers = _context.QuestionPossibleAnswers;
            IEnumerable<QuestionCorrectAnswer> correctAnswers = _context.QuestionCorrectAnswers;
            IEnumerable<Answer> answers = _context.Answers;
            List<QuestionDTO> questionsDTOs = new List<QuestionDTO>();
            foreach (Question q in questions)
            {
                QuestionDTO qDto = await buildQuestionDTO(q, possibleAnswers, correctAnswers,
                answers);
                questionsDTOs.Add(qDto);
            }
            return Ok(questionsDTOs);
        }

        private async Task<QuestionDTO> buildQuestionDTO(Question q, IEnumerable<QuestionPossibleAnswer> possibleAnswers,
        IEnumerable<QuestionCorrectAnswer> correctAnswers, IEnumerable<Answer> answers){
            QuestionDTO dto = new QuestionDTO();
            dto.Title = q.Title;
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
        public async Task<IActionResult> PostQuestion([FromBody] QuestionDTO question)
        {
            if (!ModelState.IsValid || question == null || question.Title == null)
            {
                return BadRequest(ModelState);
            }
            IEnumerable<Question> questions = GetQuestions();
            IEnumerable<Answer> answers = _context.Answers;
            IEnumerable<Subject> subjects = _context.Subjects;
            Question toAdd = new Question();
            if(questions.Count() > 0){
                foreach(Question q in questions){
                    if(q.Title == question.Title){
                        return BadRequest("Question Exists");
                    }
                }
            }
            toAdd.Title = question.Title;
            bool subjectExists = false;
            if(subjects.Count() > 0){
                foreach(Subject s in subjects){
                    if(s.Name == question.Subject){
                        toAdd.IdSubject = s.Id;
                        subjectExists = true;
                    }
                }
            }
            if(!subjectExists){
                Subject s = new Subject();
                s.Name = question.Subject;
                _context.Subjects.Add(s);
            }
            await _context.SaveChangesAsync();
            IEnumerable<Subject> subjectsUpdated = _context.Subjects;
            foreach(Subject s in subjects){
                if(s.Name == question.Subject){
                    toAdd.IdSubject = s.Id;
                }
            }
            _context.Questions.Add(toAdd);
            List<Answer> added = new List<Answer>();
            foreach(AnswerDTO a in question.PossibleAnswers){
                Answer aToAdd = new Answer();
                aToAdd.Description = a.Description;
                added.Add(aToAdd);
                _context.Answers.Add(aToAdd);
            }
            await _context.SaveChangesAsync();
            handleRelations(question, added);
            return Ok(question);
        }

        private async void handleRelations(QuestionDTO question, List<Answer> added){
            IEnumerable<Question> questions = GetQuestions();
            IEnumerable<QuestionPossibleAnswer> possibleAnswers = _context.QuestionPossibleAnswers;
            IEnumerable<QuestionCorrectAnswer> correctAnswers = _context.QuestionCorrectAnswers;
            IEnumerable<Answer> answers = _context.Answers;
            IEnumerable<Subject> subjects = _context.Subjects;
            Question questionAdded = new Question();
            Subject subjectAdded = new Subject();
            List<Answer> answersAdded = new List<Answer>();
            Answer correctAnswerAdded = new Answer();
            
            foreach(Question q in questions){
                if(q.Title == question.Title){
                    questionAdded = q;
                }
            }
            foreach(Subject s in subjects){
                if(s.Name == question.Subject){
                    subjectAdded = s;
                }
            }
            foreach(Answer a in answers){
                if(a.Description == question.CorrectAnswer.Description){
                    correctAnswerAdded = a;
                }
            }
            foreach(Answer a in added){
                QuestionPossibleAnswer qpa = new QuestionPossibleAnswer();
                qpa.IdPossibleAnswer = a.Id;
                qpa.IdQuestion = questionAdded.Id;
                _context.QuestionPossibleAnswers.Add(qpa);
            }
            QuestionCorrectAnswer qca = new QuestionCorrectAnswer();
            qca.IdCorrectAnswer = correctAnswerAdded.Id;
            qca.IdQuestion = questionAdded.Id;
            _context.QuestionCorrectAnswers.Add(qca);
            await _context.SaveChangesAsync();
        }

        // DELETE: api/Question/titleofquestion
        [HttpDelete("{title}")]
        public async Task<IActionResult> DeleteQuestion([FromRoute] string title)
        {
            if (!ModelState.IsValid || title == null)
            {
                return BadRequest(ModelState);
            }
            IEnumerable<Question> questions = GetQuestions();
            IEnumerable<QuestionPossibleAnswer> possibleAnswers = _context.QuestionPossibleAnswers;
            IEnumerable<QuestionCorrectAnswer> correctAnswers = _context.QuestionCorrectAnswers;
            IEnumerable<Answer> answers = _context.Answers;

            List<long> idAnswersToDelete = new List<long>();
            Question toDelete = new Question();

            foreach(Question q in questions){
                if(q.Title == title){
                    toDelete = q;
                }
            }
            foreach(QuestionPossibleAnswer qpa in possibleAnswers){
                if(qpa.IdQuestion == toDelete.Id){
                    idAnswersToDelete.Add(qpa.IdPossibleAnswer);
                    _context.QuestionPossibleAnswers.Remove(qpa);
                }
            }
            foreach(long id in idAnswersToDelete){
                Answer a = await _context.Answers.FindAsync(id);
                _context.Answers.Remove(a);
            }
            foreach(QuestionCorrectAnswer qca in correctAnswers){
                if(qca.IdQuestion == toDelete.Id){
                    _context.QuestionCorrectAnswers.Remove(qca);
                }
            }
            _context.Questions.Remove(toDelete);
            await _context.SaveChangesAsync();

            return Ok(toDelete);
        }
        private bool QuestionExists(long id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}
