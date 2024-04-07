using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using webapi.Data;
using webapi.Models;

namespace webapi
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly IConfiguration configuration;

        public LoginController(MyContext context,IConfiguration configuration)
        {
            _context = context;
            this.configuration=configuration;
        }

        [HttpGet]
        public async Task<string> Login(string username, string password)
        {
            var user=await _context.User.FirstOrDefaultAsync(x=>x.UserName==username&&x.Password==password);
            if (user==null)
            {
                // 密码错误
                return null;
            }

            var claims = new Claim[]
            {
                new Claim("uid",$"{user.UserId}")
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecurityKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                null,
                DateTime.Now.AddHours(12),
                signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }

        [HttpPost]
        public async Task<ActionResult<User>> Register(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
