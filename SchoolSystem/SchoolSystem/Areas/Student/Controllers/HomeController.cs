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
        [Area(AreaName)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            StudentHomePageStatsViewModel model;
            try
            {
                model = await service.GetStudentHomePageStats(User);
            }
            catch
            {
                return NotFound();
            }

            return View(model);
        }
    }
}
