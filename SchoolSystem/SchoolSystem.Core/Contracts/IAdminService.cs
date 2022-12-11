using SchoolSystem.Core.Models.Group;
using SchoolSystem.Core.Models.Student;
using SchoolSystem.Core.Models.Subject;
using SchoolSystem.Core.Models.Teacher;

namespace SchoolSystem.Core.Contracts
{
    public interface IAdminService
    {
        public Task AddStudentAsync(AddStudentViewModel model);

        public Task<bool> IsGroupExistAsync(string number);

        public Task<bool> IsUserNameExistAsync(string username);

        public Task AddTeacherAsync(AddTeacherViewModel model);

        public Task<bool> IsSubjectExistAsync(string subjectName);

        public Task AddGroupAsync(AddGroupViewModel model);

        public Task AddSubjectAsync(AddSubjectViewModel model);
    }
}
