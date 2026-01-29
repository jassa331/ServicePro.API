using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServicePro.Core.DTOs;

namespace ServicePro.Core.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequestDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }
}

