using ServicePro.Core.DTOs;
using ServicePro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.Core.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequestDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task<User> GetProfileAsync(string email);
    }
}

