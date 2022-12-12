using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Models.Group;
using SchoolSystem.Core.Models.Student;
using SchoolSystem.Core.Models.Subject;
using SchoolSystem.Core.Models.Teacher;
using SchoolSystem.Data.Data;
using SchoolSystem.Data.Data.Entities;
using static SchoolSystem.Core.Constraints.ErrorConstants;

namespace SchoolSystem.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;

        public AdminService(ApplicationDbContext context,
            UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task AddGroupAsync(AddGroupViewModel model)
        {
            if (await IsGroupExistAsync(model.Number))
            {
                throw new ArgumentException(GroupAlreadyExist);
            }

            Group group = new Group
            {
                Number = model.Number,
                MaxPeople = model.MaxPeople
            };

            await context.Groups.AddAsync(group);
            await context.SaveChangesAsync();
        }

        public async Task AddStudentAsync(AddStudentViewModel model)
        {
            if (!await IsUserNameExistAsync(model.UserName))
            {
                throw new ArgumentException(UsernameDoesNotExist);
            }

            if (!await IsGroupExistAsync(model.GroupNumber))
            {
                throw new ArgumentException(GroupDoesNotExist);
            }

            Group group = await context.Groups
                 .FirstOrDefaultAsync(g => g.Number == model.GroupNumber);

            User user = await context
                .Users
                .FirstOrDefaultAsync(u => u.UserName == model.UserName);

            var result = await userManager.AddToRoleAsync(user, "Student");

            Student student = new Student
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
            if (await IsSubjectExistAsync(model.Name))
            {
                throw new ArgumentException(SubjectAlreadyExist);
            }

            Subject subject = new Subject
            {
                Name = model.Name
            };

            await context.Subjects.AddAsync(subject);
            await context.SaveChangesAsync();
        }

        public async Task AddTeacherAsync(AddTeacherViewModel model)
        {
            if (!await IsUserNameExistAsync(model.UserName))
            {
                throw new ArgumentException(UsernameDoesNotExist);
            }

            if (!await IsSubjectExistAsync(model.SubjectId))
            {
                throw new ArgumentException(SubjectDoesNotExist);
            }

            if (model.GroupNumber != null &&
                !await IsGroupExistAsync(model.GroupNumber))
            {
                throw new ArgumentException(GroupDoesNotExist);
            }

            User user = await context
                .Users
                .FirstOrDefaultAsync(u => u.UserName == model.UserName);

            Group group = await context.Groups
                 .FirstOrDefaultAsync(g => g.Number == model.GroupNumber);

            var result = await userManager.AddToRoleAsync(user, "Teacher");

            Teacher teacher;

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
         => await context
            .Students
            .Select(s => new StudentViewModel
            {
                FirstName = s.User.FirstName,
                LastName = s.User.LastName,
                Group = s.Group.Number,
                Id = s.Id,
                UserName = s.User.UserName
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

            var notes = context
                .Notes
                .Where(n => n.TeacherId == t.Id)
                .Select(n => new Note
                {
                    Id = n.Id,
                    Title = n.Title,
                    Description = n.Description,
                    StudentId = n.StudentId,
                    SubjectId = n.SubjectId,
                    TeacherId = null,
                });

            var grades = context
                .Grades
                .Where(g => g.TeacherId == t.Id)
                .Select(g => new Grade
                {
                    Id = g.Id,
                    Number = g.Number,
                    StudentId = g.StudentId,
                    SubjectId = g.StudentId,
                    TeacherId = null,
                });

            context.Teachers.Remove(t);
            await context.SaveChangesAsync();
        }

        public async Task EditGroup(EditGroupViewModel model)
        {
            Group g = await context.Groups.FindAsync(model.Id);
            if (g == null)
            {
                throw new ArgumentException(GroupDoesNotExist);
            }

            if (model.MaxPeople < await context.Students.CountAsync(s => s.GroupId == model.Id))
            {
                throw new ArgumentException(MaxPeopleLowerThanCurrentPeople);
            }

            g.Number = model.Number;
            g.MaxPeople = model.MaxPeople;
            await context.SaveChangesAsync();
        }

        public async Task EditStudent(EditStudentViewModel model)
        {
            if (await IsUserNameExistAsync(model.UserName))
            {
                throw new ArgumentException(UserNameExist);
            }

            Student s = await GetStudent(model.Id);
            if (s == null)
            {
                throw new ArgumentException(UserDoesNotExist);
            }

            s.User.FirstName = model.FirstName;
            s.User.LastName = model.LastName;
            s.User.UserName = model.UserName;
            s.GroupId = model.GroupID;

            await context.SaveChangesAsync();
        }

        public async Task EditSubject(EditSubjectViewModel model)
        {
            Subject s = await context
                .Subjects
                .Where(s => s.Name == model.Name)
                .FirstAsync();

            if (s == null)
            {
                throw new ArgumentException(SubjectDoesNotExist);
            }

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

        public async Task<IEnumerable<Group>> GetGroups()
            => await context.Groups.ToArrayAsync();

        public async Task<Student> GetStudent(int id)
          => await context.Students.Include(s => s.User).Where(s => s.Id == id).FirstOrDefaultAsync();

        public async Task<Subject> GetSubject(int id)
            => await context.Subjects.FindAsync(id);

        public async Task<IEnumerable<Subject>> GetSubjects()
         => await context.Subjects.ToListAsync<Subject>();

        public async Task<Teacher> GetTeacher(int id)
        => await context.Teachers.Include(t => t.User).Where(t => t.Id == id).FirstAsync();

        public async Task<bool> IsGroupExistAsync(string number)
        => await context.Groups.AnyAsync(g => g.Number == number);

        public async Task<bool> IsSubjectExistAsync(string subjectName)
        => await context.Subjects.AnyAsync(s => s.Name == subjectName);

        public async Task<bool> IsSubjectExistAsync(int id)
         => await context.Subjects.FindAsync(id) != null;

        public async Task<bool> IsUserNameExistAsync(string username)
        => await context.Users.AnyAsync(u => u.UserName == username);

    }
}
