using System.Collections.Generic;

namespace AgileMethodsTestFramework.Models{
    public class QuestionDTO{
        public string Title{get;set;}
        public List<AnswerDTO> PossibleAnswers{get;set;}
        public AnswerDTO CorrectAnswer{get;set;}
        public string Subject{get;set;}
    }
}