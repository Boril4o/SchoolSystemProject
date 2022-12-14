using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Models.Student;
using SchoolSystem.Data.Data;
using SchoolSystem.Data.Data.Entities;
using System.Linq;
using System.Security.Claims;
using System.Text;
using static SchoolSystem.Core.Constraints.ErrorConstants;
namespace SchoolSystem.Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;

        public StudentService(ApplicationDbContext context,
            UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<StudentGradesViewModel>> GetStudentGrades(ClaimsPrincipal currentUser)
        {
            List<StudentGradesViewModel> model = new List<StudentGradesViewModel>();

            User user = await userManager.GetUserAsync(currentUser);

            if (user == null)
            {
                throw new ArgumentException(UserDoesNotExist);
            }

            var student = await context
               .Students
               .Where(s => s.UserId == user.Id)
               .Include(s => s.Grades)
               .ThenInclude(g => g.Subject)
               .FirstOrDefaultAsync();

            if (student == null)
            {
                throw new ArgumentException(StudentDoesNotExist);
            }

            foreach (var s in context.Subjects)
            {
                StringBuilder sb = new StringBuilder();
                int count = 1;
                foreach (var g in student.Grades)
                {
                    if (g.Subject.Name == s.Name)
                    {
                        sb.Append($"{g.Number}, ");
                        count++;
                    }

                    if (count == 3)
                    {
                        sb.AppendLine();
                        count = 0;
                    }
                }

                model.Add(new StudentGradesViewModel
                {
                    Subject = s.Name,
                    Grades = sb.ToString().Trim()
                });
            }

            return model;
        }

        public async Task<StudentHomePageStatsViewModel> GetStudentHomePageStats(ClaimsPrincipal currentUser)
        {
            User user = await userManager.GetUserAsync(currentUser);

            if (user == null)
            {
                throw new ArgumentException(UserDoesNotExist);
            }

            var student = await context
                .Students
                .Include(s => s.Grades)
                .ThenInclude(g => g.Subject)
                .Include(s => s.Group)
                .ThenInclude(g => g.Students)
                .ThenInclude(s => s.Grades)
                .Where(s => s.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (student == null)
            {
                throw new ArgumentException(StudentDoesNotExist);
            }

            double grade;
            grade = student.AverageGrade;

            var group = student
                .Group
                .Students
                .OrderByDescending(s => s.AverageGrade)
                .ToArray();

            string placeInClass = "1";
            int place = 0;
            if (group.Count() > 0)
            {
                foreach (var s in group)
                {
                    place++;
                    if (s.UserId == user.Id)
                    {
                        placeInClass = place.ToString();
                        break;
                    }
                }
            }

            string bestSubjectName = "none";
            double bestGrade = 0;
            var grades = student.Grades;
            if (grades.Count > 0)
            {
                foreach (var g in grades)
                {
                    string currentSubjectName = g.Subject.Name;
                    double currentSubjectGrade = grades.Where(g => g.Subject.Name == currentSubjectName).Average(g => g.Number);
                    if (currentSubjectName != bestSubjectName &&
                       currentSubjectGrade > bestGrade)
                    {
                        bestSubjectName = currentSubjectName;
                        bestGrade = currentSubjectGrade;
                    }
                }
            }

            var model = new StudentHomePageStatsViewModel
            {
                Grade = $"{grade:f2}",
                GradesCount = grades.Count.ToString() + " Grades",
                PlaceInClass = placeInClass + " place in class",
                BestSubjectName = $"Best Subject: {bestSubjectName}"
            };

            return model;
        }

       
    }
}
