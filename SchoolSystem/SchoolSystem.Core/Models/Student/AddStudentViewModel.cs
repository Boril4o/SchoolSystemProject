using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Core.Models.Student
{
    public class AddStudentViewModel
    {
        public string UserName { get; set; }

        public int GroupId { get; set; }

        public IEnumerable<Infrastructure.Data.Entities.Group>? Groups { get; set; }
    }
}
