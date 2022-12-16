using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Models.Student;
using static SchoolSystem.Areas.Student.StudentConstants;

namespace SchoolSystem.Areas.Student.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentService service;

        public HomeController(IStudentService service)
        {
            this.service = service;
        }

        [Authorize(Roles = StudentRoleName)]
        [Area(StudentAreaName)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await service.GetStudentHomePageStats(User));
        }
    }
}
