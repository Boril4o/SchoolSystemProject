using Microsoft.EntityFrameworkCore;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Services;
using SchoolSystem.Infrastructure.Data;
using SchoolSystem.Infrastructure.Data.Entities;


namespace SchoolSystem.UnitTests
{
    [TestFixture]
    public class UserSeviceTests
    {
        private ApplicationDbContext context;
        private IUserService userService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "Db")
               .Options;

            this.context = new ApplicationDbContext(options);
            this.context.Database.EnsureDeleted();
            this.context.Database.EnsureCreated();

            userService = new UserService(context);
        }

        [Test]
        public async Task Get_User_By_Username()
        {
            var user = new User
            {
                Id = "1",
                Age = 15,
                Email = "Boris@gmail.com",
                FirstName = "Boris",
                LastName = "Goshev",
                UserName = "Boris",
                PasswordHash = "1234"
            };

            var user1 = new User
            {
                Id = "2",
                Age = 19,
                Email = "tosho@gmail.com",
                FirstName = "Tosho",
                LastName = "Goshev",
                UserName = "Tosheto",
                PasswordHash = "123456789"
            };

            await context.Users.AddAsync(user);
            await context.Users.AddAsync(user1);
            await context.SaveChangesAsync();

            var firstUser = await userService.GetUser(user.UserName);
            var secondUser = await userService.GetUser(user1.UserName);

            Assert.IsTrue(firstUser.Id == user.Id, "User id is not the same");
            Assert.IsTrue(secondUser.Id == user1.Id, "User id is not the same");
        }
    }
}
