using System.Collections.Generic;

namespace AgileMethodsTestFramework.Models{
    public class TestAnswerDTO{
        public TestDTO Test{get;set;}
        public StudentDTO Student{get;set;}
        public List<AnswerToQuestionDTO> AnswersToQuestions{get;set;}
    }
}