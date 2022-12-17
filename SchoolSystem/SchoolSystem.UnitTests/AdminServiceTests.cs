using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Models.Group;
using SchoolSystem.Core.Models.Student;
using SchoolSystem.Core.Models.Subject;
using SchoolSystem.Core.Models.Teacher;
using SchoolSystem.Core.Services;
using SchoolSystem.Infrastructure.Data;
using SchoolSystem.Infrastructure.Data.Entities;


namespace SchoolSystem.UnitTests
{
    [TestFixture]
    public class AdminServiceTests
    {
        private ApplicationDbContext context;
        private UserManager<User> userManager;
        private IAdminService adminService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Db")
                .Options;

            this.context = new ApplicationDbContext(options);
            this.context.Database.EnsureDeleted();
            this.context.Database.EnsureCreated();


            var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(),
                null, null, null, null, null, null, null, null);

            userManager = userManagerMock.Object;
            adminService = new AdminService(context, userManager);
        }

        [Test]
        public async Task Add_Group_Test()
        {
            var model = new AddGroupViewModel
            {
                Number = "5A",
                MaxPeople = 25
            };

            await adminService.AddGroupAsync(model);

            Assert.That(context.Groups.Count(), Is.EqualTo(1));

            Group group = await context.Groups.FirstOrDefaultAsync();

            Assert.That(group.Number, Is.EqualTo(model.Number));
            Assert.That(group.MaxPeople, Is.EqualTo(model.MaxPeople));
        }

		[Test]
        public async Task Add_Student_Test()
		{
            var student = new AddStudentViewModel()
            {
                UserName = "Boris",
                GroupId = 1
            };

            var group = new Group()
            {
                Number = "5A",
                MaxPeople = 15
            };

            var user = new User
            {
                Id = "8f537a79-857e-41f9-a9c5-916fb3784ff2",
                Age = 15,
                Email = "Boris@gmail.com",
                FirstName = "Boris",
                LastName = "Goshev",
                UserName = "Boris",
                PasswordHash = "1234"
            };

            await context.Users.AddAsync(user);
            await context.Groups.AddAsync(group);
            await context.SaveChangesAsync();

            await adminService.AddStudentAsync(student);

            Assert.That(await context.Students.CountAsync(), Is.EqualTo(1));
		}

		[Test]
        public async Task Add_Subjects_Test()
		{
            var model = new AddSubjectViewModel()
            {
                Name = "Math"
            };

            await adminService.AddSubjectAsync(model);

            Assert.That(await context.Subjects.CountAsync(), Is.EqualTo(1));

            Subject subject = await context.Subjects.FirstOrDefaultAsync();

            Assert.That(subject.Name, Is.EqualTo(model.Name));
		}

		[Test]
        public async Task Add_Teacher_Test()
		{
            var teacher = new AddTeacherViewModel()
            {
                GroupId = 1,
                Salary = 900,
                SubjectId = 1,
                UserName = "Boris"
            };

            var user = new User
            {
                Id = "8f537a79-857e-41f9-a9c5-916fb3784ff2",
                Age = 15,
                Email = "Boris@gmail.com",
                FirstName = "Boris",
                LastName = "Goshev",
                UserName = "Boris",
                PasswordHash = "1234"
            };

            var group = new Group
            {
                Number = "5A",
                MaxPeople = 15,
            };

            var subject = new Subject
            {
                Name = "Art"
            };

            await context.Users.AddAsync(user);
            await context.Groups.AddAsync(group);
            await context.Subjects.AddAsync(subject);
            await context.SaveChangesAsync();

            await adminService.AddTeacherAsync(teacher);

            Assert.That(await context.Teachers.CountAsync(), Is.EqualTo(1));
		}

		[Test]
        public async Task All_Groups_Test()
		{
            List<Group> groups = new List<Group>();
			for (int i = 0; i < 10; i++)
			{
                groups.Add(new Group
                {
                    MaxPeople = i,
                    Number = "5" + i,
                });
			}

            await context.Groups.AddRangeAsync(groups);
            await context.SaveChangesAsync();

            IEnumerable<GroupViewModel> AllGroups = await adminService.AllGroups();

            Assert.That(AllGroups.Count(), Is.EqualTo(groups.Count));

            int count = 0;
			foreach (var group in AllGroups)
			{
                Assert.That(group.Number, Is.EqualTo(groups[count].Number));
                Assert.That(group.MaxPeople, Is.EqualTo(groups[count].MaxPeople));
                Assert.That(group.Id, Is.EqualTo(groups[count].Id));
                count++;
            }
		}

		[Test] 
        public async Task All_Groups_People_Property_Shoud_Be_Not_Zero()
		{
            List<Student> students = new List<Student>();
			for (int i = 0; i < 5; i++)
			{
                students.Add(new Student
                {
                    UserId = $"{i}",
                    GroupId = 1,
                    Id = i + new Random().Next(1, 20000),
                });
			}

			for (int i = 1; i <= 3; i++)
			{
                students.Add(new Student
                {
                    UserId = $"{i}",
                    GroupId = 2,
                    Id = i + new Random().Next(1, 20000),
                });
			}

            Group firstGroup = new Group
            {
                Number = "5A",
                MaxPeople = 15,
                Id = 1
            };

            Group secondGroup = new Group
            {
                Number = "6A",
                MaxPeople = 15,
                Id = 2
            };

            await context.Students.AddRangeAsync(students);
            await context.Groups.AddRangeAsync(firstGroup, secondGroup);
            await context.SaveChangesAsync();


            IEnumerable<GroupViewModel> groups = await adminService.AllGroups();
			var groupsList = groups.ToList();


            Assert.That(groupsList[0].People, Is.EqualTo(5));
            Assert.That(groupsList[1].People, Is.EqualTo(3));
        }

		[Test]
        public async Task All_Students_Test()
		{
            List<Student> students = new List<Student>();
            List<User> users = new List<User>();
			for (int i = 0; i < 10; i++)
			{
                students.Add(new Student
                {
                    UserId = $"{i}",
                    GroupId = i,
                });

                users.Add(new User
                {
                    Id = $"{i}",
                    Age = 15,
                    Email = "Boris@gmail.com",
                    FirstName = "Boris",
                    LastName = "Goshev",
                    UserName = "Boris",
                    PasswordHash = "1234"
                });
			}

            Group group = new Group()
            {
                Number = "5A",
                MaxPeople = 20
            };

            await context.Users.AddRangeAsync(users);
            await context.Students.AddRangeAsync(students);
            await context.Groups.AddAsync(group);
            await context.SaveChangesAsync();

            IEnumerable<StudentViewModel> allStudents = await adminService.AllStudents();

            Assert.That(allStudents.Count(), Is.EqualTo(students.Count));
        }
    }
}
