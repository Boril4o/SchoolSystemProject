using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Models.Note;
using SchoolSystem.Core.Models.Student;
using static SchoolSystem.Areas.Student.StudentConstants;

namespace SchoolSystem.Areas.Student.Controllers
{
    [Authorize(Roles = StudentRoleName)]
    [Area(AreaName)]
    public class StudentController : Controller
    {
        private readonly IStudentService service;

        public StudentController(IStudentService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ShowGrades()
        {
            IEnumerable<StudentGradesViewModel> model;
            try
            {
                model = await service.GetStudentGrades(User);
            }
            catch
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ShowNotes()
        {
            IEnumerable<NoteViewModel> model;
            try
            {
                model = await service.GetStudentNotes(User);
            }
            catch
            {
                return NotFound();
            }

            return View(model);
        }
    }
}
