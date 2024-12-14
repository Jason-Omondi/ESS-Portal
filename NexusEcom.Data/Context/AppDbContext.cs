using Microsoft.EntityFrameworkCore;
using NexusEcom.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusEcom.DataAccess.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }
        public DbSet<Leave> Leaves { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
               new User
               {
                   userId = 1,
                   Email = "admin@admin.com",
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                   Role = "admin",
                   CreatedAt = DateTime.Now,
               }
               );

            modelBuilder.Entity<LeaveBalance>(entity =>
            {
                entity.HasKey(lb => lb.leaveBalanceId);
                entity.Property(lb => lb.EmployeeId).IsRequired();
                entity.Property(lb => lb.LeaveTypeId).IsRequired();
                entity.Property(lb => lb.TotalEntitlementDays).IsRequired();
                entity.Property(lb => lb.RemainingDays).IsRequired();
                entity.Property(lb => lb.ConsumedDays).IsRequired();
                entity.Property(lb => lb.CarriedForwardDays).IsRequired();
            });

            // Leave Entity Configuration
            modelBuilder.Entity<Leave>(entity =>
            {
                entity.HasKey(l => l.LeaveId);
                entity.Property(l => l.EmployeeId).IsRequired();
                entity.Property(l => l.LeaveTypeId).IsRequired();
                entity.Property(l => l.StartDate).IsRequired();
                entity.Property(l => l.EndDate).IsRequired();
                entity.Property(l => l.Status).HasMaxLength(50).IsRequired();

                // Define relationship with LeaveBalance using composite keys
                entity.HasOne(l => l.LeaveBalance)
          .WithMany(lb => lb.Leaves)
          .HasForeignKey(l => l.leaveBalanceId)
          .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes
            });
        }
    }
}

