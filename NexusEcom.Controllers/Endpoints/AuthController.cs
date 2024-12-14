using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusEcom.Controllers.Services;
using NexusEcom.DataAccess.DataTransferObjects;
using NexusEcom.Utils;

namespace NexusEcom.Controllers.Endpoints
{

    [ApiController]
    [Route("DataTransfer")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new DefaultConfigs.DefaultResponse(
                        DefaultConfigs.STATUS_ERROR,
                        "Request payload is missing.",
                        null,
                        null,
                        false
                    ));
                }

                var token = await _authService.LoginAsync(request);

                if (string.IsNullOrWhiteSpace(token))
                {
                    return StatusCode(500, new DefaultConfigs.DefaultResponse(
                        DefaultConfigs.STATUS_FAIL,
                        DefaultConfigs.ERROR_MESSAGE,
                        null,
                        null,
                        false
                    ));
                }

                return Ok(new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_SUCCESS,
                    "Login successful!",
                    token,
                    null,
                    true
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_ERROR,
                    ex.Message,
                    null,
                    null,
                    false
                ));
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_FAIL,
                    ex.Message,
                    null,
                    null,
                    false
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_FAIL,
                    "An unexpected error occurred.",
                    ex.Message,
                    null,
                    false
                ));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Register(UserDto request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new DefaultConfigs.DefaultResponse(
                        DefaultConfigs.STATUS_ERROR,
                        "Invalid rewuest! Missing a payload?",
                        null,
                        null,
                        false
                    ));
                }

                var result = await _authService.Register(
                    request
                    );

                if (!result)
                {
                    return StatusCode(500, new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_FAIL,
                    DefaultConfigs.ERROR_MESSAGE,
                    null,
                    null,
                    false
                    ));
                }

                return Ok(new DefaultConfigs.DefaultResponse(
                    DefaultConfigs.STATUS_SUCCESS,
                    "Request Processed Successfully!",
                   null,
                   null,
                    res:true
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
