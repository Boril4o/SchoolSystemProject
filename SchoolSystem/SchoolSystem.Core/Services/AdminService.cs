using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Models.Group;
using SchoolSystem.Core.Models.Student;
using SchoolSystem.Core.Models.Subject;
using SchoolSystem.Core.Models.Teacher;
using SchoolSystem.Infrastructure.Data;
using SchoolSystem.Infrastructure.Data.Entities;

namespace SchoolSystem.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;
        private readonly string studentRole = "Student";
        private readonly string teacherRole = "Teacher";

        public AdminService(ApplicationDbContext context,
            UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task AddGroupAsync(AddGroupViewModel model)
        {
            Group group = new()
            {
                Number = model.Number,
                MaxPeople = model.MaxPeople
            };

            await context.Groups.AddAsync(group);
            await context.SaveChangesAsync();
        }

        public async Task AddStudentAsync(AddStudentViewModel model)
        {
            Group group = await context.Groups
                 .FirstOrDefaultAsync(g => g.Id == model.GroupId);

            User user = await context
                .Users
                .FirstOrDefaultAsync(u => u.UserName == model.UserName);

            var result = await userManager.AddToRoleAsync(user, studentRole);

            Student student = new()
            {
                UserId = user.Id,
                User = user,
                GroupId = group.Id,
                Group = group,
            };

            await context.AddAsync(student);
            await context.SaveChangesAsync();
        }

        public async Task AddSubjectAsync(AddSubjectViewModel model)
        {
            Subject subject = new ()
            {
                Name = model.Name
            };

            await context.Subjects.AddAsync(subject);
            await context.SaveChangesAsync();
        }

        public async Task AddTeacherAsync(AddTeacherViewModel model)
        {
            Group group = await context.Groups
                .FirstOrDefaultAsync(g => g.Id == model.GroupId);

            User user = await context
                .Users
                .FirstOrDefaultAsync(u => u.UserName == model.UserName);

            var result = await userManager.AddToRoleAsync(user, "Teacher");

            Teacher teacher = new();

            if (group == null)
            {
                teacher = new Teacher
                {
                    User = user,
                    UserId = user.Id,
                    Salary = model.Salary,
                    SubjectId = model.SubjectId
                };
            }
            else
            {
                teacher = new Teacher
                {
                    User = user,
                    UserId = user.Id,
                    Salary = model.Salary,
                    Group = group,
                    GroupID = group.Id,
                    SubjectId = model.SubjectId
                };
            }

            await context.AddAsync(teacher);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<GroupViewModel>> AllGroups()
            => await context.Groups.Select(g => new GroupViewModel
            {
                MaxPeople = g.MaxPeople,
                Number = g.Number,
                People = context.Students.Count(s => s.GroupId == g.Id),
                Id = g.Id
            })
            .ToListAsync();

        public async Task<IEnumerable<StudentViewModel>> AllStudents()
         =>  await context
            .Students
            .Select(s => new StudentViewModel
            {
                UserName = s.User.UserName,
                FirstName = s.User.FirstName,
                LastName = s.User.LastName,
                Group = s.Group.Number,
                Id = s.Id,
            })
            .ToListAsync();

        public async Task<IEnumerable<SubjectViewModel>> AllSubjects()
            => await context
            .Subjects
            .Select(s => new SubjectViewModel
            {
                Id = s.Id,
                Name = s.Name
            })
            .ToListAsync();

        public async Task<IEnumerable<TeacherViewModel>> AllTeachers()
            => await context
            .Teachers
            .Select(t => new TeacherViewModel
            {
                FirstName = t.User.FirstName,
                LastName = t.User.LastName,
                UserName = t.User.UserName,
                Group = t.Group == null ? "none" : t.Group.Number,
                Subject = t.Subject.Name,
                Salary = t.Salary,
                Id = t.Id,
            })
            .ToArrayAsync();

        public async Task DeleteGroup(int id)
        {
            Group g = await GetGroup(id);

            context.Groups.Remove(g);
            await context.SaveChangesAsync();
        }

        public async Task DeleteStudent(int id)
        {
            Student s = await GetStudent(id);

            context.Students.Remove(s);
            await context.SaveChangesAsync();
        }

        public async Task DeleteSubject(int id)
        {
            Subject s = await GetSubject(id);

            context.Subjects.Remove(s);
            await context.SaveChangesAsync();
        }

        public async Task DeleteTeacherAsync(int id)
        {
            Teacher t = await GetTeacher(id);

            await userManager.RemoveFromRoleAsync(context.Users.Find(t.UserId), teacherRole);

            context.Teachers.Remove(t);
            await context.SaveChangesAsync();
        }

        public async Task EditGroup(EditGroupViewModel model)
        {
            Group g = await context.Groups.FindAsync(model.Id);

            g.Number = model.Number;
            g.MaxPeople = model.MaxPeople;
            await context.SaveChangesAsync();
        }

        public async Task EditStudent(EditStudentViewModel model)
        {
            Student s = await GetStudent(model.Id);

            s.User.FirstName = model.FirstName;
            s.User.LastName = model.LastName;
            s.User.UserName = model.UserName;
            s.GroupId = model.GroupID;

            await context.SaveChangesAsync();
        }

        public async Task EditSubject(EditSubjectViewModel model)
        {
            Subject s = await GetSubject(model.Id);

            s.Name = model.Name;
            await context.SaveChangesAsync();
        }

        public async Task EditTeacherAsync(int id, EditTeacherViewModel model)
        {
            Teacher teacher = await GetTeacher(id);

            teacher.User.UserName = model.UserName;
            teacher.User.FirstName = model.FirstName;
            teacher.User.LastName = model.LastName;
            teacher.GroupID = model.GroupId;
            teacher.SubjectId = model.SubjectId;
            teacher.Salary = model.Salary;

            await context.SaveChangesAsync();
        }

        public async Task<Group> GetGroup(int id)
            => await context.Groups.FindAsync(id);

        public async Task<Group> GetGroup(string number)
        {
            return await context.Groups.FirstOrDefaultAsync(g => g.Number == number);
        }

        public async Task<IEnumerable<Group>> GetGroups()
            => await context.Groups.ToArrayAsync();

        public async Task<Student> GetStudent(int id)
          => await context.Students.Include(s => s.User).Where(s => s.Id == id).FirstOrDefaultAsync();

        public async Task<int> GetStudentsCountFromGroup(int GroupId)
         => await context.Students.Where(s => s.GroupId == GroupId).CountAsync();

        public async Task<Subject> GetSubject(int id)
            => await context.Subjects.FindAsync(id);

        public async Task<Subject> GetSubject(string name)
        {
            return await context.Subjects.FirstOrDefaultAsync(s => s.Name == name);
        }

        public async Task<IEnumerable<Subject>> GetSubjects()
         => await context.Subjects.ToListAsync<Subject>();

        public async Task<Teacher> GetTeacher(int id)
        => await context.Teachers.Include(t => t.User).Where(t => t.Id == id).FirstAsync();

        public async Task<bool> IsGroupExistAsync(string number)
        => await context.Groups.AnyAsync(g => g.Number == number);

        public async Task<bool> IsStudentUserNameExistAsync(string username, int studentID)
        {
            Student student = await context.Students.FirstOrDefaultAsync(s => s.User.UserName == username);

            if (student == null)
            {
                return false;
            }

            return student.Id != studentID;
        }

        public async Task<bool> IsSubjectExistAsync(string subjectName)
        => await context.Subjects.AnyAsync(s => s.Name == subjectName);

        public async Task<bool> IsSubjectExistAsync(int id)
         => await context.Subjects.FindAsync(id) != null;

        public async Task<bool> IsTeacherUserNameExistAsync(string username, int teacherId)
        {
            Teacher teacher = await context.Teachers.FirstOrDefaultAsync(t => t.User.UserName == username);

            if (teacher == null)
            {
                return false;
            }

            return teacher.Id != teacherId;
        }

        public async Task<bool> IsUserNameExistAsync(string username)
        => await context.Users.AnyAsync(u => u.UserName == username);

    }
}
