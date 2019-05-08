using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreTest1.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreTest1.Data
{
    public class RocketContext : DbContext
    {
        public RocketContext(DbContextOptions<RocketContext> options) : base(options)
        {
        }

        public DbSet<Position> Positions { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Left> Lefts { get; set; }
        public DbSet<PartType> PartTypes { get; set; }
        public DbSet<PartInContract> ContractItems { get; set; }
    }
}
