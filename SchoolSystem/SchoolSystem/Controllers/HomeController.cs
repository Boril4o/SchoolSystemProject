using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.Models;
using System.Diagnostics;
using static SchoolSystem.Areas.Admin.AdminConstans;
using static SchoolSystem.Areas.Teacher.TeacherConstants;
using static SchoolSystem.Areas.Student.StudentConstants;

namespace SchoolSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.IsInRole(AdminRoleName))
            {
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
            else if (User.IsInRole(TeacherRoleName))
            {
                return RedirectToAction("Index", "Home", new { area = "Teacher" });
            }
            else if (User.IsInRole(StudentRoleName))
            {
                return RedirectToAction("Index", "Home", new { area = "Student" });
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}