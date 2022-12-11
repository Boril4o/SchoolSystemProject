using SchoolSystem.Core.Models.Student;
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
    }
}
