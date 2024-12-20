using Microsoft.EntityFrameworkCore;
using NexusEcom.Data.Entities;

namespace NexusEcom.Data.Context
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

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.Property(u => u.EmployeeNumber).IsRequired().HasMaxLength(50);
                entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PasswordHash).IsRequired(false);
                entity.Property(u => u.PhoneNumber).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Role).IsRequired().HasMaxLength(50);
                entity.Property(u => u.CreatedAt).IsRequired();
                entity.Property(u => u.LastLogin).IsRequired(false);
            });

            //seed simulation of 30 users
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Email = "john.mutemi@example.com", EmployeeNumber = "EMP001", FullName = "John Mtemi", Role = "Employee", CreatedAt = DateTime.Now, PhoneNumber = "257769711031" },
                new User { UserId = 2, Email = "jane.awiyo@example.com", EmployeeNumber = "EMP002", FullName = "Jane Awiyo", Role = "Employee", CreatedAt = DateTime.Now, PhoneNumber = "2547769711032" },
                new User { UserId = 3, Email = "admin@admin.com", EmployeeNumber = "EMP000", FullName = "System Admin", Role = "Admin", CreatedAt = DateTime.Now, PhoneNumber = "2547769711000" }
            );

            var names = new[] { "Alice", "Amondi", "Catherine", "David", "Kratos", "Tamre", "Grace", "Henry" };

            for (int i = 4; i <= 30; i++)
            {
                var name = names[(i - 4) % names.Length];
                string phoneNumber = $"2547{i:D7}";
                modelBuilder.Entity<User>().HasData(new User
                {
                    UserId = i,
                    Email = $"{name.ToLower()}{i}@company.co.ke",
                    EmployeeNumber = $"EMP{i:D3}",
                    FullName = $"{name} {i}",
                    Role = "Employee",
                    CreatedAt = DateTime.Now,
                    PhoneNumber = phoneNumber
                });
            }

            modelBuilder.Entity<LeaveBalance>(entity =>
            {
                entity.HasKey(lb => lb.leaveBalanceId);
                entity.Property(lb => lb.EmployeeId).IsRequired(); //employeeNumber
                entity.Property(lb => lb.LeaveTypeId).IsRequired();
                entity.Property(lb => lb.TotalEntitlementDays).IsRequired();
                entity.Property(lb => lb.RemainingDays).IsRequired();
                entity.Property(lb => lb.ConsumedDays).IsRequired();
                entity.Property(lb => lb.CarriedForwardDays).IsRequired();
            });

            int leaveBalanceId = 1;
            var leaveTypes = new[] { 1, 2, 3 };
            for (int i = 1; i <= 30; i++) 
            {
                int consumedDays = new Random().Next(0, 10);
                int carriedForwardDays = new Random().Next(0, 5);
                int remainingDays = 21 - consumedDays + carriedForwardDays;

                modelBuilder.Entity<LeaveBalance>().HasData(new
                LeaveBalance 
                {
                    leaveBalanceId = leaveBalanceId,
                    EmployeeId = $"EMP{i:D3}",
                    LeaveTypeId = leaveTypes[i % leaveTypes.Length],
                    TotalEntitlementDays = 21,
                    ConsumedDays = consumedDays,
                    CarriedForwardDays = carriedForwardDays,
                    RemainingDays = remainingDays
                });
                leaveBalanceId++;
            }

            // Leave Applications Seeding
            modelBuilder.Entity<Leave>(entity =>
            {
                entity.HasKey(l => l.LeaveId);
                entity.Property(l => l.EmployeeId).IsRequired();
                entity.Property(l => l.LeaveTypeId).IsRequired();
                entity.Property(l => l.StartDate).IsRequired();
                entity.Property(l => l.EndDate).IsRequired();
                entity.Property(l => l.Status).HasMaxLength(50).IsRequired();

                entity.HasOne(l => l.LeaveBalance)
                    .WithMany(lb => lb.Leaves)
                    .HasForeignKey(l => l.leaveBalanceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            int leaveId = 1;
            for (int i = 1; i <= 30; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    modelBuilder.Entity<Leave>().HasData(new Leave
                    {
                        LeaveId = leaveId,
                        EmployeeId = $"EMP{i:D3}",
                        leaveBalanceId = i, // 1:1 relationship with LeaveBalance
                        LeaveTypeId = new Random().Next(1, 4), 
                        StartDate = DateTime.Now.AddDays(-10 * j),
                        EndDate = DateTime.Now.AddDays(-10 * j + 5),
                        TotalDays = 5,
                        Status = j % 2 == 0 ? "Pending" : "Approved",
                        CreatedAt = DateTime.Now.AddDays(-10 * j),
                        UpdatedAt = DateTime.Now.AddDays(-10 * j + 1)
                    });
                    leaveId++;
                }
            }
        }
    }
}


/*
 modelBuilder.Entity<Leave>(entity =>
            {
                entity.HasKey(l => l.LeaveId);
                entity.Property(l => l.EmployeeId).IsRequired(); //employeeNumber
                entity.Property(l => l.LeaveTypeId).IsRequired();
                entity.Property(l => l.StartDate).IsRequired();
                entity.Property(l => l.EndDate).IsRequired();
                entity.Property(l => l.Status).HasMaxLength(50).IsRequired();

                entity.HasOne(l => l.LeaveBalance)
          .WithMany(lb => lb.Leaves)
          .HasForeignKey(l => l.leaveBalanceId)
          .OnDelete(DeleteBehavior.Restrict); // Prevents cascading deletes
            });
 */
