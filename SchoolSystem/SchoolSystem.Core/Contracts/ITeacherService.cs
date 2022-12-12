﻿using SchoolSystem.Core.Models.Grade;
using SchoolSystem.Core.Models.Group;
using SchoolSystem.Core.Models.Student;
using SchoolSystem.Data.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Core.Contracts
{
    public interface ITeacherService
    {
        public Task<IEnumerable<GroupViewModel>> AllGroups();

        public Task<IEnumerable<StudentViewModel>> AllStudentsFromGroup(int groupId);

        public Task AddGrade(AddGradeViewModel model);

        public Task<IEnumerable<Subject>> GetSubjects();
    }
}
