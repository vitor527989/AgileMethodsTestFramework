using System.Collections.Generic;

namespace AgileMethodsTestFramework.Models{
    public class Question{
        public string Title{get;set;}
        public List<Answer> PossibleAnswers{get;set;}
        public List<Answer> CorrectAnswer{get;set;}
        public int value{get;set;}
    }
}