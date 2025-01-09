using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Login.Data.Mappings;
using API_Login.Entities;
using Microsoft.EntityFrameworkCore;

namespace API_Login.Context
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