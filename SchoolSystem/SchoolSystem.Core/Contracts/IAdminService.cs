using SchoolSystem.Core.Models.Group;
using SchoolSystem.Core.Models.Student;
using SchoolSystem.Core.Models.Subject;
using SchoolSystem.Core.Models.Teacher;
using SchoolSystem.Data.Data.Entities;

namespace SchoolSystem.Core.Contracts
{
    public interface IAdminService
    {
        public Task AddStudentAsync(AddStudentViewModel model);

        public Task<bool> IsGroupExistAsync(string number);

        public Task<bool> IsUserNameExistAsync(string username);

        public Task AddTeacherAsync(AddTeacherViewModel model);

        public Task<bool> IsSubjectExistAsync(string subjectName);

        public Task<bool> IsSubjectExistAsync(int id);

        public Task AddGroupAsync(AddGroupViewModel model);

        public Task AddSubjectAsync(AddSubjectViewModel model);

        public Task<IEnumerable<TeacherViewModel>> AllTeachers();

        public Task<IEnumerable<Subject>> GetSubjects();

        public Task EditTeacherAsync(int id, EditTeacherViewModel model);

        public Task<IEnumerable<Group>> GetGroups();

        public Task<Teacher> GetTeacher(int id);

        public Task DeleteTeacherAsync(int id);
    }
}
