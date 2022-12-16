using Microsoft.EntityFrameworkCore;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Models.Grade;
using SchoolSystem.Core.Models.Group;
using SchoolSystem.Core.Models.Student;
using SchoolSystem.Core.Models.Note;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using SchoolSystem.Infrastructure.Data.Entities;
using SchoolSystem.Infrastructure.Data;

namespace SchoolSystem.Core.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;

        public TeacherService(ApplicationDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task AddGrade(AddGradeViewModel model, ClaimsPrincipal currentUser)
        {
            User user = await userManager.GetUserAsync(currentUser);

            Teacher teacher = await context.Teachers.FirstOrDefaultAsync(t => t.UserId == user.Id);
            
            Grade grade = new Grade()
            {
                Number = model.Number,
                StudentId = model.StudentId,
                SubjectId = model.SubjectId,
                TeacherId = teacher.Id,
                TeacherName = user.FirstName + " " + user.LastName
            };

            await context.Grades.AddAsync(grade);
            await context.SaveChangesAsync();
        }

        public async Task AddNote(AddNoteViewModel model, ClaimsPrincipal currentUser)
        {
            User user = await userManager.GetUserAsync(currentUser);

            Teacher teacher = await context.Teachers.FirstOrDefaultAsync(t => t.UserId == user.Id);

            Note note = new Note
            {
                Title = model.Title,
                Description = model.Description,
                TeacherId = teacher.Id,
                StudentId = model.StudentId,
                SubjectId = model.SubjectId,
                TeacherName = user.FirstName + " " + user.LastName,
                Date = DateTime.UtcNow,
                IsPostive = model.IsPositive
            };

            await context.Notes.AddAsync(note);
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

        public async Task<IEnumerable<StudentViewModel>> AllStudentsFromGroup(int groupId)
            => await context
            .Students
            .Where(s => s.GroupId == groupId)
            .Select(s => new StudentViewModel
            {
                Id = s.Id,
                FirstName = s.User.FirstName,
                LastName = s.User.LastName,
                Group = s.Group.Number,
            })
            .ToListAsync();

        public async Task<IEnumerable<Subject>> GetSubjects()
         => await context.Subjects.ToListAsync();
    }
}
