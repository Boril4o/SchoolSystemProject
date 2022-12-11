using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Models.Group;
using SchoolSystem.Core.Models.Student;
using SchoolSystem.Core.Models.Subject;
using SchoolSystem.Core.Models.Teacher;
using static SchoolSystem.Areas.Admin.AdminConstans;
using static SchoolSystem.Areas.Admin.Constants.ErrorConstants;

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
                ModelState.AddModelError("", UsernameDoesNotExist);
                return View(model);
            }

            if (!await adminService.IsGroupExistAsync(model.GroupNumber))
            {
                ModelState.AddModelError("", GroupDoesNotExist);
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
            model.subjects = await adminService.GetSubjects();

            return View(model);
        }

        [Area(AreaName)]
        [Authorize(Roles = AdminRoleName)]
        [HttpPost]
        public async Task<IActionResult> AddTeacher(AddTeacherViewModel model)
        {
            if (!await adminService.IsUserNameExistAsync(model.UserName))
            {
                ModelState.AddModelError("", UsernameDoesNotExist);
                return View(model);
            }

            if (!await adminService.IsSubjectExistAsync(model.SubjectId))
            {
                ModelState.AddModelError("", SubjectDoesNotExist);
                return View(model);
            }

            if (model.GroupNumber != null &&
                !await adminService.IsGroupExistAsync(model.GroupNumber))
            {
                ModelState.AddModelError("", GroupDoesNotExist);
                return View(model);
            }

            await adminService.AddTeacherAsync(model);
            return View(nameof(Index));
        }

        [Area(AreaName)]
        [Authorize(Roles = AdminRoleName)]
        [HttpGet]
        public IActionResult AddGroup()
        {
            AddGroupViewModel model = new AddGroupViewModel();

            return View(model);
        }

        [Area(AreaName)]
        [Authorize(Roles = AdminRoleName)]
        [HttpPost]
        public async Task<IActionResult> AddGroup(AddGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await adminService.IsGroupExistAsync(model.Number))
            {
                ModelState.AddModelError("", GroupAlreadyExist);
                return View(model);
            }

            await adminService.AddGroupAsync(model);
            return RedirectToAction(nameof(Index));
        }
        
        [Area(AreaName)]
        [Authorize(Roles = AdminRoleName)]
        [HttpGet]
        public IActionResult AddSubject()
        {
            var model = new AddSubjectViewModel();

            return View(model);
        }

        [Area(AreaName)]
        [Authorize(Roles = AdminRoleName)]
        [HttpPost]
        public async Task<IActionResult> AddSubject(AddSubjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await adminService.IsSubjectExistAsync(model.Name))
            {
                ModelState.AddModelError("", SubjectAlreadyExist);
                return View(model);
            }

            await adminService.AddSubjectAsync(model);
            return View(nameof(Index));
        }

        [Area(AreaName)]
        [Authorize(Roles = AdminRoleName)]
        [HttpGet]
        public async Task<IActionResult> AllTeachers()
        {
            IEnumerable<TeacherViewModel> teachers =  await adminService.AllTeachers();

            return View(teachers);
        }

        [Area(AreaName)]
        [Authorize(Roles = AdminRoleName)]
        [HttpGet]
        public async Task<IActionResult> EditTeacher(int id)
        {
            var currentModel = await adminService.GetTeacher(id);

            EditTeacherViewModel model = new EditTeacherViewModel()
            {
                UserName = currentModel.User.UserName,
                FirstName = currentModel.User.FirstName,
                LastName = currentModel.User.LastName,
                GroupId = currentModel.GroupID,
                Salary = currentModel.Salary,
                SubjectId = currentModel.SubjectId,
                
                Subjects = await adminService.GetSubjects(),
                Groups = await adminService.GetGroups(),
                Id = id
            };


            return View(model);
        }

        [Area(AreaName)]
        [Authorize(Roles = AdminRoleName)]
        [HttpPost]
        public async Task<IActionResult> EditTeacher(EditTeacherViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await adminService.EditTeacherAsync(model.Id, model);

            return View(nameof(Index));
        }
        
        [Area(AreaName)]
        [Authorize(Roles = AdminRoleName)]
        [HttpGet]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            await adminService.DeleteTeacherAsync(id);

            return View(nameof(Index));
        }
    }
}
