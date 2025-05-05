using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sports_Playlist_Server.DTOs;
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
            
            if(!validate.IsValid)
            {
                return BadRequest(validate.Errors);
            }

            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.Email,
                Email = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var token = await _jwtService.GenerateToken(user);

            return Ok(new
            {
                Message = "Registration successful",
                Token = token,
                User = new
                {
                    user.Id,
                    user.Email,
                    user.UserName
                }
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var registerValidor = new LoginDtoValidator();
            var validate = await registerValidor.ValidateAsync(loginDto);
            
            if(!validate.IsValid)
            {
                return BadRequest(validate.Errors);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                return BadRequest("Invalid credentials");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
                return BadRequest("Invalid credentials");

            var token = await _jwtService.GenerateToken(user);

            return Ok(new
            {
                Message = "Login successful",
                Token = token,
                User = new
                {
                    user.Id,
                    user.Email,
                    user.UserName
                }
            });
        }
    }
}