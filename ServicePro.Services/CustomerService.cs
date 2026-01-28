using ServicePro.Core.Entities;
using ServicePro.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _repository;

        public CustomerService(IRepository<Customer> repository)
        {
            _repository = repository;
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}
