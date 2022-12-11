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

            var result = await userManager.AddToRoleAsync(user, "Teacher");

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

            Subject subject = await context
                .Subjects
                .FirstOrDefaultAsync(s => s.Name == model.SubjectName);

            var result = await userManager.AddToRoleAsync(user, "Student");

            Teacher teacher;

            if (group == null)
            {
                teacher = new Teacher
                {
                    User = user,
                    UserId = user.Id,
                    Salary = model.Salary,
                    Subject = subject,
                    SubjectId = subject.Id
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
                    Subject = subject,
                    SubjectId = subject.Id
                };
            }
         
            await context.AddAsync(teacher);
            await context.SaveChangesAsync();
        }

        public async Task<bool> IsGroupExistAsync(string number)
        => await context.Groups.AnyAsync(g => g.Number == number);

        public async Task<bool> IsSubjectExistAsync(string subjectName)
        => await context.Subjects.AnyAsync(s => s.Name == subjectName);

        public async Task<bool> IsUserNameExistAsync(string username)
        => await context.Users.AnyAsync(u => u.UserName == username);
    }
}
