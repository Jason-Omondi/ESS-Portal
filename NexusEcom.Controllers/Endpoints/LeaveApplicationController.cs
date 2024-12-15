using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NexusEcom.Controllers.Services.Interfaces;
using NexusEcom.Data.DataTransferObjects;
using NexusEcom.Utils;


namespace NexusEcom.Controllers.Endpoints
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class LeaveApplicationController : ControllerBase
    {

        private readonly ILeaveService _leaveService;
        private readonly ILogger _logger;

        public LeaveApplicationController(ILeaveService leaveService, ILogger<LeaveApplicationController> logger)
        {
            _leaveService = leaveService;
            _logger = logger;
        }

        [HttpGet("balance/{employeeId}", Name = "GetLeaveBalance")]
        public async Task<IActionResult> GetLeaveBalance(string employeeId)
        {
            try
            {
                var leaveBalance = await _leaveService.GetLeaveBalanceAsync(employeeId);

                return Ok(new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_SUCCESS,
                    "Request processed successfully!",
                    "",
                    leaveBalance,
                    true
                    ));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_FAIL,
                    DefaultConfigs.ERROR_MESSAGE,
                    ex.Message,
                    null,
                    false
                    ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching leave balance");
                return StatusCode(500, new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_ERROR,
                    DefaultConfigs.ERROR_MESSAGE,
                    ex.Message,
                    null,
                    false
                    ));
            }
        }

        [HttpGet("requests/{employeeId}", Name = "GetLeaveRequestsByEmployee")]
        public async Task<IActionResult> GetLeaveRequests(string employeeId)
        {
            try
            {
                var leaveRequest = await _leaveService.GetLeaveRequestsByEmployeeIdAsync(employeeId);

                return Ok(new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_SUCCESS,
                    "Request processed successfully!",
                    "",
                    leaveRequest,
                    true
                    ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching leave requests");
                return StatusCode(500, new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_FAIL,
                    DefaultConfigs.ERROR_MESSAGE,
                    ex.ToString(),
                    null,
                    false
                    ));
            }
        }

        [HttpPost("submit", Name = "SubmitLeaveRequest")]
        public async Task<IActionResult> SubmitLeaveRequest([FromBody] CreateLeaveDto leaveDto)
        {
            try
            {
                await _leaveService.SubmitLeaveRequestAsync(leaveDto);

                var leavedata = await _leaveService.GetAllLeaveRequestsAsync();

                return Ok(new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_SUCCESS,
                    "Request submitted successfully",
                    "",
                    leavedata,
                    true
                    ));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_ERROR,
                    DefaultConfigs.ERROR_MESSAGE,
                    ex.Message,
                    null,
                    false
                    ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting leave request");
                return StatusCode(500,
                    new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_FAIL,
                    DefaultConfigs.ERROR_MESSAGE,
                    ex.ToString(),
                    null,
                    false
                    ));
            }
        }

        [HttpPut("status/{leaveId}", Name = "UpdateLeaveStatus")]
        public async Task<IActionResult> UpdateLeaveStatus(LeaveResponseDto request)
        {
            try
            {
                await _leaveService.UpdateLeaveStatusAsync(request.LeaveId, request.Status, "");

                var updatedData = await _leaveService.GetLeaveRequestsByEmployeeIdAsync(request.EmployeeId);

                return Ok(new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_SUCCESS,
                    "Request processed successfully!",
                    "",
                    updatedData,
                    true
                    ));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_FAIL,
                    DefaultConfigs.ERROR_MESSAGE,
                    ex.ToString(),
                    null,
                    false
                    ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating leave status");
                return StatusCode(500, new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_FAIL,
                    DefaultConfigs.ERROR_MESSAGE,
                    ex.ToString(),
                    null,
                    false
                    ));
            }
        }

        [HttpGet("all", Name = "GetAllLeaveRequests")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllLeaveRequests()
        {
            try
            {
                var leaveRequest = await _leaveService.GetAllLeaveRequestsAsync();
                return Ok(new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_SUCCESS,
                    "Request processed successfully!",
                    "",
                    leaveRequest,
                    true
                    ));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error fetching all requests!");
                return StatusCode(500, new DefaultConfigs.DefaultResponse(
                        DefaultConfigs.STATUS_ERROR,
                        DefaultConfigs.ERROR_MESSAGE,
                        e.ToString(),
                        null,
                        false
                    ));
            }
        }

        [HttpGet("filter", Name = "GetLeavesByCriteria")]
        public async Task<IActionResult> GetLeavesByCriteria(LeaveResponseDto request)
        {
            try
            {
                var leaveRequest = await _leaveService.GetLeavesByCriteriaAsync(
                    request.StartDate,
                    request.EndDate,
                    request.EmployeeId,
                    request.LeaveTypeId);

                return Ok(new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_SUCCESS,
                    "Request processed successfully!",
                    "",
                    leaveRequest,
                    true
                    ));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_ERROR,
                    DefaultConfigs.ERROR_MESSAGE,
                    ex.ToString(),
                    null,
                    false
                    ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering leave requests");
                return StatusCode(500, new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_FAIL,
                    DefaultConfigs.ERROR_MESSAGE,
                    ex.ToString(),
                    null,
                    false
                    ));
            }
        }
    }
}

