using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServicePro.Core.Entities;

namespace ServicePro.Core.Interfaces
{
    public interface IAuthRepository
    {
        Task RegisterUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
    }
}

