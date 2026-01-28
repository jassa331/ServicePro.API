using ServicePro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetCustomersAsync();
    }
}
