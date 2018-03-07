using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebEngineering01_ASP.NetCore.Models;
using System;

namespace TodoApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly TodoContext _context;

        public UserController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItem.Count() == 0)
            {
                List<User> collaborators = new List<User>();
                collaborators.Add(new User
                {
                    LastName = "Collab",
                    FirstName = "User",
                    MailAdress = "col@lab.de",
                    Password = "12345"
                });

                _context.TodoItem.Add(new TodoItem
                {
                    Name = "Do Stuff",
                    List = new TodoList
                    {
                        Name = "Stuff"
                        /*Owner = new User
                        {
                            LastName = "Admin",
                            FirstName = "Sys",
                            MailAdress = "sys@admin.de",
                            Password = "123456"
                        },*/
                        //Collaborators = collaborators
                    }
                });
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Returns all User.
        /// </summary>
        /// <returns>All Users</returns>
        /// <response code="200">Returns all items</response>
        [HttpGet]
        public IEnumerable<User> GetAll()
        {
            return _context.User.ToList();
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
            return new ObjectResult(item);
        }

        /// <summary>
        /// Creates a User.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /User
        ///     {
        ///        "id": 1,
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
        ///        "id": 1,
        ///        "lastName": "Last",
        ///        "FirstName": "First",
        ///        "mailAdress" : "first@last.tld",
        ///        "password" : "SafePass"
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <response code="400">If the item is null</response>
        /// <response code="404">If the id is not found</response>
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] User item)
        {
            if (item == null || !item.Id.Equals(id))
            {
                return BadRequest();
            }

            var user = _context.User.FirstOrDefault(t => t.Id.Equals(id));
            if (user == null)
            {
                return NotFound();
            }

            user.LastName = item.LastName;
            user.MailAdress = item.MailAdress;
            user.FirstName = item.FirstName;
            user.Password = item.Password;

            _context.User.Update(user);
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// Deletes a specific TodoItem.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="400">If the item is null</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var user = _context.User.FirstOrDefault(t => t.Id.Equals(id));
            if (User == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}