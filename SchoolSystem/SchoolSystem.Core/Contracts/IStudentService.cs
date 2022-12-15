using SchoolSystem.Core.Models.Note;
using SchoolSystem.Core.Models.Student;
using System.Security.Claims;

namespace SchoolSystem.Core.Contracts
{
    public interface IStudentService
    {
        public Task<StudentHomePageStatsViewModel> GetStudentHomePageStats(ClaimsPrincipal currentUser);

        public Task<IEnumerable<StudentGradesViewModel>> GetStudentGrades(ClaimsPrincipal currentUser);

        public Task<IEnumerable<NoteViewModel>> GetStudentNotes(ClaimsPrincipal currentUser);
    }
}
