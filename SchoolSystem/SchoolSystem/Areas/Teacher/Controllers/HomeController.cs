using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolSystem.Areas.Teacher.Controllers
{
    public class HomeController : Controller
    {
        [Area(TeacherConstants.TeacherAreaName)]
        [Authorize(Roles = TeacherConstants.TeacherRoleName)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
