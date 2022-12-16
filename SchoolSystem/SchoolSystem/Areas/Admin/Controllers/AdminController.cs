using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Models.Group;
using SchoolSystem.Core.Models.Student;
using SchoolSystem.Core.Models.Subject;
using SchoolSystem.Core.Models.Teacher;
using SchoolSystem.Infrastructure.Data.Entities;
using static SchoolSystem.Areas.Admin.AdminConstans;
using static SchoolSystem.Areas.ErrorConstants;
using static SchoolSystem.Areas.Student.StudentConstants;
using static SchoolSystem.Areas.Teacher.TeacherConstants;

namespace SchoolSystem.Areas.Admin.Controllers
{
    [Area(AreaName)]
    [Authorize(Roles = AdminRoleName)]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;

        public AdminController(IAdminService adminService,
            IUserService userService,
            UserManager<User> userManager)
        {
            this.adminService = adminService;
            this.userService = userService;
            this.userManager = userManager;
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
            model.Groups = await adminService.GetGroups();
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!await adminService.IsUserNameExistAsync(model.UserName))
            {
                ModelState.AddModelError("", UsernameDoesNotExist);
                return View(model);
            }

            User user = await userService.GetUser(model.UserName);

            if (await userManager.IsInRoleAsync(user, StudentRoleName))
            {
                ModelState.AddModelError("", StudentAlreadyExist);
                return View(model);
            }

            if (await userManager.IsInRoleAsync(user, TeacherRoleName))
            {
                ModelState.AddModelError("", UserCantBeStudentAndTeacher);
                return View(model);
            }

            await adminService.AddStudentAsync(model);
            
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
            model.subjects = await adminService.GetSubjects();
            model.Groups = await adminService.GetGroups();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!await adminService.IsUserNameExistAsync(model.UserName))
            {
                ModelState.AddModelError("", UsernameDoesNotExist);
                return View(model);
            }

            User user = await userService.GetUser(model.UserName);

            if (await userManager.IsInRoleAsync(user, TeacherRoleName))
            {
                ModelState.AddModelError("", TeacherAlreadyExist);
                return View(model);
            }

            if (await userManager.IsInRoleAsync(user, StudentRoleName))
            {
                ModelState.AddModelError("", UserCantBeStudentAndTeacher);
                return View(model);
            }

            await adminService.AddTeacherAsync(model);

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

            if (await adminService.IsGroupExistAsync(model.Number))
            {
                ModelState.AddModelError("", GroupAlreadyExist);
                return View(model);
            }

            await adminService.AddGroupAsync(model);

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

            if (await adminService.IsSubjectExistAsync(model.Name))
            {
                ModelState.AddModelError("", SubjectAlreadyExist);
                return View(model);
            }

            await adminService.AddSubjectAsync(model);

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

            if (await adminService.IsTeacherUserNameExistAsync(model.UserName, model.Id))
            {
                ModelState.AddModelError("", UserNameExist);
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
            Infrastructure.Data.Entities.Student s = await adminService.GetStudent(id);
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
            model.Groups = await adminService.GetGroups();
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await adminService.IsStudentUserNameExistAsync(model.UserName, model.Id))
            {
                ModelState.AddModelError("", UserNameExist);
                return View(model);
            }

            await adminService.EditStudent(model);

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

            Group group = await adminService.GetGroup(model.Number);

            if (group != null && group.Id != model.Id)
            {
                ModelState.AddModelError("", GroupAlreadyExist);
                return View(model);
            }

            if (model.MaxPeople < await adminService.GetStudentsCountFromGroup(model.Id))
            {
                ModelState.AddModelError("", MaxPeopleLowerThanCurrentPeople);
                return View(model);
            }
            
            await adminService.EditGroup(model);

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

            Subject subject = await adminService.GetSubject(model.Name);

            if (subject != null && subject.Id != model.Id)
            {
                ModelState.AddModelError("", SubjectAlreadyExist);
                return View(model);
            }

            await adminService.EditSubject(model);

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
