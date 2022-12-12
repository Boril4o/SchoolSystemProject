using SchoolSystem.Core.Models.Group;
using SchoolSystem.Core.Models.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Core.Contracts
{
    public interface ITeacherService
    {
        public Task<IEnumerable<GroupViewModel>> AllGroups();

        public Task<IEnumerable<StudentViewModel>> AllStudents(int groupId);
    }
}
