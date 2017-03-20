using Microsoft.AspNet.Identity.EntityFramework;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Paper_Rock.Models
{
    public class DBContext: IdentityDbContext<Player>
    { 

        public DbSet<Room> Rooms { get; set; }

        public DBContext()
                : base("DefaultConnection")
            {
            }

            public static DBContext Create()
            {
                return new DBContext();
            }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            //modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            //one-to-many 
            //modelBuilder.Entity<Room>()
            //            .HasOptional<Player>(s => s.Player1) // Student entity requires Standard 
            //            .WithMany(s => s.Room)
            //            .HasForeignKey(s => s.Player1Id);
      

            // Standard entity includes many Students entities

        }




    }
}