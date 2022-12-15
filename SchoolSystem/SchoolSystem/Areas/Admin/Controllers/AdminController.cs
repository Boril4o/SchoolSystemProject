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
    [Area(AreaName)]
    [Authorize(Roles = AdminRoleName)]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;

        private string ErrorMessage(Exception e)
        {
            string message;
            if (e.GetType().Name == nameof(ArgumentException))
            {
                message = e.Message;
            }
            else
            {
                message = "Error";
            }

            return message;
        }

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddStudent()
        {
            AddStudentViewModel model = new AddStudentViewModel();
            model.Groups = await adminService.GetGroups();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(AddStudentViewModel model)
        {
            try
            {
                await adminService.AddStudentAsync(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", ErrorMessage(e));
                model.Groups = await adminService.GetGroups();
                return View(model);
            }

            
            return View(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AddTeacher()
        {
            AddTeacherViewModel model = new AddTeacherViewModel();
            model.subjects = await adminService.GetSubjects();
            model.Groups = await adminService.GetGroups();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddTeacher(AddTeacherViewModel model)
        {
            try
            {
                await adminService.AddTeacherAsync(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", ErrorMessage(e));
                model.subjects = await adminService.GetSubjects();
                model.Groups = await adminService.GetGroups();
                return View(model);
            }

            return View(nameof(Index));
        }

        [HttpGet]
        public IActionResult AddGroup()
        {
            AddGroupViewModel model = new AddGroupViewModel();

            return View(model);
        }

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
                ModelState.AddModelError("", ErrorMessage(e));
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public IActionResult AddSubject()
        {
            var model = new AddSubjectViewModel();

            return View(model);
        }

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
                ModelState.AddModelError("", ErrorMessage(e));
                return View(model);
            }

            return View(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AllTeachers()
        {
            IEnumerable<TeacherViewModel> teachers =  await adminService.AllTeachers();

            return View(teachers);
        }

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
        
        [HttpGet]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            await adminService.DeleteTeacherAsync(id);

            return View(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AllStudents()
        {
            return View(await adminService.AllStudents());
        }

        [HttpGet]
        public async Task<IActionResult> EditStudent(int id)
        {
            SchoolSystem.Data.Data.Entities.Student s = await adminService.GetStudent(id);
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
                ModelState.AddModelError("", ErrorMessage(e));

                model.Groups = await adminService.GetGroups();
                return View(model);
            }

            return View(nameof(AllStudents), await adminService.AllStudents());
        }

        [HttpGet]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            await adminService.DeleteStudent(id);
            return View(nameof(AllStudents), await adminService.AllStudents());
        }

        [HttpGet]
        public async Task<IActionResult> AllGroups()
        {
            return View(await adminService.AllGroups());
        }

        [HttpGet]
        public async Task<IActionResult> EditGroup(int id)
        {
            Group group = await adminService.GetGroup(id);
            EditGroupViewModel model = new EditGroupViewModel
            {
                Number = group.Number,
                MaxPeople = group.MaxPeople,
                Id = group.Id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditGroup(EditGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await adminService.EditGroup(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", ErrorMessage(e));

                return View(model);
            }

            return View(nameof(AllGroups), await adminService.AllGroups());
        }

        [HttpGet]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            await adminService.DeleteGroup(id);

            return View(nameof(AllGroups), await adminService.AllGroups());
        }

        [HttpGet]
        public async Task<IActionResult> AllSubjects()
        {
            return View(await adminService.AllSubjects());
        }

        [HttpGet]
        public async Task<IActionResult> EditSubject(int id)
        {
            Subject s = await adminService.GetSubject(id);
            EditSubjectViewModel model = new EditSubjectViewModel
            {
                Name = s.Name,
                Id = id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditSubject(EditSubjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await adminService.EditSubject(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", ErrorMessage(e));
            }

            return View(nameof(AllSubjects), await adminService.AllSubjects());
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            await adminService.DeleteSubject(id);

            return View(nameof(AllSubjects), await adminService.AllSubjects());
        }
    }
}
