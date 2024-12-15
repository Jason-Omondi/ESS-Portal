
using Microsoft.EntityFrameworkCore;
using NexusEcom.Data.Context;
using NexusEcom.Data.Entities;
using NexusEcom.DataAccess.Repositories.Interfaces;

namespace NexusEcom.Data.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly AppDbContext _context;

        public LeaveRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LeaveBalance> GetLeaveBalanceByEmployeeIdAsync(string employeeId)
        {
            try
            {
                return await _context.LeaveBalances
                    .Include(lb => lb.Leaves)
                    .FirstOrDefaultAsync(lb => lb.EmployeeId == employeeId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving leave balance: {ex.Message}");
                return null;
            }
        }

        public async Task AddLeaveRequestAsync(Leave leave)
        {
            try
            {
                if (leave == null)
                    throw new ArgumentNullException(nameof(leave), "Leave object cannot be null");

                await _context.Leaves.AddAsync(leave);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                Console.Error.WriteLine($"Error while adding leave request: {dbEx.Message}");
                throw new Exception("Ngori. Please try again.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error adding leave request: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Leave>> GetLeaveRequestsByEmployeeIdAsync(string employeeId)
        {
            try
            {
                return await _context.Leaves
                    .Where(l => l.EmployeeId == employeeId)
                    .OrderByDescending(l => l.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving leave requests: {ex.Message}");
                return new List<Leave>();
            }
        }

        public async Task<List<Leave>> GetAllLeaveRequestsAsync()
        {
            try
            {
                return await _context.Leaves
                    .Include(l => l.LeaveBalance)
                    .OrderByDescending(l => l.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            { 
                Console.Error.WriteLine($"Error retrieving all leave requests: {ex.Message}");
                return new List<Leave>();
            }
        }

        public async Task<Leave> GetLeaveRequestByIdAsync(int leaveId)
        {
            try
            {
                var leave = await _context.Leaves
                    .Include(l => l.LeaveBalance)
                    .FirstOrDefaultAsync(l => l.LeaveId == leaveId);

                if (leave == null)
                    throw new KeyNotFoundException("Leave request not found.");

                return leave;
            }
            catch (KeyNotFoundException knfEx)
            {
                // Log not found errors
                Console.Error.WriteLine(knfEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                // Log general errors
                Console.Error.WriteLine($"Error retrieving leave request: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateLeaveStatusAsync(int leaveId, string status, string comments)
        {
            try
            {
                var leave = await _context.Leaves.FirstOrDefaultAsync(l => l.LeaveId == leaveId);

                if (leave == null)
                    throw new KeyNotFoundException("Leave request not found.");

                leave.Status = status;
                leave.UpdatedAt = DateTime.UtcNow;

                _context.Leaves.Update(leave);
                await _context.SaveChangesAsync();
            }
            catch (KeyNotFoundException knfEx)
            {
                // Log not found errors
                Console.Error.WriteLine(knfEx.Message);
                throw;
            }
            catch (DbUpdateException dbEx)
            {
                // Log database errors
                Console.Error.WriteLine($"Database error while updating leave status: {dbEx.Message}");
                throw new Exception("An error occurred while updating the leave status. Please try again.");
            }
            catch (Exception ex)
            {
                // Log general errors
                Console.Error.WriteLine($"Error updating leave status: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Leave>> GetLeavesByCriteriaAsync(DateTime startDate, DateTime endDate, string? employeeId, int? leaveTypeId)
        {
            try
            {
                var query = _context.Leaves.AsQueryable();

                query = query.Where(l => l.StartDate >= startDate && l.EndDate <= endDate);

                if (employeeId != null)
                    query = query.Where(l => l.EmployeeId == employeeId);

                if (leaveTypeId.HasValue)
                    query = query.Where(l => l.LeaveTypeId == leaveTypeId);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log error here
                Console.Error.WriteLine($"Error retrieving leaves by criteria: {ex.Message}");
                return new List<Leave>();
            }
        }
    }
}

