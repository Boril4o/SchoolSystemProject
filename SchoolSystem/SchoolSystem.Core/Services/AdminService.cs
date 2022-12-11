using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Models.Group;
using SchoolSystem.Core.Models.Student;
using SchoolSystem.Core.Models.Subject;
using SchoolSystem.Core.Models.Teacher;
using SchoolSystem.Data.Data;
using SchoolSystem.Data.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Subject subject = new Subject
            {
                Name = model.Name
            };
        
            await context.Subjects.AddAsync(subject);
            await context.SaveChangesAsync();
        }

        public async Task AddTeacherAsync(AddTeacherViewModel model)
        {
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

        public async Task<IEnumerable<Group>> GetGroups()
            => await context.Groups.ToArrayAsync();

        public async Task<IEnumerable<Subject>> GetSubjects()
         => await context.Subjects.ToListAsync<Subject>();

        public async Task<Teacher> GetTeacher(int id)
        => await context.Teachers.Include(t => t.User).Where(t => t.Id == id).FirstAsync();

        public async Task<bool> IsGroupExistAsync(string number)
        => await context.Groups.AnyAsync(g => g.Number == number);

        public async Task<bool> IsSubjectExistAsync(string subjectName)
        => await context.Subjects.AnyAsync(s => s.Name == subjectName);

        public async Task<bool> IsSubjectExistAsync(int id)
         => await context.Subjects.FindAsync(id) == null;

        public async Task<bool> IsUserNameExistAsync(string username)
        => await context.Users.AnyAsync(u => u.UserName == username);

        
    }
}
