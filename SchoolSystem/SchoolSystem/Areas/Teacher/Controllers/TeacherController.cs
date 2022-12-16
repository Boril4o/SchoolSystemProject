using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Models.Grade;
using SchoolSystem.Core.Models.Note;
using SchoolSystem.Core.Services;

namespace SchoolSystem.Areas.Teacher.Controllers
{
    [Area(TeacherConstants.TeacherAreaName)]
    [Authorize(Roles = TeacherConstants.TeacherRoleName)]
    public class TeacherController : Controller
    {
        private readonly ITeacherService service;

        public TeacherController(ITeacherService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> AllGroups()
        {
            return View(await service.AllGroups());
        }

        [HttpGet]
        public async Task<IActionResult> AllStudentsFromGroup(int id)
        {
            return View(await service.AllStudentsFromGroup(id));
        }

        [HttpGet]
        public async Task<IActionResult> AddGrade(int id)
        {
            AddGradeViewModel model = new AddGradeViewModel();
            model.Subjects = await service.GetSubjects();
            model.StudentId = id;
            TempData["StudentId"] = model.StudentId;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddGrade(AddGradeViewModel model)
        {
            model.StudentId = (int)TempData["StudentId"];

            await service.AddGrade(model, User);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> AddNote(int id)
        {
            AddNoteViewModel model = new AddNoteViewModel();
            model.Subjects = await service.GetSubjects();
            model.StudentId = id;
            TempData["NoteStudentId"] = model.StudentId;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddNote(AddNoteViewModel model)
        {
            model.StudentId = (int)TempData["NoteStudentId"];
            model.Subjects = await service.GetSubjects();

            if (!ModelState.IsValid)
            {
                TempData["NoteStudentId"] = model.StudentId;
                return View(model);
            }

            await service.AddNote(model, User);

            return RedirectToAction("Index", "Home");
        }
    }
}
