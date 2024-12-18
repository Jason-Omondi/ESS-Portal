using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusEcom.Controllers.Services;
using NexusEcom.Controllers.Services.Interfaces;
using NexusEcom.Data.DataTransferObjects;
using NexusEcom.Utils;

namespace NexusEcom.Controllers.Endpoints
{

    [ApiController]
    [Route("DataTransfer")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _authService;
        private readonly IUserService _userService;

        public AuthController(UserService authService, IUserService userService)
        {
            this._authService = authService;
            this._userService = userService;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserDto request)
        {

            var employeeNo = request.EmployeeNumber;
            var password = request.Password;
            try
            {
                if (employeeNo == null)
                {
                    Console.WriteLine($"bad request");
                    return BadRequest(new DefaultConfigs.DefaultResponse(
                        DefaultConfigs.STATUS_ERROR,
                        DefaultConfigs.ERROR_MESSAGE,
                        null,
                        null,
                        false
                        ));
                }

                if (!await _authService.ValidateUserLoginAsync(employeeNo, password))
                {
                    Console.WriteLine($"Failed to validate login user: {employeeNo}");
                    return BadRequest(new DefaultConfigs.DefaultResponse(
                        DefaultConfigs.STATUS_FAIL,
                        DefaultConfigs.ERROR_MESSAGE,
                        null,
                        null,
                        false
                        ));
                }
                //var data = _authService.GetUserByEmail(email);
                var data = _authService.GetByEmpNoAsync(employeeNo);
                var email = data.Result!.Email;
                var token = DefaultConfigs.GenerateToken(email, password);
                return Ok(
                    new DefaultConfigs.DefaultResponse(
                        DefaultConfigs.STATUS_SUCCESS,
                        "Request processed successfully",
                        token,
                        data.Result!,
                        true
                        )
                    );


            }
            catch (Exception ex) 
            { 
                Console.WriteLine($"Failed to login user {employeeNo}", ex.ToString());
                return BadRequest(new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_FAIL,
                    DefaultConfigs.ERROR_MESSAGE,
                    ex.ToString(),
                    null,
                    false
                    ));
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.EmployeeNumber) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest(new DefaultConfigs.DefaultResponse(
                        DefaultConfigs.STATUS_ERROR,
                        "Invalid request! Missing a payload item?",
                        null,
                        null,
                        false
                    ));
                }

                var result = await _authService.RegisterUserAsync(request);

                if (!result)
                {
                    return StatusCode(500, new DefaultConfigs.DefaultResponse(
                        DefaultConfigs.STATUS_FAIL,
                        DefaultConfigs.ERROR_MESSAGE,
                        $"User does not exist or is already registered! {request.EmployeeNumber}",
                        null,
                        false
                    ));
                }

                return Ok(new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_SUCCESS,
                    "User registered successfully!",
                    "",
                    "",
                    true
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_FAIL,
                    DefaultConfigs.ERROR_MESSAGE,
                    ex.Message,
                    null,
                    false
                ));
            }
        }


        [HttpGet("GetAllUsers")]
        [AllowAnonymous]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();

                if (users == null || !users.Any())
                {
                    return NotFound(new DefaultConfigs.DefaultResponse(
                        DefaultConfigs.STATUS_ERROR,
                        "No users found.",
                        null,
                        null,
                        false
                    ));
                }

                return Ok(new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_SUCCESS,
                    "Users retrieved successfully!",
                    "",
                    users,
                    true
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_FAIL,
                    DefaultConfigs.ERROR_MESSAGE,
                    ex.Message,
                    null,
                    false
                ));
            }
        }

        // Update User (Protected)
        [HttpPut("UpdateUser")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto request)
        {
            try
            {
                var result = await _userService.UpdateUserAsync(request.EmployeeNumber, request);

                if (!result)
                {
                    return StatusCode(500, new DefaultConfigs.DefaultResponse(
                        DefaultConfigs.STATUS_FAIL,
                        "Failed to update user.",
                        null,
                        null,
                        false
                    ));
                }

                var data = await _authService.GetUserByEmail(request.Email);

                return Ok(new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_SUCCESS,
                    "User updated successfully!",
                    null,
                    data!,
                    true
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_FAIL,
                    DefaultConfigs.ERROR_MESSAGE,
                    ex.Message,
                    null,
                    false
                ));
            }
        }

        // Delete User (Protected)
        [HttpDelete("DeleteUser/{employeeId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string employeeId)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(employeeId);

                if (!result)
                {
                    return StatusCode(500, new DefaultConfigs.DefaultResponse(
                        DefaultConfigs.STATUS_FAIL,
                        "Failed to delete user.",
                        null,
                        null,
                        false
                    ));
                }

                return Ok(new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_SUCCESS,
                    "User deleted successfully!",
                    null,
                    null,
                    true
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_FAIL,
                    DefaultConfigs.ERROR_MESSAGE,
                    ex.Message,
                    null,
                    false
                ));
            }
        }

    }
}

//[HttpPost("Login")]
//[AllowAnonymous]
//public async Task<IActionResult> Login(LoginDto request)
//{
//    try
//    {
//        if (request == null)
//        {
//            return BadRequest(new DefaultConfigs.DefaultResponse(
//                DefaultConfigs.STATUS_ERROR,
//                "Request payload is missing.",
//                null,
//                null,
//                false
//            ));
//        }

//        var token = await _authService.LoginAsync(request);

//        if (string.IsNullOrWhiteSpace(token))
//        {
//            return StatusCode(500, new DefaultConfigs.DefaultResponse(
//                DefaultConfigs.STATUS_FAIL,
//                DefaultConfigs.ERROR_MESSAGE,
//                null,
//                null,
//                false
//            ));
//        }

//        return Ok(new DefaultConfigs.DefaultResponse(
//            DefaultConfigs.STATUS_SUCCESS,
//            "Login successful!",
//            token,
//            null,
//            true
//        ));
//    }
//    catch (ArgumentException ex)
//    {
//        return BadRequest(new DefaultConfigs.DefaultResponse(
//            DefaultConfigs.STATUS_ERROR,
//            ex.Message,
//            null,
//            null,
//            false
//        ));
//    }
//    catch (InvalidOperationException ex)
//    {
//        return Unauthorized(new DefaultConfigs.DefaultResponse(
//            DefaultConfigs.STATUS_FAIL,
//            ex.Message,
//            null,
//            null,
//            false
//        ));
//    }
//    catch (Exception ex)
//    {
//        return StatusCode(500, new DefaultConfigs.DefaultResponse(
//            DefaultConfigs.STATUS_FAIL,
//            "An unexpected error occurred.",
//            ex.Message,
//            null,
//            false
//        ));
//    }
//}

//[HttpPost]
//[AllowAnonymous]
//[Route("Register")]
//public async Task<IActionResult> Register(UserDto request)
//{
//    try
//    {
//        if (request == null)
//        {
//            return BadRequest(new DefaultConfigs.DefaultResponse(
//                DefaultConfigs.STATUS_ERROR,
//                "Invalid rewuest! Missing a payload?",
//                null,
//                null,
//                false
//            ));
//        }

//        var result = await _authService.Register(
//            request
//            );

//        if (!result)
//        {
//            return StatusCode(500, new DefaultConfigs.DefaultResponse(
//            DefaultConfigs.STATUS_FAIL,
//            DefaultConfigs.ERROR_MESSAGE,
//            null,
//            null,
//            false
//            ));
//        }

//        return Ok(new DefaultConfigs.DefaultResponse(
//            DefaultConfigs.STATUS_SUCCESS,
//            "Request Processed Successfully!",
//           null,
//           null,
//            res: true
//            ));


//    }
//    catch (Exception ex)

//    {
//        return StatusCode(500, new DefaultConfigs.DefaultResponse(
//            DefaultConfigs.STATUS_FAIL,
//            DefaultConfigs.ERROR_MESSAGE,
//            ex.Message,
//            null,
//            false
//            ));
//    }

//}

