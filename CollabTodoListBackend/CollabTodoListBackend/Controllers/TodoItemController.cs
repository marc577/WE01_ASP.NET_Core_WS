using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebEngineering01_ASP.NetCore.Models;
using System;

namespace TodoApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TodoItemController : Controller
    {
        private readonly TodoContext _context;

        public TodoItemController(TodoContext context)
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
        /// Returns all TodoItems.
        /// </summary>
        /// <returns>All TodoItems</returns>
        /// <response code="200">Returns all items</response>
        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return _context.TodoItem.ToList();
        }

        /// <summary>
        /// Returns a TodoItem.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A newly-created TodoItem</returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="404">If the id is not found</response> 
        [HttpGet("{id}", Name = "GetTodoItem")]
        public IActionResult GetById(Guid id)
        {
            var item = _context.TodoItem.FirstOrDefault(t => t.Id.Equals(id));
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
        ///     POST /TodoItem
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true,
        ///        "owner": USEROBJECT
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
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.TodoItem.Add(item);
            var list = _context.TodoList.FirstOrDefault(e => e.Id.Equals(item.ListID));
            list.TodoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodoItem", new { id = item.Id }, item);
        }

        /// <summary>
        /// Updates a TodoItem.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true,
        ///        "owner": USEROBJECT
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <response code="200">Returns no Content</response>
        /// <response code="404">If the id is not found</response> 
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TodoItem item)
        {
            if (item == null || item.Id.Equals(id))
            {
                return BadRequest();
            }

            var todoItem = _context.TodoItem.FirstOrDefault(t => t.Id.Equals(id));
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.IsComplete = item.IsComplete;
            todoItem.Name = item.Name;
            todoItem.Until = item.Until;
            todoItem.List = item.List;

            _context.TodoItem.Update(todoItem);
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// Deletes a specific TodoItem.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Returns no Content</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var todoItem = _context.TodoItem.FirstOrDefault(t => t.Id.Equals(id));
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItem.Remove(todoItem);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}