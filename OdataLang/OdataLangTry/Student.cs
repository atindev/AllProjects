using System;

namespace OdataLangTry
{
    public class Student
    {
        //[DataAnnotation("Atin")]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
    }
}
