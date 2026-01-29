using ServicePro.Core.Entities;
using Microsoft.EntityFrameworkCore;
using ServicePro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<User> Users { get; set; }

    }
}
