using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Core.Models.Grade
{
    public class AddGradeViewModel
    {
        public int Number { get; set; }

        public int StudentId { get; set; }

        public int SubjectId { get; set; }

        public IEnumerable<SchoolSystem.Data.Data.Entities.Subject> Subjects { get; set; } =
            new List<SchoolSystem.Data.Data.Entities.Subject>();

        public string TeacherUserName { get; set; }
    }
}
