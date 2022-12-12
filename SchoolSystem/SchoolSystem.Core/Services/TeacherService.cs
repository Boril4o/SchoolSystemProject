using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Models.Group;
using SchoolSystem.Core.Models.Student;
using SchoolSystem.Data.Data;
using SchoolSystem.Data.Data.Entities;

namespace SchoolSystem.Core.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDbContext context;

        public TeacherService(ApplicationDbContext context)
        {
            this.context = context;
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

        public async Task<IEnumerable<StudentViewModel>> AllStudents(int groupId)
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
    }
}
