using DoTheDishesWebservice.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoTheDishesWebservice.DataAccess.Configurations
{
    public class DishesContext : DbContext
    {
        public DishesContext(DbContextOptions<DishesContext> options)
            :base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Home> Homes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasAlternateKey(c => c.Login);
        }
    }
}
