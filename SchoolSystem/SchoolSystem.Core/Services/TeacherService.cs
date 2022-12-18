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

        /// <summary>
        /// Add grade to student
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task AddGrade(AddGradeViewModel model, ClaimsPrincipal currentUser)
        {
            User user = await userManager.GetUserAsync(currentUser);

            //Finds the current teacher
            Teacher teacher = await context.Teachers.FirstOrDefaultAsync(t => t.UserId == user.Id);

            //Add grade to db and save
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

        /// <summary>
        /// Add note to student
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task AddNote(AddNoteViewModel model, ClaimsPrincipal currentUser)
        {
            //Finds current teacher
            User user = await userManager.GetUserAsync(currentUser);

            Teacher teacher = await context.Teachers.FirstOrDefaultAsync(t => t.UserId == user.Id);

            // Add note to db and save
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

        /// <summary>
        /// Returns all groups and how many people have in each group
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<GroupViewModel>> AllGroups()
         => await context.Groups.Select(g => new GroupViewModel
        {
           MaxPeople = g.MaxPeople,
           Number = g.Number,
           People = context.Students.Count(s => s.GroupId == g.Id),
           Id = g.Id
        })
            .ToListAsync();

        /// <summary>
        /// Get All Students from group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get all subjects from Db
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Subject>> GetSubjects()
         => await context.Subjects.ToListAsync();
    }
}
