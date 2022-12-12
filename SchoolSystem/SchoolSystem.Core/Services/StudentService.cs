using Microsoft.EntityFrameworkCore;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Models.Student;
using SchoolSystem.Data.Data;
using SchoolSystem.Data.Data.Entities;
using static SchoolSystem.Core.Constraints.ErrorConstants;
namespace SchoolSystem.Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext context;

        public StudentService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<StudentStatsViewModel> StudentStats(string username)
        {
            User u = await context.Users.Where(u => u.UserName == username).FirstOrDefaultAsync();

            if (u == null)
            {
                throw new ArgumentException(UserDoesNotExist);
            }

            Student s = await context.Students.Where(s => s.UserId == u.Id).FirstOrDefaultAsync();

            if (s == null)
            {
                throw new ArgumentException(StudentDoesNotExist);
            }

            StudentStatsViewModel model = new StudentStatsViewModel
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Grades = s.Grades,
                Notes = s.Notes
            };

            return model;
        }
    }
}
