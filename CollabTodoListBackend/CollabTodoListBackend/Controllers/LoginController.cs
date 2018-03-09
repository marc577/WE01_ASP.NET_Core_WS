using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebEngineering01_ASP.NetCore.Models;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace TodoApi.Controllers
{
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
        ///        "MailAdress": "listID",
        ///        "Password": ownerID
        ///     }
        ///
        /// </remarks>
        /// <returns>JWT Token</returns>
        /// <response code="200">Returns a JWT</response>
        /// <response code="404">If the user was not found</response>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult RequestToken([FromBody] UserRequest user)
        {
            var u = _context.User.FirstOrDefault(us => us.MailAdress.Equals(user.MailAdress));
            if (u != null){
                if (u.Password == user.Password)
                {
                    var claims = new[]
                    {
                    new Claim(ClaimTypes.Name, user.MailAdress)
                };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: "localhost",
                        audience: "localhost",
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: creds);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token)
                    });
                } 
            }

            return BadRequest("Could not verify username and password");
        }
    }
}