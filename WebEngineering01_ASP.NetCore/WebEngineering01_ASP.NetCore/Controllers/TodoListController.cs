using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebEngineering01_ASP.NetCore.Models;

namespace TodoApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TodoListController : Controller
    {
        private readonly TodoContext _context;

        public TodoListController(TodoContext context)
        {
            _context = context;

            if (_context.TodoList.Count() == 0)
            {
               _context.TodoList.Add(new TodoList {
                   Name = "List1",
                   Owner = new User
                   {
                       LastName = "Admin",
                       SurName = "Sys",
                       MailAdress = "sys@admin.de",
                       Password = "123456"
                   }
               });
               _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<TodoList> GetAll()
        {
            return _context.TodoList.ToList();
        }

        [HttpGet("{id}", Name = "GetTodoList")]
        public IActionResult GetById(long id)
        {
            var item = _context.TodoList.FirstOrDefault(t => t.Id == id);
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
        public IActionResult Create([FromBody] TodoList item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.TodoList.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodoList", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TodoList item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var todoList = _context.TodoList.FirstOrDefault(t => t.Id == id);
            if (todoList == null)
            {
                return NotFound();
            }

            todoList.Name = item.Name;
            todoList.Owner = item.Owner;
            todoList.TodoItems = item.TodoItems;
            todoList.Collaborators = item.Collaborators;

            _context.TodoList.Update(todoList);
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
            var todoList = _context.TodoList.FirstOrDefault(t => t.Id == id);
            if (todoList == null)
            {
                return NotFound();
            }

            _context.TodoList.Remove(todoList);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}