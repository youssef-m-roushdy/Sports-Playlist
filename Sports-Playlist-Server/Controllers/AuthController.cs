using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sports_Playlist_Server.DTOs;
using Sports_Playlist_Server.Extensions;
using Sports_Playlist_Server.Interfaces;
using Sports_Playlist_Server.Models;
using Sports_Playlist_Server.Validators;


namespace Sports_Playlist_Server.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtService _jwtService;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var registerValidor = new RegisterDtoValidator();
            var validate = await registerValidor.ValidateAsync(registerDto);

            if (!validate.IsValid)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = validate.Errors
                });
            }

            var usersWithEmail = await _userManager.Users
                .Where(u => u.NormalizedEmail == registerDto.Email.ToUpper())
                .ToListAsync();

            if (usersWithEmail.Count > 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = "Email is already registered"
                });
            }

            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.Username,
                Email = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return BadRequest(new
                {
                    Success = false,
                    Error = result.Errors
                });

            return Ok(new
            {
                Message = "User Registered successfully",
                Success = true,
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var registerValidor = new LoginDtoValidator();
            var validate = await registerValidor.ValidateAsync(loginDto);

            if (!validate.IsValid)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = validate.Errors
                });
            }

            var user = await _userManager.Users
                .Where(u => u.NormalizedEmail == loginDto.Email.ToUpper())
                .FirstOrDefaultAsync();

            if (user == null)
                return BadRequest(new
                {
                    Success = false,
                    Error = "Not a registered email"
                });

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
                return BadRequest(new
                {
                    Success = false,
                    Error = "Check if email/password are wrong typed"
                });

            var token = await _jwtService.GenerateToken(user);

            return Ok(new
            {
                Message = "Login successful",
                Success = true,
                Token = token
            });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<Response<CurrentUserDto>>> Me()
        {
            var currentLoggedInUserId = HttpContext.User.GetUserId();

            var currentLoggedInUser = await _userManager.Users
                .SingleOrDefaultAsync(x => x.Id == currentLoggedInUserId.ToString());

            if (currentLoggedInUser == null)
            {
                return NotFound("User not found.");
            }

            var currentUserDto = new CurrentUserDto
            {
                Username = currentLoggedInUser.UserName,
                Email = currentLoggedInUser.Email,
                FirstName = currentLoggedInUser.FirstName,
                LastName = currentLoggedInUser.LastName
            };

            return Ok(currentUserDto);
        }
    }
}