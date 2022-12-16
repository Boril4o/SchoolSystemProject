using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Infrastructure.Data.Constants
{
    public class DataConstants
    {
        //Subject
        public const int SubjectNameMaxLength = 20;
        public const int SubjectNameMinLength = 3;

        //Group
        public const int GroupNumberMaxLength = 10;
        public const int GroupNumberMinLength = 2;

        public const int GroupMaxPeople = 50;
        public const int GroupMinPeople = 10;

        //User
        public const int UserFirstNameMaxLength = 50;
        public const int UserFirstNameMinLength = 1;

        public const int UserLastNameMaxLength = 50;
        public const int UserLastNameMinLength = 1;

        public const int UserMaxAge = 99;
        public const int UserMinAge = 7;

        public const int UserEmailMaxLength = 320;
        public const int UserEmailMinLength = 3;

        public const int UserPasswordMaxLength = 20;
        public const int UserPasswordMinLength = 5;

        //Note
        public const int NoteTitleMaxLength = 40;
        public const int NoteTitleMinLength = 3;

        public const int NoteDescriptionMaxLength = 300;
        public const int NoteDescriptionMinLength = 3;

        //Teacher
        public const string TeacherMaxSalary = "10000";
        public const string TeacherMinSalary = "800";
    }
}
