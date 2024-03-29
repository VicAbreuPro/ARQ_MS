﻿using Users.Dtos;
using Microsoft.AspNetCore.Mvc;
using Users.Models;
using Users.Repository;
using Users.Services;
using Microsoft.AspNetCore.Authorization;

namespace Users.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserController(UserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }


        [Route("register")]
        [HttpPost]
        public ActionResult Register([FromBody] CreateUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var checkUsernameAndEmail = _userRepository.CheckUsernameAndEmail(user.Email, user.Username);

            if(checkUsernameAndEmail == 1)
            {
                return BadRequest("Username already registered");
            }

            if (checkUsernameAndEmail == 2)
            {
                return BadRequest("Email already registered");
            }

            var affectedRows = _userRepository.CreateUser(new User
            {
                UserName = user.Username,
                Email = user.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                FullName = user.FullName,
                Country = user.Country,
                Role = user.Role,
            });

            if (affectedRows == 0)
            {
                return StatusCode(500, "Unexpected Error");
            }

            EmailService es = new();

            int sendEmail = es.SendEmail("User Registered with sucess", "Account registration", user.Email);

            if(sendEmail == 0)
            {
                return StatusCode(503, "Email Service Unavailable!");
            }

            return StatusCode(201, "User Registered!");
        }

        [Route("login")]
        [HttpPost]
        public ActionResult Login([FromBody] Login loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var checkAttempts = _userRepository.GetLoginAttempts(loginRequest.Email);

            if(checkAttempts > 5)
            {
                DateTime currentTime = DateTime.Now;

                var attempt = _userRepository.GetLastLoginAttempt(loginRequest.Email);

                if(attempt != null)
                {
                    TimeSpan dif = currentTime - attempt.CreatedAt;

                    Console.WriteLine("Attempt time: " + attempt.CreatedAt.ToString());
                    Console.WriteLine("Current time: " + currentTime.ToString());
                    Console.WriteLine("Dif: " + dif.ToString());


                    if(dif.TotalMinutes <= 5)
                    {
                        return BadRequest("To Many attempts, try again later!");
                    }

                    if (dif.TotalMinutes >= 5)
                    {
                        var removeAttempts = _userRepository.RemoveAllLoginAttempts(loginRequest.Email);

                        if (removeAttempts == 0)
                        {
                            return StatusCode(500, "Unexpected Error! Remove Attempt Login error");
                        }
                    }
                }

                if(attempt == null)
                {
                    return StatusCode(500, "Unexpected Error! Attempt Login error");
                }
            }

            DateTime currentTimeAux = DateTime.Now;

            var loginAttemptInsert = _userRepository.CreateLoginAttempt(new LoginAttempts
            {
                email = loginRequest.Email,
                CreatedAt = currentTimeAux
            });

            if(loginAttemptInsert == 0)
            {
                return StatusCode(500, "Unexpected Error!Insert Attempt Login error");
            }

            var user = _userRepository.GetUserByEmail(loginRequest.Email);
            var token = "";

            if(user != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
                {
                    return BadRequest("Invalid Credentials");
                }

                try
                {
                    TokenService tokenconfig;

                    tokenconfig = new TokenService(_configuration);

                    token = tokenconfig.GenerateToken(user);
                }catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }

            if (user == null)
            {
                return BadRequest("Invalid Credentials");
            }

            return Ok(token);
        }

        [HttpPut("ChangePassword")]
        public ActionResult ChangePassword([FromBody] ChangePassword changePasswordRequest)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var token = "";
                TokenService ts;
                ts = new TokenService(_configuration);

                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    token = authHeader.ToString().Replace("Bearer ", "");
                }

                if (!HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeaderAux))
                {
                    return BadRequest("Invalid Token");
                }

                // Get userId from token
                int userId = ts.GetUserIdFromJwtToken(token);

                var user = _userRepository.GetUserById(userId);

                if(user != null)
                {
                    if (!BCrypt.Net.BCrypt.Verify(changePasswordRequest.OldPassword, user.Password))
                    {
                        return BadRequest("Invalid OldPassword");
                    }
                }

                if(user == null)
                {
                    return NotFound("Invalid User");
                }

                var affectedRows = _userRepository.UpdatePassword(user, changePasswordRequest.NewPassword);

                if(affectedRows == 0)
                {
                    return StatusCode(500, "Unexpected error!");
                }

                EmailService es = new();

                var checkSenEmail = es.SendEmail("Password  for your account was changed with Success, Please login again with the new credentials!", "Password Changed With Success!", user.Email);

                return Ok("Password Changed!");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
