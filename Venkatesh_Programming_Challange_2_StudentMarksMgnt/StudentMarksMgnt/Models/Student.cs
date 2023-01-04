using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentMarksMgnt.Models
{

    public class ListofStudents
    {
        public List<Student> Students { get; set; }
    }

    public class Student
    {
        public string StudentName { get; set; }
        public string StudentID { get; set; }
        public int Subject1 { get; set; }
        public int Subject2 { get; set; }

        public int Subject3 { get; set; }

        public int Subject4 { get; set; }

        public int Subject5 { get; set; }

    }
}