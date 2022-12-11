using System.ComponentModel.DataAnnotations;
using static SchoolSystem.Data.Data.Constants.DataConstants;

namespace SchoolSystem.Core.Models.Teacher
{
    public class AddTeacherViewModel
    {
        public string UserName { get; set; }

        public string? GroupNumber { get; set; }

        [Range(typeof(decimal), TeacherMinSalary, TeacherMaxSalary)]
        public decimal Salary { get; set; }

        public string SubjectName { get; set; }
    }
}
