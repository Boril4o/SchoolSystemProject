using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.Contracts;
using SchoolSystem.Core.Models.Student;
using static SchoolSystem.Areas.Student.StudentConstants;

namespace SchoolSystem.Areas.Student.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService service;

        public StudentController(IStudentService service)
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

        [Authorize(Roles = StudentRoleName)]
        [Area(AreaName)]
        [HttpGet]
        public async Task<IActionResult> GetUserName()
        {
            StudenUserNameViewMode model = new StudenUserNameViewMode();
            
            return View(model);
        }

        [Authorize(Roles = StudentRoleName)]
        [Area(AreaName)]
        [HttpGet]
        public async Task<IActionResult> ShowStats(StudenUserNameViewMode username)
        {
            StudentStatsViewModel model;
            try
            {
               model =  await service.StudentStats(username.UserName);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", ErrorMessage(e));
                return View(username);
            }

            return View(model);
        }
    }
}
