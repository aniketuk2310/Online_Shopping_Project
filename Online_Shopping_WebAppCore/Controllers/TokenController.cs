using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Online_Shopping_WebAPICore.Models;

namespace Online_Shopping_WebAPICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly Online_Shopping_DbContext _context;

        public TokenController(Online_Shopping_DbContext context)
        {
            _context = context;
        }

        // GET: api/Token
        //[HttpGet]
        private async Task<User> GetUser(string email,string password)
        {
            //return await _context.Users.ToListAsync();
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        [HttpGet]
        public async Task<User> GetUser(int id)
        {
            //return await _context.Users.ToListAsync();
            return await _context.Users.FirstOrDefaultAsync(u => u.UId == id);
        }

        [HttpPost]
        public async Task<IActionResult> Post(LoginDto userinfo)
        {
            if (userinfo != null && userinfo.Email != null && userinfo.Password != null)
            {
                var user = await GetUser(userinfo.Email, userinfo.Password);
                if (user != null)
                {
                    var claims = new Claim[] { new Claim(ClaimTypes.Name, user.FName),new Claim(ClaimTypes.Role,user.role), new Claim(ClaimTypes.SerialNumber, user.UId.ToString()) };

                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MyNameisAniketKale"));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                            issuer: "DemoAuthority",
                            audience: "ClientProject",
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials: credentials,
                            claims: claims
                        );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid Credentials");
                }
            }
            else
            {
                return BadRequest("Please provide Username and Password");
            }
        }
    }
}
