using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Services;

namespace SchoolSystem.Areas.Teacher.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ITeacherService service;

        public TeacherController(ITeacherService service)
        {
            this.service = service;
        }

        [Area(TeacherConstants.AreaName)]
        [Authorize(Roles = TeacherConstants.TeacherRoleName)]
        [HttpGet]
        public async Task<IActionResult> AllGroups()
        {
            return View(await service.AllGroups());
        }

        [Area(TeacherConstants.AreaName)]
        [Authorize(Roles = TeacherConstants.TeacherRoleName)]
        [HttpGet]
        public async Task<IActionResult> AllStudents(int id)
        {
            return View(await service.AllStudents(id));
        }
    }
}
