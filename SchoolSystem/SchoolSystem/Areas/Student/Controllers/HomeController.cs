using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static SchoolSystem.Areas.Student.StudentConstants;

namespace SchoolSystem.Areas.Student.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = StudentRoleName)]
        [Area(AreaName)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
