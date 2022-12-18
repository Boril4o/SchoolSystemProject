using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Models.Note;
using SchoolSystem.Core.Models.Student;
using SchoolSystem.Infrastructure.Data;
using SchoolSystem.Infrastructure.Data.Entities;
using System.Linq;
using System.Security.Claims;
using System.Text;
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

        /// <summary>
        /// Get student subjects and grades
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<IEnumerable<StudentGradesViewModel>> GetStudentGrades(ClaimsPrincipal currentUser)
        {
            List<StudentGradesViewModel> model = new List<StudentGradesViewModel>();
            
            //Find current user
            User user = await userManager.GetUserAsync(currentUser);

            //Get current student
            var student = await context
               .Students
               .Where(s => s.UserId == user.Id)
               .Include(s => s.Grades)
               .ThenInclude(g => g.Subject)
               .FirstOrDefaultAsync();

            //We make foreach of subjects and for every subject if subject name is
            //equal to grades.subject.name property then we add the grade. if we have added 15 grades we add new line.
            //And at the end we make StudentGradesViewModel and add it to model list at the beginning of the method
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
                    Grades = sb.ToString().TrimEnd().TrimEnd(',')
                });
            }

            return model;
        }

        public async Task<StudentHomePageStatsViewModel> GetStudentHomePageStats(ClaimsPrincipal currentUser)
        {
            User user = await userManager.GetUserAsync(currentUser);

            var student = await context
                .Students
                .Include(s => s.Grades)
                .ThenInclude(g => g.Subject)
                .Include(s => s.Group)
                .ThenInclude(g => g.Students)
                .ThenInclude(s => s.Grades)
                .Where(s => s.UserId == user.Id)
                .FirstOrDefaultAsync();

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

        /// <summary>
        /// Get current student all notes. Every note has Title, Description, SubjectName, TeacherName, IsPositive and Date properties
        /// if isPostitive is false means the note is bad(doesn't listen in class) otherwise is good
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NoteViewModel>> GetStudentNotes(ClaimsPrincipal currentUser)
        {
            User user = await userManager.GetUserAsync(currentUser);

            Student student = await context
                 .Students
                 .Where(s => s.UserId == user.Id)
                 .Include(s => s.Notes)
                 .ThenInclude(n => n.Subject)
                 .FirstOrDefaultAsync();

            var model = new List<NoteViewModel>();

            foreach (var note in student.Notes)
            {
                model.Add(new NoteViewModel
                {
                    Title = note.Title,
                    Description = note.Description,
                    SubjectName = note.Subject.Name,
                    TeacherName = note.TeacherName,
                    IsPositive = note.IsPostive,
                    Date = note.Date
                });
            }

            return model.OrderByDescending(x => x.Date);
        }
    }
}
