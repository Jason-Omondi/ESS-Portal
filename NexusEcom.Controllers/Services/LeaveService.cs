using AutoMapper;
using Microsoft.Extensions.Logging;
using NexusEcom.Controllers.Services.Interfaces;
using NexusEcom.Data.DataTransferObjects;
using NexusEcom.Data.Entities;
using NexusEcom.DataAccess.Repositories.Interfaces;

namespace NexusEcom.Controllers.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LeaveService> _logger;

        public LeaveService(
            ILeaveRepository leaveRepository,
            IMapper mapper,
            ILogger<LeaveService> logger)
        {
            _leaveRepository = leaveRepository ?? throw new ArgumentNullException(nameof(leaveRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<LeaveBalanceDto> GetLeaveBalanceAsync(string employeeId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(employeeId))
                    throw new ArgumentException("Employee ID cannot be null or empty.", nameof(employeeId));

                var leaveBalance = await _leaveRepository.GetLeaveBalanceByEmployeeIdAsync(employeeId);

                if (leaveBalance == null)
                {
                    _logger.LogWarning($"Leave balance not found for employee {employeeId}");
                    throw new KeyNotFoundException($"Leave balance not found for employee {employeeId}");
                }

                return _mapper.Map<LeaveBalanceDto>(leaveBalance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving leave balance for employee {employeeId}");
                throw; // Re-throw to allow calling method to handle or propagate
            }
        }

        public async Task<List<LeaveResponseDto>> GetLeaveRequestsByEmployeeIdAsync(string employeeId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(employeeId))
                    throw new ArgumentException("Employee ID cannot be null or empty.", nameof(employeeId));

                var leaves = await _leaveRepository.GetLeaveRequestsByEmployeeIdAsync(employeeId);

                if (leaves == null || leaves.Count == 0)
                {
                    _logger.LogInformation($"No leave requests found for employee {employeeId}");
                    return new List<LeaveResponseDto>(); // Return empty list
                }

                // Map and return
                return _mapper.Map<List<LeaveResponseDto>>(leaves);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving leave requests for employee {employeeId}");
                throw;
            }
        }

        public async Task SubmitLeaveRequestAsync(CreateLeaveDto leaveDto)
        {
            try
            {
                if (leaveDto == null)
                    throw new ArgumentNullException(nameof(leaveDto), "Leave request data cannot be null.");

                ValidateLeaveRequest(leaveDto);

                var leave = _mapper.Map<Leave>(leaveDto);
                await _leaveRepository.AddLeaveRequestAsync(leave);

                _logger.LogInformation($"Leave request submitted successfully for employee {leaveDto.EmployeeId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting leave request");
                throw;
            }
        }

        public async Task UpdateLeaveStatusAsync(int leaveId, string status, string comments = null)
        {
            try
            {
                if (leaveId <= 0)
                    throw new ArgumentException("Invalid leave ID.", nameof(leaveId));

                if (string.IsNullOrWhiteSpace(status))
                    throw new ArgumentException("Status cannot be null or empty.", nameof(status));

                await _leaveRepository.UpdateLeaveStatusAsync(leaveId, status, comments);

                _logger.LogInformation($"Leave request {leaveId} updated to status {status}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating leave request {leaveId}");
                throw;
            }
        }

        public async Task<List<LeaveResponseDto>> GetAllLeaveRequestsAsync()
        {
            try
            {
                var leaves = await _leaveRepository.GetAllLeaveRequestsAsync();

                if (leaves == null || leaves.Count == 0)
                {
                    _logger.LogInformation("No leave requests found!");
                    return new List<LeaveResponseDto>();
                }

                // Map and return
                return _mapper.Map<List<LeaveResponseDto>>(leaves);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving leave requests!");
                throw;
            }
        }

        public async Task<List<LeaveResponseDto>> GetLeavesByCriteriaAsync(
            DateTime startDate,
            DateTime endDate,
            string? employeeId,
            int? leaveTypeId)
        {
            try
            {
                if (startDate > endDate)
                    throw new ArgumentException("Start date must be before or equal to end date.");

                var leaves = await _leaveRepository.GetLeavesByCriteriaAsync(startDate, endDate, employeeId, leaveTypeId);

                if (leaves == null || leaves.Count == 0)
                {
                    _logger.LogInformation("No leave requests found matching the specified criteria");
                    return new List<LeaveResponseDto>();
                }

                return _mapper.Map<List<LeaveResponseDto>>(leaves);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving leaves by criteria");
                throw;
            }
        }
        private void ValidateLeaveRequest(CreateLeaveDto leaveDto)
        {
            if (leaveDto.StartDate > leaveDto.EndDate)
                throw new ArgumentException("Leave start date must be before or equal to end date.");

            //other validations...
        }
    }
}





//{
//    public class LeaveService : ILeaveService
//    {
//        private readonly ILeaveRepository _leaveRepository;
//        private readonly IMapper _mapper;

//        public LeaveService(ILeaveRepository leaveRepository, IMapper mapper)
//        {
//            _leaveRepository = leaveRepository;
//            _mapper = mapper;
//        }

//        async Task<LeaveBalanceDto> GetLeaveBalanceAsync(string employeeId)
//        {
//            var leaveBalance = await _leaveRepository.GetLeaveBalanceByEmployeeIdAsync(employeeId);

//            if (leaveBalance == null)
//                throw new KeyNotFoundException("Leave balance not found for the specified employee.");

//            return _mapper.Map<LeaveBalanceDto>(leaveBalance);
//        }

//       async Task<List<LeaveResponseDto>> GetLeaveRequestsByEmployeeIdAsync(string employeeId)
//        {
//            var leaves = await _leaveRepository.GetLeaveRequestsByEmployeeIdAsync(employeeId);

//            return _mapper.Map<List<LeaveResponseDto>>(leaves);
//        }

//        async Task SubmitLeaveRequestAsync(LeaveResponseDto leaveDto)
//        {
//            if (leaveDto == null)
//                throw new ArgumentNullException(nameof(leaveDto), "Leave request data cannot be null.");

//            var leave = _mapper.Map<Leave>(leaveDto);
//            await _leaveRepository.AddLeaveRequestAsync(leave);
//        }

//         async Task UpdateLeaveStatusAsync(int leaveId, string status, string comments = null)
//        {
//            if (string.IsNullOrEmpty(status))
//                throw new ArgumentException("Status cannot be null or empty.", nameof(status));

//            await _leaveRepository.UpdateLeaveStatusAsync(leaveId, status, comments);
//        }

//        async Task<List<LeaveResponseDto>> GetAllLeaveRequestsAsync()
//        {
//            var leaves = await _leaveRepository.GetAllLeaveRequestsAsync();

//            return _mapper.Map<List<LeaveResponseDto>>(leaves);
//        }

//         async Task<List<LeaveResponseDto>> GetLeavesByCriteriaAsync(DateTime startDate, DateTime endDate, string? employeeId, int? leaveTypeId)
//        {
//            var leaves = await _leaveRepository.GetLeavesByCriteriaAsync(startDate, endDate, employeeId, leaveTypeId);

//            return _mapper.Map<List<LeaveResponseDto>>(leaves);
//        }


//    }
//}
