using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebEngineering01_ASP.NetCore.Models;

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

            if (_context.User.Count() == 0)
            {
                _context.User.Add(new User
                {
                    LastName = "Admin",
                    SurName = "Sys",
                    MailAdress = "sys@admin.de",
                    Password = "123456"

               });
               _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<User> GetAll()
        {
            return _context.User.ToList();
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetById(long id)
        {
            var item = _context.User.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        /// <summary>
        /// Creates a TodoItem.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly-created TodoItem</returns>
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

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] User item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var user = _context.User.FirstOrDefault(t => t.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            user.LastName = item.LastName;
            user.MailAdress = item.MailAdress;
            user.SurName = item.Password;
            user.Password = item.Password;

            _context.User.Update(user);
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// Deletes a specific TodoItem.
        /// </summary>
        /// <param name="id"></param>  
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var user = _context.User.FirstOrDefault(t => t.Id == id);
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