using SchoolSystem.Core.Models.Grade;
using SchoolSystem.Core.Models.Group;
using SchoolSystem.Core.Models.Note;
using SchoolSystem.Core.Models.Student;
using SchoolSystem.Data.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Core.Contracts
{
    public interface ITeacherService
    {
        public Task<IEnumerable<GroupViewModel>> AllGroups();

        public Task<IEnumerable<StudentViewModel>> AllStudentsFromGroup(int groupId);

        public Task AddGrade(AddGradeViewModel model, ClaimsPrincipal currentUser);

        public Task<IEnumerable<Subject>> GetSubjects();

        public Task AddNote(AddNoteViewModel model, ClaimsPrincipal currentUser);
    }
}
