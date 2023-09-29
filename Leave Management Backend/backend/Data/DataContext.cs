using backend.Enums;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;

namespace backend.Data
{
    public class DataContext :DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(e => e.Resolvements).WithOne(e => e.Admin);
            modelBuilder.Entity<User>().HasMany(e => e.Requests).WithOne(e => e.User);
            modelBuilder.Entity<Request>().HasOne(e=>e.Resolvement).WithOne(e=>e.Request).HasForeignKey<Resolvement>(e=>e.RequestId).IsRequired(true);

            var user = new User { Id=1, Username = "PersonName", Password = "123", UserType = UserType.Normal };
            var admin = new User { Id=2, Username = "AdminName", Password = "123", UserType = UserType.Admin };
            var request = new Request { Id = 1, ResolvementId=1, UserId = user.Id, StartDate= DateTime.Now, EndDate = DateTime.Now, LeaveType=LeaveType.Annual, Comments="This is for my hella sick vacay B)" };
            var resolvement = new Resolvement { Id = 1, RequestId = request.Id, AdminId = admin.Id, Comments = "Hell yeah B)", IsApproved = true };
            
            modelBuilder.Entity<User>().HasData(user);
            modelBuilder.Entity<User>().HasData(admin);
            modelBuilder.Entity<Request>().HasData(request);
            modelBuilder.Entity<Resolvement>().HasData(resolvement);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=AuraDB;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}
