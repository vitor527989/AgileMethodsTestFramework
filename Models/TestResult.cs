using System.Collections.Generic;

namespace AgileMethodsTestFramework.Models{
    public class TestResult{
        public long Id{get;set;}
        public Student Student{get;set;}
        public List<Answer> Answers{get;set;}
        public int Grade{get;set;}
    }
}