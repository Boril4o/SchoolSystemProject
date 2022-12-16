using SchoolSystem.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Core.Contracts
{
    public interface IUserService
    {
        public Task<User> GetUser(string username);
    }
}
