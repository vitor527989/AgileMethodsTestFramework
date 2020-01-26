using System.Collections.Generic;

namespace AgileMethodsTestFramework.Models{
    public class TestDTO{
        public long Id{get;set;}
        public List<QuestionDTO> questions{get;set;}
    }
}