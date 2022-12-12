using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Core.Models.Student
{
    public class StudentStatsViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<SchoolSystem.Data.Data.Entities.Grade> Grades { get; set; } = 
            new List<SchoolSystem.Data.Data.Entities.Grade>();

        public IEnumerable<SchoolSystem.Data.Data.Entities.Note> Notes { get; set; } =
            new List<SchoolSystem.Data.Data.Entities.Note>();

        //public int Id { get; set; }
    }
}
