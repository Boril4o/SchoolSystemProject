using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Services;
using SchoolSystem.Infrastructure.Data;
using SchoolSystem.Infrastructure.Data.Entities;
using System.Security.Claims;

namespace SchoolSystem.UnitTests
{
    [TestFixture]
    public class StudentServiceTests
    {
        private ApplicationDbContext context;
        private UserManager<User> userManager;
        private IStudentService studentService;
        private User user;

        [SetUp]
        public void SetUp()
        {
            user = new User
            {
                Id = "8f537a79-857e-41f9-a9c5-916fb3784ff2",
                Age = 15,
                Email = "Boris@gmail.com",
                FirstName = "Boris",
                LastName = "Goshev",
                UserName = "Boris",
                PasswordHash = "1234"
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Db")
                .Options;

            this.context = new ApplicationDbContext(options);
            this.context.Database.EnsureDeleted();
            this.context.Database.EnsureCreated();
            this.context.Users.Add(user);

            var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(),
               null, null, null, null, null, null, null, null);

            userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            userManager = userManagerMock.Object;
            studentService = new StudentService(context, userManager);
        }

        [Test]
        public async Task Get_Student_Grades()
        {
            Student student = new Student
            {
                GroupId = 1,
                UserId = user.Id,
                Id = 1
            };

            var subject = new Subject
            {
                Name = "Math",
                Id = 1
            };

            var subject1 = new Subject
            {
                Name = "Art",
                Id = 2
            };

            var grade = new Grade
            {
                Number = 4,
                StudentId = student.Id,
                SubjectId = subject.Id,
                TeacherId = 1,
                TeacherName = "Koleto"
            };

            var grade1 = new Grade
            {
                Number = 5,
                StudentId = student.Id,
                SubjectId = subject.Id,
                TeacherId = 1,
                TeacherName = "Koleto"
            };

            var grade2 = new Grade
            {
                Number = 6,
                StudentId = student.Id,
                SubjectId = subject1.Id,
                TeacherId = 1,
                TeacherName = "Koleto"
            };

            await context.Students.AddAsync(student);
            await context.Subjects.AddRangeAsync(subject, subject1);
            await context.Grades.AddRangeAsync(grade, grade1, grade2);
            await context.SaveChangesAsync();

            var gradesModels = await studentService.GetStudentGrades(new ClaimsPrincipal());
            var mathGrades = gradesModels.Where(x => x.Subject == subject.Name).FirstOrDefault();
            var artGrades = gradesModels.Where(x => x.Subject != subject.Name).FirstOrDefault();

            Assert.IsTrue(gradesModels.Count() == 2, "Subjects are not 2");
            Assert.IsTrue(mathGrades.Grades == "4, 5", "Math grades do not have right values");
            Assert.IsTrue(artGrades.Grades == "6", "Art grades do not have right values");
        }

        [Test]
        public async Task Get_Student_Notes()
        {
            var note = new Note
            {
                Title = "always watching his phone",
                Description = "Dont listen me in class",
                IsPostive = false,
                StudentId = 1,
                TeacherId = 1,
                SubjectId = 1,
                TeacherName = "petur"
            };

            var subject = new Subject
            {
                Name = "Math"
            };

            var student = new Student
            {
                GroupId = 1,
                UserId = user.Id
            };

            await context.Students.AddAsync(student);
            await context.Subjects.AddAsync(subject);
            await context.Notes.AddAsync(note);
            await context.SaveChangesAsync();

            var noteModels = await studentService.GetStudentNotes(new ClaimsPrincipal());
            var notesToList = noteModels.ToList();

            Assert.IsTrue(noteModels.Count() == 1, "notes count should be 1");
            Assert.IsTrue(notesToList[0].Title == note.Title, "note title has diffrent value");
            Assert.IsTrue(notesToList[0].Description == note.Description, "note Description has diffrent value");
            Assert.IsTrue(notesToList[0].SubjectName == subject.Name, "note SubjectName has diffrent value");
        }

        //[Test]
        //public async Task Get_Student_HomePage_Test()
        //{

        //}
    }
}
