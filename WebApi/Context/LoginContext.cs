using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Mappings;
using WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Context
{
    public class LoginContext :DbContext
    {
        public LoginContext(DbContextOptions<LoginContext> options) : base(options)
        {
            
        }


        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());

        }
    }
}