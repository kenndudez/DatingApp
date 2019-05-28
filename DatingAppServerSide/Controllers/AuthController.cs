using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingAppServerSide.Data;
using DatingAppServerSide.Dtos;
using DatingAppServerSide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingAppServerSide.Controllers
{
   
    [Route("api/[controller]")]
  [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _Config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _Config = config;
            _repo = repo;

        }
        [HttpPost("register")]

        public async Task<IActionResult> Register(UserForRegisterDTo userForRegisterDTo)
        {

            // validate Request 


            userForRegisterDTo.Username = userForRegisterDTo.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDTo.Username))
                return BadRequest("Username already exists");

            var userToCreate = new User
            {
                Username = userForRegisterDTo.Username
            };




            return StatusCode(201);


        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTo userForLoginDTo)
        {
            var userfromRepo = await _repo.Login(userForLoginDTo.Username.ToLower(), userForLoginDTo.Passsord);
            if (userfromRepo == null)
                return Unauthorized();


            var claims = new[] {
    new Claim(ClaimTypes.NameIdentifier, userfromRepo.id.ToString()),
    new Claim(ClaimTypes.NameIdentifier, userfromRepo.Username)
         };

            var key = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(_Config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(
                new {
                    token = tokenHandler.WriteToken(token)
                });
            
        }
    }
}