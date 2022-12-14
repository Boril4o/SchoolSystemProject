using SchoolSystem.Core.Models.Student;
using System.Security.Claims;

namespace SchoolSystem.Core.Contracts
{
    public interface IStudentService
    {
        public Task<StudentHomePageStatsViewModel> GetStudentHomePageStats(ClaimsPrincipal currentUser);
    }
}
