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
                    GroupId = 1,
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

            Assert.That(allStudents.Count(), Is.EqualTo(await context.Students.CountAsync()));
        }

        [Test]
        public async Task All_Subjects_Test()
        {
            List<Subject> addSubjects = new List<Subject>();
            for (int i = 0; i < 10; i++)
            {
                addSubjects.Add(new Subject
                {
                    Name = $"{i}"
                });
            }

            await context.Subjects.AddRangeAsync(addSubjects);

            var subjects = await adminService.AllSubjects();

            Assert.That(subjects.Count(), Is.EqualTo(0));

            await context.SaveChangesAsync();

            subjects = await adminService.AllSubjects();

            Assert.That(subjects.Count(), Is.EqualTo(10));
        }

        [Test]
        public async Task All_Teachers_Teast()
        {
            List<Teacher> addTeachers = new List<Teacher>();
            for (int i = 0; i < 10; i++)
            {
                addTeachers.Add(new Teacher
                {
                    GroupID = 1,
                    Salary = 900,
                    SubjectId = 1,
                    UserId = "8f537a79-857e-41f9-a9c5-916fb3784ff2"
                });
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
                MaxPeople = 15
            };

            var subject = new Subject()
            {
                Name = "Math"
            };

            await context.Subjects.AddAsync(subject);
            await context.Groups.AddAsync(group);
            await context.Users.AddAsync(user);
            await context.Teachers.AddRangeAsync(addTeachers);
            await context.SaveChangesAsync();

            var teachers = await adminService.AllTeachers();
            Assert.That(teachers.Count(), Is.EqualTo(10));
        }

        [Test]
        public async Task Delete_Group_Test()
        {
            Group group = new Group
            {
                Number = "5A",
                MaxPeople = 15,
                Id = 1
            };

            Group group1 = new Group
            {
                Number = "5B",
                MaxPeople = 15,
                Id = 2
            };

            Group group2 = new Group
            {
                Number = "5C",
                MaxPeople = 15,
                Id = 3
            };

            await context.Groups.AddRangeAsync(group, group1, group2);
            await context.SaveChangesAsync();

            Assert.That(await context.Groups.CountAsync(), Is.EqualTo(3));

            await adminService.DeleteGroup(1);
            await adminService.DeleteGroup(2);

            Assert.That(await context.Groups.CountAsync(), Is.EqualTo(1));
        }

        [Test]
        public async Task Delete_Student_Test()
        {
            Student student = new Student
            {
                GroupId = 1,
                UserId = "8f537a79-857e-41f9-a9c5-916fb3784ff2",
                Id = 1
            };

            Student student1 = new Student
            {
                GroupId = 1,
                UserId = "8f537a79-857e-41f9-a9c5-916fb3784ff2",
                Id = 2
            };

            Student student2 = new Student
            {
                GroupId = 1,
                UserId = "8f537a79-857e-41f9-a9c5-916fb3784ff2",
                Id = 3
            };
            Student student3 = new Student
            {
                GroupId = 1,
                UserId = "8f537a79-857e-41f9-a9c5-916fb3784ff2",
                Id = 4
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

            Group group = new Group
            {
                Number = "5A",
                MaxPeople = 14,
            };

            await context.Users.AddAsync(user);
            await context.Groups.AddAsync(group);
            await context.Students.AddRangeAsync(student, student1, student2, student3);
            await context.SaveChangesAsync();

            Assert.That(await context.Students.CountAsync(), Is.EqualTo(4));

            await adminService.DeleteStudent(student.Id);
            await adminService.DeleteStudent(student1.Id);

            Assert.That(await context.Students.CountAsync(), Is.EqualTo(2));
        }

        [Test]
        public async Task Delete_Subject_Test()
        {
            List<Subject> addSubjects = new List<Subject>();
            for (int i = 1; i <= 10; i++)
            {
                addSubjects.Add(new Subject
                {
                    Name = "Art",
                    Id = i,
                });
            }

            await context.Subjects.AddRangeAsync(addSubjects);
            await context.SaveChangesAsync();

            Assert.That(await context.Subjects.CountAsync(), Is.EqualTo(10));

            for (int i = 1; i <= 5; i++)
            {
                await adminService.DeleteSubject(i);
            }

            Assert.That(await context.Subjects.CountAsync(), Is.EqualTo(5));
        }

        [Test]
        public async Task Delete_Teacher_Test()
        {
            Teacher teacher = new Teacher
            {
                GroupID = 1,
                Salary = 900,
                SubjectId = 1,
                UserId = "8f537a79-857e-41f9-a9c5-916fb3784ff2",
                Id = 1
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

            Group group = new Group
            {
                Number = "5A",
                MaxPeople = 15
            };

            Subject subject = new Subject
            {
                Name = "Art"
            };

            await context.Users.AddAsync(user);
            await context.Groups.AddAsync(group);
            await context.Subjects.AddAsync(subject);
            await context.Teachers.AddAsync(teacher);
            await context.SaveChangesAsync();

            Assert.That(await context.Teachers.CountAsync(), Is.EqualTo(1));

            await adminService.DeleteTeacherAsync(1);

            Assert.That(await context.Teachers.CountAsync(), Is.EqualTo(0));
        }

        [Test]
        public async Task Edit_Group_Test()
        {
            var model = new EditGroupViewModel
            {
                Number = "5A",
                MaxPeople = 13,
                Id = 1
            };

            var group = new Group
            {
                Number = "8B",
                MaxPeople = 30
            };

            await context.Groups.AddAsync(group);
            await context.SaveChangesAsync();

            var addedGroup = await context.Groups.FirstOrDefaultAsync();

            Assert.That(addedGroup.Number, Is.EqualTo(group.Number));
            Assert.That(addedGroup.MaxPeople, Is.EqualTo(group.MaxPeople));

            await adminService.EditGroup(model);

            var editGroup = await context.Groups.FirstOrDefaultAsync();

            Assert.That(editGroup.Number, Is.EqualTo(model.Number));
            Assert.That(editGroup.MaxPeople, Is.EqualTo(model.MaxPeople));
        }

        [Test]
        public async Task Edit_Student_Test()
        {
            var model = new EditStudentViewModel
            {
                FirstName = "Gosho",
                LastName = "Toshev",
                GroupID = 1,
                Id = 1,
                UserName = "Delcho",
            };

            var student = new Student
            {
                GroupId = 1,
                UserId = "8f537a79-857e-41f9-a9c5-916fb3784ff2",
                Id = 1
            };

            var group = new Group
            {
                Number = "5A",
                MaxPeople = 20,
                Id = 1,
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

            await context.Groups.AddAsync(group);
            await context.Users.AddAsync(user);
            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();

            var addedStudent = await context.Students.FirstOrDefaultAsync();

            Assert.That(addedStudent.User.FirstName, Is.EqualTo(user.FirstName));
            Assert.That(addedStudent.User.LastName, Is.EqualTo(user.LastName));
            Assert.That(addedStudent.User.UserName, Is.EqualTo(user.UserName));

            await adminService.EditStudent(model);

            var editStudent = await context.Students.FirstOrDefaultAsync();

            Assert.That(editStudent.User.FirstName, Is.EqualTo(model.FirstName));
            Assert.That(editStudent.User.LastName, Is.EqualTo(model.LastName));
            Assert.That(editStudent.User.UserName, Is.EqualTo(model.UserName));
        }

        [Test]
        public async Task Edit_Subject_Test()
        {
            var model = new EditSubjectViewModel
            {
                Name = "Art",
                Id = 1

            };

            var subject = new Subject
            {
                Name = "Math",
                Id = 1
            };

            await context.Subjects.AddAsync(subject);
            await context.SaveChangesAsync();

            var addedSubject = await context.Subjects.FirstOrDefaultAsync();

            Assert.That(addedSubject.Name, Is.EqualTo(subject.Name));

            await adminService.EditSubject(model);

            var editSubject = await context.Subjects.FirstOrDefaultAsync();

            Assert.That(editSubject.Name, Is.EqualTo(model.Name));
        }

        [Test]
        public async Task Edit_Teacher_Test()
        {
            var model = new EditTeacherViewModel
            {
                FirstName = "Greta",
                LastName = "Petrove",
                GroupId = 1,
                Salary = 1000,
                UserName = "User15",
                Id = 1,
                SubjectId = 1,
            };

            Group group = new Group
            {
                Number = "4A",
                MaxPeople = 30,
                Id = 2
            };

            Group group1 = new Group
            {
                Number = "4B",
                MaxPeople = 40,
                Id = 1
            };

            Subject subject = new Subject
            {
                Name = "Math"
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

            Teacher teacher = new Teacher
            {
                GroupID = 1,
                Id = 1,
                Salary = 900,
                SubjectId = 1,
                UserId = "8f537a79-857e-41f9-a9c5-916fb3784ff2"
            };

            await context.Subjects.AddAsync(subject);
            await context.Groups.AddRangeAsync(group, group1);
            await context.Users.AddAsync(user);
            await context.Teachers.AddAsync(teacher);
            await context.SaveChangesAsync();

            var addedTeacher = await context.Teachers.FirstOrDefaultAsync();

            Assert.That(addedTeacher.UserId, Is.EqualTo(user.Id));
            Assert.That(addedTeacher.GroupID, Is.EqualTo(teacher.GroupID));
            Assert.That(addedTeacher.Salary, Is.EqualTo(teacher.Salary));
            Assert.That(addedTeacher.SubjectId, Is.EqualTo(teacher.SubjectId));

            await adminService.EditTeacherAsync(model.Id, model);

            var editTeacher = await context.Teachers.FirstOrDefaultAsync();

            Assert.That(editTeacher.User.FirstName, Is.EqualTo(model.FirstName));
            Assert.That(editTeacher.User.LastName, Is.EqualTo(model.LastName));
            Assert.That(editTeacher.Salary, Is.EqualTo(model.Salary));
            Assert.That(editTeacher.SubjectId, Is.EqualTo(model.SubjectId));
            Assert.That(editTeacher.User.UserName, Is.EqualTo(model.UserName));
            Assert.That(editTeacher.GroupID, Is.EqualTo(model.GroupId));
        }

        [Test]
        public async Task Get_Group_Test()
        {
            List<Group> groups = new List<Group>()
            {
                new()
                {
                    Number = "5A",
                    MaxPeople = 20,
                    Id = 1,
                },
                new()
                {
                    Number = "5B",
                    MaxPeople = 30,
                    Id = 2
                },
                new()
                {
                    Number = "6B",
                    MaxPeople = 14,
                    Id = 3
                }
            };

            await context.Groups.AddRangeAsync(groups);
            await context.SaveChangesAsync();

            Group group1 = await adminService.GetGroup(3);
            Group group2 = await adminService.GetGroup(1);

            Assert.That(group1.Number, Is.EqualTo("6B"));
            Assert.That(group2.Number, Is.EqualTo("5A"));
        }

        [Test]
        public async Task Get_Group_By_Number_Test()
        {
            List<Group> groups = new List<Group>()
            {
                new()
                {
                    Number = "5A",
                    MaxPeople = 20,
                    Id = 1,
                },
                new()
                {
                    Number = "5B",
                    MaxPeople = 30,
                    Id = 2
                },
                new()
                {
                    Number = "6B",
                    MaxPeople = 14,
                    Id = 3
                }
            };

            await context.Groups.AddRangeAsync(groups);
            await context.SaveChangesAsync();

            Group group1 = await adminService.GetGroup("6B");
            Group group2 = await adminService.GetGroup("5A");

            Assert.That(group1.Number, Is.EqualTo("6B"));
            Assert.That(group2.Number, Is.EqualTo("5A"));
        }

        [Test]
        public async Task Get_Groups_Test()
        {
            List<Group> groups = new List<Group>()
            {
                new()
                {
                    Number = "5A",
                    MaxPeople = 20,
                    Id = 1,
                },
                new()
                {
                    Number = "5B",
                    MaxPeople = 30,
                    Id = 2
                },
                new()
                {
                    Number = "6B",
                    MaxPeople = 14,
                    Id = 3
                }
            }
            .OrderBy(x => x.Number)
            .ToList();

            await context.Groups.AddRangeAsync(groups);
            await context.SaveChangesAsync();

            var groupsFromDb = await adminService.GetGroups();
            var groupsList = groupsFromDb.OrderBy(x => x.Number).ToList();

            for (int i = 0; i < 3; i++)
            {
                Assert.That(groupsList[i].Number, Is.EqualTo(groups[i].Number));
            }
        }

        [Test]
        public async Task Get_Student_Test()
        {
            Student student = new Student()
            {
                GroupId = 1,
                UserId = "8f537a79-857e-41f9-a9c5-916fb3784ff2"
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

            Group group = new Group()
            {
                Number = "5A",
                MaxPeople = 20
            };

            await context.Groups.AddAsync(group);
            await context.Users.AddAsync(user);
            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();

            var studentFromDb = await adminService.GetStudent(1);

            Assert.That(studentFromDb.User.UserName, Is.EqualTo(user.UserName));
        }

        [Test]
        public async Task Get_Studens_Count_From_Group()
        {
            List<Student> students = new List<Student>()
            {
                new()
                {
                    GroupId = 1,
                    UserId = ""
                },
                new()
                {
                    GroupId = 1,
                    UserId = ""
                },
                new()
                {
                    GroupId = 1,
                    UserId = ""
                }
            };

            var group = new Group
            {
                Number = "5A",
                Id = 1,
                MaxPeople = 20
            };

            await context.Students.AddRangeAsync(students);
            await context.Groups.AddAsync(group);
            await context.SaveChangesAsync();

            int studentsInGroup = await adminService.GetStudentsCountFromGroup(group.Id);

            Assert.That(studentsInGroup, Is.EqualTo(students.Count()));
        }

        [Test]
        public async Task Get_Subject_By_Id_Test()
        {
            var subject = new Subject
            {
                Name = "Math",
                Id = 2
            };

            var subject1 = new Subject
            {
                Name = "Art",
                Id = 1
            };

            await context.Subjects.AddRangeAsync(subject, subject1);
            await context.SaveChangesAsync();

            var subjectFromDb1 = await adminService.GetSubject(subject.Id);
            var subjectFromDb2 = await adminService.GetSubject(subject1.Id);

            Assert.That(subjectFromDb1.Name, Is.EqualTo(subject.Name));
            Assert.That(subjectFromDb2.Name, Is.EqualTo(subject1.Name));
        }

        [Test]
        public async Task Get_Subject_By_Name_Test()
        {
            var subject = new Subject
            {
                Name = "Math",
                Id = 2
            };

            var subject1 = new Subject
            {
                Name = "Art",
                Id = 1
            };

            await context.Subjects.AddRangeAsync(subject, subject1);
            await context.SaveChangesAsync();

            var subjectFromDb = await adminService.GetSubject(subject.Name);
            var subjectFromDb1 = await adminService.GetSubject(subject1.Name);

            Assert.That(subjectFromDb1.Id, Is.EqualTo(subject1.Id));
            Assert.That(subjectFromDb.Id, Is.EqualTo(subject.Id));
        }

        [Test]
        public async Task Get_Subjects_Shoud_Return_All_Subjects_From_Db()
        {
            var subject = new Subject
            {
                Name = "Math",
                Id = 2
            };

            var subject1 = new Subject
            {
                Name = "Art",
                Id = 1
            };

            await context.Subjects.AddRangeAsync(subject, subject1);
            await context.SaveChangesAsync();

            var subjects = await adminService.GetSubjects();
            var subjectsToList = subjects.ToList();

            Assert.That(subjects.Count(), Is.EqualTo(2));
            Assert.That(subjectsToList[0].Name, Is.EqualTo(subject.Name));
            Assert.That(subjectsToList[1].Name, Is.EqualTo(subject1.Name));
        }

        [Test]
        public async Task Get_Teacher_Shoud_Not_Return_User_Property_Null()
        {
            var teacher = new Teacher
            {
                GroupID = 1,
                SubjectId = 1,
                UserId = "8f537a79-857e-41f9-a9c5-916fb3784ff2"
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

            await context.Teachers.AddAsync(teacher);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            Teacher teacherFromDb = await adminService.GetTeacher(1);

            Assert.IsTrue(teacherFromDb.User != null);
        }

        [Test]
        public async Task Is_Group_Exist_Test()
        {
            var group = new Group
            {
                Number = "5A",
                MaxPeople = 15,
                Id = 1,
            };

            await context.Groups.AddAsync(group);

            Assert.IsFalse(await adminService.IsGroupExistAsync(group.Number));

            await context.SaveChangesAsync();

            Assert.IsTrue(await adminService.IsGroupExistAsync(group.Number));
        }

        [Test]
        public async Task Is_Student_Username_Exist_Should_Return_True_If_Other_User_Has_This_Username()
        {
            var student = new Student
            {
                GroupId = 1,
                UserId = "8f537a79-857e-41f9-a9c5-916fb3784ff2"
            };

            var student1 = new Student
            {
                GroupId = 1,
                UserId = "8f537a79-857e-41f9-a9c5"
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

            var user1 = new User
            {
                Id = "8f537a79-857e-41f9-a9c5",
                Age = 15,
                Email = "Boris@gmail.com",
                FirstName = "Boris",
                LastName = "Goshev",
                UserName = "Gosho",
                PasswordHash = "1234"
            };

            await context.Users.AddRangeAsync(user1, user);
            await context.Students.AddRangeAsync(student, student1);
            await context.SaveChangesAsync();

            Assert.IsTrue(await adminService.IsStudentUserNameExistAsync(user1.UserName, student.Id));
        }

        [Test]
        public async Task Is_Student_Username_Exist_Should_Return_False_If_No_Other_User_Has_This_Username()
        {
            var student = new Student
            {
                GroupId = 1,
                UserId = "8f537a79-857e-41f9-a9c5-916fb3784ff2"
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

            await context.Students.AddAsync(student);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            Assert.IsFalse(await adminService.IsStudentUserNameExistAsync(user.UserName, student.Id));
        }

        [Test]
        public async Task Is_Subjects_Exist_With_Username_Test()
        {
            var subject = new Subject
            {
                Name = "Art"
            };

            await context.Subjects.AddAsync(subject);
            await context.SaveChangesAsync();

            Assert.IsTrue(await adminService.IsSubjectExistAsync(subject.Name));
            Assert.IsFalse(await adminService.IsSubjectExistAsync("Math"));
        }

        [Test]
        public async Task Is_Subjects_Exist_With_Id_Test()
        {
            var subject = new Subject
            {
                Name = "Art",
                Id = 4
            };

            await context.Subjects.AddAsync(subject);
            await context.SaveChangesAsync();

            Assert.IsTrue(await adminService.IsSubjectExistAsync(subject.Id));
            Assert.IsFalse(await adminService.IsSubjectExistAsync(5));
        }

        [Test]
        public async Task Is_Teacher_Username_Exist_Test()
        {
            var teacher = new Teacher
            {
                GroupID = 1,
                Salary = 900,
                SubjectId = 1,
                UserId = "8f537a79-857e-41f9-a9c5-916fb3784ff2"
            };

            var teacher1 = new Teacher
            {
                GroupID = 1,
                Salary = 900,
                SubjectId = 1,
                UserId = "8f537a79-857e-41f9-a9c5"
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

            var user1 = new User
            {
                Id = "8f537a79-857e-41f9-a9c5",
                Age = 15,
                Email = "Boris@gmail.com",
                FirstName = "Boris",
                LastName = "Goshev",
                UserName = "Gosho",
                PasswordHash = "1234"
            };

            await context.Users.AddRangeAsync(user1, user);
            await context.Teachers.AddRangeAsync(teacher, teacher1);
            await context.SaveChangesAsync();

            Assert.IsTrue(await adminService.IsTeacherUserNameExistAsync(user1.UserName, teacher.Id));
        }

        [Test]
        public async Task Is_UserName_Exist_Test()
        {
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
            await context.SaveChangesAsync();

            Assert.IsTrue(await adminService.IsUserNameExistAsync(user.UserName));
            Assert.IsFalse(await adminService.IsUserNameExistAsync("Ivan"));
        }
    }
}
