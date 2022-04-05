using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestIdentityServer.AuthServer.Models
{
    public class CustomDbContext:DbContext
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> opts):base(opts)
        {

        }

        public DbSet<CustomUser> CustomUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomUser>().HasData(
                new CustomUser()
            {

                Id = 1,
                Email = "uyaralp71@gmail.com",
                Password = "password",
                City = "Ankara",
                UserName = "baturalp"
            }, 
                new CustomUser()
            {

                Id = 2,
                Email = "uyaralp72@gmail.com",
                Password = "password",
                City = "Ankara",
                UserName = "baturalp1"
            }


            );
        }
    }
}
