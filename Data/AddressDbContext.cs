using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brotherscase.Domain
{
    public class AddressDbContext : DbContext
    {
        public DbSet<Address> Address { get; set; }

        public AddressDbContext(DbContextOptions<AddressDbContext> options) : base(options) 
        {
        }       
    }
}
