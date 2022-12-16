using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Models.Note;
using SchoolSystem.Core.Models.Student;
using static SchoolSystem.Areas.Student.StudentConstants;

namespace SchoolSystem.Areas.Student.Controllers
{
    [Authorize(Roles = StudentRoleName)]
    [Area(StudentAreaName)]
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
            return View(await service.GetStudentGrades(User));
        }

        [HttpGet]
        public async Task<IActionResult> ShowNotes()
        {
            return View(await service.GetStudentNotes(User));
        }
    }
}
