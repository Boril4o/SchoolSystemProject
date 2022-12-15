using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Models.Grade;
using SchoolSystem.Core.Models.Note;
using SchoolSystem.Core.Services;

namespace SchoolSystem.Areas.Teacher.Controllers
{
    [Area(TeacherConstants.AreaName)]
    [Authorize(Roles = TeacherConstants.TeacherRoleName)]
    public class TeacherController : Controller
    {
        private readonly ITeacherService service;

        public TeacherController(ITeacherService service)
        {
            this.service = service;
        }

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
            try
            {
                await service.AddGrade(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", ErrorMessage(e));
                model.Subjects = await service.GetSubjects();
                return View(model);
            }

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

            try
            {
                await service.AddNote(model, User);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", ErrorMessage(e));
                model.Subjects = await service.GetSubjects();
                TempData["NoteStudentId"] = model.StudentId;
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
