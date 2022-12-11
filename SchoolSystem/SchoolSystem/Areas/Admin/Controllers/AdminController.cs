using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Models.Student;
using SchoolSystem.Core.Models.Teacher;
using static SchoolSystem.Areas.Admin.AdminConstans;

namespace SchoolSystem.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [Area(AreaName)]
        [Authorize(Roles = AdminRoleName)]
        public IActionResult Index()
        {
            return View();
        }

        [Area(AreaName)]
        [Authorize(Roles = AdminRoleName)]
        [HttpGet]
        public IActionResult AddStudent()
        {
            AddStudentViewModel model = new AddStudentViewModel();

            return View(model);
        }

        [Area(AreaName)]
        [Authorize(Roles = AdminRoleName)]
        [HttpPost]
        public async Task<IActionResult> AddStudent(AddStudentViewModel model)
        {
            if (!await adminService.IsUserNameExistAsync(model.UserName))
            {
                ModelState.AddModelError("", "Username does not exists");
                return View(model);
            }

            if(!await adminService.IsGroupExistAsync(model.GroupNumber))
            {
                ModelState.AddModelError("", "Group does not exists");
                return View(model);
            }

            await adminService.AddStudentAsync(model);
            return View(nameof(Index));
        }

        [Area(AreaName)]
        [Authorize(Roles = AdminRoleName)]
        [HttpGet]
        public async Task<IActionResult> AddTeacher()
        {
            AddTeacherViewModel model = new AddTeacherViewModel();

            return View(model);
        }

        [Area(AreaName)]
        [Authorize(Roles = AdminRoleName)]
        [HttpPost]
        public async Task<IActionResult> AddTeacher(AddTeacherViewModel model)
        {
            if (!await adminService.IsUserNameExistAsync(model.UserName))
            {
                ModelState.AddModelError("", "Username does not exists");
                return View(model);
            }

            if (!await adminService.IsSubjectExistAsync(model.SubjectName))
            {
                ModelState.AddModelError("", "Subject does not exists");
                return View(model);
            }

            if (model.GroupNumber != null &&
                !await adminService.IsGroupExistAsync(model.GroupNumber))
            {
                ModelState.AddModelError("", "Group does not exists");
                return View(model);
            }

            await adminService.AddTeacherAsync(model);
            return View(model);
        }
    }
}
