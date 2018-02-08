using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace may222017.Models
{
    public class myDbContext : IdentityDbContext<ApplicationUser>
    {
        public myDbContext():base("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }

        public DbSet<Places> Places { get; set; }
        public DbSet<ImageLinks> Links { get; set; }
        
    }
}