using System.Collections.Generic;

namespace AgileMethodsTestFramework.Models{
    public class Test{
        public Subject Subject{get;set;}
        public List<TestResult> Results{get;set;}
        public Teacher Teacher{get;set;}
        public List<Question> Questions{get;set;}
    }
}