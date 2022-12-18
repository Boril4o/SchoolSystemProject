using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Models.Grade;
using SchoolSystem.Core.Models.Note;
using SchoolSystem.Core.Services;
using SchoolSystem.Infrastructure.Data;
using SchoolSystem.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.UnitTests
{
    [TestFixture]
    public class TeacherServiceTests
    {
        private ITeacherService teacherService;
        private ApplicationDbContext context;
        private UserManager<User> userManager;
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
            teacherService = new TeacherService(context, userManager);
        }

        [Test]
        public async Task Add_Grade_Test()
        {
            Teacher teacher = new Teacher()
            {
                GroupID = 1,
                Salary = 900,
                SubjectId = 1,
                UserId = user.Id
            };

            var subject = new Subject()
            {
                Name = "Math"
            };

            var model = new AddGradeViewModel()
            {
                Number = 6,
                StudentId = 1,
                SubjectId = 1,
            };

            await context.Subjects.AddAsync(subject);
            await context.Teachers.AddAsync(teacher);
            await context.SaveChangesAsync();

            await teacherService.AddGrade(model, new ClaimsPrincipal());

            Grade grade = await context.Grades.FirstOrDefaultAsync();

            Assert.IsTrue(grade != null, "Grade is not added to database");
            Assert.IsTrue(grade.TeacherId == teacher.Id, "Grade has invalid teacher id");
        }

        [Test]
        public async Task Add_Note_Test()
        {
            Teacher teacher = new Teacher()
            {
                GroupID = 1,
                Salary = 900,
                SubjectId = 1,
                UserId = user.Id
            };

            var model = new AddNoteViewModel()
            {
                Title = "title",
                Description = "description",
                IsPositive = true,
                StudentId = 1,
                SubjectId = 1,
                TeacherId = 1,
            };

            await context.Teachers.AddAsync(teacher);
            await context.SaveChangesAsync();

            await teacherService.AddNote(model, new ClaimsPrincipal());

            Note note = await context.Notes.FirstOrDefaultAsync();

            Assert.IsTrue(note != null, "Note is not added to database");
        }

        [Test]
        public async Task All_Groups_Test()
        {
            var firstGroup = new Group
            {
                Number = "5A",
                MaxPeople = 15
            };

            var secondGroup = new Group
            {
                Number = "6A",
                MaxPeople = 15
            };

            List<Student> students = new List<Student>();
            for (int i = 0; i < 5; i++)
            {
                students.Add(new Student()
                {
                    GroupId = 1,
                    UserId = user.Id
                });
            }

            for (int i = 0; i < 12; i++)
            {
                students.Add(new Student()
                {
                    GroupId = 2,
                    UserId = user.Id
                });
            }

            await context.Students.AddRangeAsync(students);
            await context.Groups.AddRangeAsync(firstGroup, secondGroup);
            await context.SaveChangesAsync();

            var groups = await teacherService.AllGroups();

            int firstGroupCount = groups.Where(x => x.Number == firstGroup.Number).FirstOrDefault().People;
            int secondGroupCount = groups.Where(x => x.Number == secondGroup.Number).FirstOrDefault().People;

            Assert.IsTrue(firstGroupCount == 5, "First group should have 5 students");
            Assert.IsTrue(secondGroupCount == 12, "Second group should have 12 students");
        }

        [Test]
        public async Task All_Students_From_Group()
        {
            var students = new List<Student>();
            for (int i = 0; i < 20; i++)
            {
                students.Add(new Student
                {
                    GroupId = 1,
                    UserId = user.Id
                });
            }

            Group group = new Group()
            {
                Id = 1,
                Number = "5A"
            };

            await context.Students.AddRangeAsync(students);
            await context.Groups.AddAsync(group);
            await context.SaveChangesAsync();

            var studentsModels = await teacherService.AllStudentsFromGroup(group.Id);

            Assert.IsTrue(studentsModels.Count() == students.Count,
                "Students models and students list are not the same count");

            Assert.IsTrue(studentsModels.All(x => x.Group == group.Number),
                "Students models are not from the right group");
        }

        [Test]
        public async Task Get_Subjects_Test()
        {
            var subjects = new List<Subject>();
            for (int i = 0; i < 10; i++)
            {
                subjects.Add(new Subject
                {
                    Name = "Math"
                });
            }

            await context.Subjects.AddRangeAsync(subjects);
            await context.SaveChangesAsync();

            var subjectsModels = await teacherService.GetSubjects();

            Assert.IsTrue(subjectsModels.Count() == subjects.Count, "Not all subjects are added");
            Assert.IsTrue(subjectsModels.All(x => x.Name == "Math"), "Subjects are not with right values");
        }
    }
}
