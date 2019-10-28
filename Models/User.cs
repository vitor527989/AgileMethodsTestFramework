using System.Collections.Generic;

namespace AgileMethodsTestFramework.Models{
    public class User{
        public string Name{get;set;}
        public string Login{get;set;}
        public string Password{get;set;}
        public List<Subject> subjects{get;set;}
    }
}