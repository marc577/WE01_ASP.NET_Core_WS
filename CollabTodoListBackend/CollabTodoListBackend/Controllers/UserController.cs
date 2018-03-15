using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebEngineering01_ASP.NetCore.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace TodoApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]

    public class UserController : Controller
    {
        private readonly TodoContext _context;

        public UserController(TodoContext context)
        {
            _context = context;
        }

        private bool isUser(User us)
        {
            if (us == null) return false;
            var currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == JwtRegisteredClaimNames.Jti))
            {
                string guidstring = currentUser.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
                if (us.Id.ToString().Equals(guidstring))
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Returns a User.
        /// </summary>
        /// <returns>A User</returns>
        /// <response code="200">Returns an items</response>
        /// <response code="404">If the id is not found</response>
        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetById(Guid id)
        {
            var item = _context.User.FirstOrDefault(t => t.Id.Equals(id));
            if (item == null)
            {
                return NotFound();
            }
            if(isUser(item)){
                item.Password = "*";
                return new ObjectResult(item);
            }

            return Unauthorized();
        }

        /// <summary>
        /// Creates a User.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /User
        ///     {
        ///        "lastName": "Last",
        ///        "firstName": "First",
        ///        "mailAdress" : "first@last.tld",
        ///        "password" : "SafePass"
        ///     }
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly-created User</returns>
        /// <response code="201">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(TodoItem), 201)]
        [ProducesResponseType(typeof(TodoItem), 400)]
        public IActionResult Create([FromBody] User item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.User.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetUser", new { id = item.Id }, item);
        }

        /// <summary>
        /// Updates a User.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /User
        ///     {
        ///        "lastName": "Last",
        ///        "FirstName": "First",
        ///        "mailAdress" : "first@last.tld",
        ///        "password" : "SafePass"
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <response code="200">If the item was successfully updated</response>
        /// <response code="400">If the item is null</response>
        /// <response code="404">If the id was not found</response>
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] User item)
        {
            if (item == null || item.Password == null)
            {
                return BadRequest();
            }

            var user = _context.User.FirstOrDefault(t => t.Id.Equals(id));
            if (user == null)
            {
                return NotFound();
            }
            if(isUser(user)){
                user.LastName = item.LastName;
                user.MailAdress = item.MailAdress;
                user.FirstName = item.FirstName;
                user.Password = item.Password;
                _context.User.Update(user);
                _context.SaveChanges();
                user.Password = "*";
                return new OkObjectResult(user);
            }
            return Unauthorized();
        }

        /// <summary>
        /// Deletes a User.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204">If the user was successfully deleted</response>
        /// <response code="400">If the item is null</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var user = _context.User.FirstOrDefault(t => t.Id.Equals(id));
            if (User == null)
            {
                return NotFound();
            }
            if(isUser(user)){
                _context.User.Remove(user);
                _context.SaveChanges();
                return new NoContentResult();
            }
            return Unauthorized();
        }
    }
}