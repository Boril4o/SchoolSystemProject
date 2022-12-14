using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Core.Models.Student
{
    public class StudentGradesViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public double AverageGrade { get; set; }

        public string SubjectName { get; set; }
    }
}
