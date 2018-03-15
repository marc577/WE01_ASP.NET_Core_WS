using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebEngineering01_ASP.NetCore.Models;

namespace TodoApi.Controllers
{
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly TodoContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(TodoContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Login
        ///     {
        ///        "MailAdress": "mailAdress",
        ///        "Password": "password"
        ///     }
        ///
        /// </remarks>
        /// <returns>JWT Token</returns>
        /// <response code="200">Returns a JWT</response>
        /// <response code="404">If the user was not found</response>
        [HttpPost]
        public IActionResult RequestToken([FromBody] UserRequest user)
        {
            var u = _context.User.FirstOrDefault(us => us.MailAdress.Equals(user.MailAdress));
            if (u != null){
                if (u.Password == user.Password)
                {
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, u.LastName),
                        new Claim(JwtRegisteredClaimNames.Email, u.MailAdress),
                        new Claim(JwtRegisteredClaimNames.Jti, u.Id.ToString())
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        audience: _configuration["Jwt:Issuer"],
                        claims: claims,
						expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: creds);
                    var t = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(new
                    {
                        token = t,
                        userID = u.Id,
                        header = "Bearer " + t
                    });
                } 
            }

            return BadRequest("Could not verify username and password");
        }



    }
}