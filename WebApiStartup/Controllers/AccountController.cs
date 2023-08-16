using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using WebApiStartup.DTOs;
using WebApiStartup.Interfaces;

namespace WebApiStartup.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly ITokenRepository _tokenRepository;

        public AccountController(UserManager<IdentityUser> userManager, ILogger<AccountController> logger, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _logger = logger;
            _tokenRepository = tokenRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = new MailAddress(registerDto.Email).User,
                Email = registerDto.Email
            };
            var identityResult = await _userManager.CreateAsync(identityUser, registerDto.Password);
            if (identityResult.Succeeded)
            {
                _logger.LogInformation($"A new user ({identityUser.UserName}) register successfully");
                if(registerDto.Roles is not null && registerDto.Roles.Any())
                {
                    identityResult = await _userManager.AddToRolesAsync(identityUser, registerDto.Roles);

                    if(identityResult.Succeeded)
                    {
                        return Ok("Register successfully");
                    }
                }
            }
            _logger.LogError(identityResult.Errors.ToArray().ToString(), identityUser);
            return BadRequest("Something went wrong");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var identityUser = await _userManager.FindByEmailAsync(loginRequestDTO.Email);
            if (identityUser is not null)
            {
                var checkPasswordResult = await _userManager.CheckPasswordAsync(identityUser, loginRequestDTO.Password);
                if (checkPasswordResult)
                {
                    var roles = await _userManager.GetRolesAsync(identityUser);
                    if (roles is not null)
                    {
                        // Create jwt token by calling token repository
                        var jwtToken = _tokenRepository.CreateJWTToken(identityUser, roles.ToList());
                        var response = new LoginResponseDTO
                        {
                            JWTToken = jwtToken
                        };
                        return Ok(response);
                    }
                }
            }
            return BadRequest("Username or Password wrong");
        }
    }
}
