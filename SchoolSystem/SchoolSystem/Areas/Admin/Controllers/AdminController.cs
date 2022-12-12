using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Models.Group;
using SchoolSystem.Core.Models.Student;
using SchoolSystem.Core.Models.Subject;
using SchoolSystem.Core.Models.Teacher;
using SchoolSystem.Data.Data.Entities;
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
            try
            {
                await adminService.AddStudentAsync(model);
            }
            catch (Exception e)
            {
                if (e.GetType().Name == nameof(ArgumentException))
                {
                    ModelState.AddModelError("", e.Message);
                }
                else
                {
                    ModelState.AddModelError("", "Error");
                }
                return View(model);
            }

            
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
            try
            {
                await adminService.AddTeacherAsync(model);
            }
            catch (Exception e)
            {
                if (e.GetType().Name == nameof(ArgumentException))
                {
                    ModelState.AddModelError("", e.Message);
                }
                else
                {
                    ModelState.AddModelError("", "Error");
                }
                model.subjects = await adminService.GetSubjects();
                return View(model);
            }

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

            try
            {
                await adminService.AddGroupAsync(model);
            }
            catch (Exception e)
            {
                if (e.GetType().Name == nameof(ArgumentException))
                {
                    ModelState.AddModelError("", e.Message);
                }
                else
                {
                    ModelState.AddModelError("", "Error");
                }
                return View(model);
            }

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

            try
            {
                await adminService.AddSubjectAsync(model);

            }
            catch (Exception e)
            {
                if (e.GetType().Name == nameof(ArgumentException))
                {
                    ModelState.AddModelError("", e.Message);
                }
                else
                {
                    ModelState.AddModelError("", "Error");
                }
                return View(model);
            }

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
            model.Subjects = await adminService.GetSubjects();
            model.Groups = await adminService.GetGroups();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await adminService.EditTeacherAsync(model.Id, model);

            return View(nameof(AllTeachers), await adminService.AllTeachers());
        }
        
        [Area(AreaName)]
        [Authorize(Roles = AdminRoleName)]
        [HttpGet]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            await adminService.DeleteTeacherAsync(id);

            return View(nameof(Index));
        }

        [Area(AreaName)]
        [Authorize(Roles = AdminRoleName)]
        [HttpGet]
        public async Task<IActionResult> AllStudents()
        {
            return View(await adminService.AllStudents());
        }

        [Area(AreaName)]
        [Authorize(Roles = AdminRoleName)]
        [HttpGet]
        public async Task<IActionResult> EditStudent(int id)
        {
            Student s = await adminService.GetStudent(id);
            EditStudentViewModel model = new EditStudentViewModel
            {
                FirstName = s.User.FirstName,
                GroupID = s.GroupId,
                Groups = await adminService.GetGroups(),
                Id = id,
                LastName = s.User.LastName,
                UserName = s.User.UserName
            };

            return View(model);
        }

        [Area(AreaName)]
        [Authorize(Roles = AdminRoleName)]
        [HttpPost]
        public async Task<IActionResult> EditStudent(EditStudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Groups = await adminService.GetGroups();
                return View(model);
            }

            try
            {
                await adminService.EditStudent(model);
            }
            catch (Exception e)
            {
                if (e.GetType().Name == nameof(ArgumentException))
                {
                    ModelState.AddModelError("", e.Message);
                }
                else
                {
                    ModelState.AddModelError("", "Error");
                }

                model.Groups = await adminService.GetGroups();
                return View(model);
            }

            return View(nameof(AllStudents), await adminService.AllStudents());
        }

        [Area(AreaName)]
        [Authorize(Roles = AdminRoleName)]
        [HttpGet]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            await adminService.DeleteStudent(id);
            return View(nameof(AllStudents), await adminService.AllStudents());
        }
    }
}
