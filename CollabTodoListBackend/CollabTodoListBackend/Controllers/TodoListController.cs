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
                       FirstName = "Sys",
                       MailAdress = "sys@admin.de",
                       Password = "123456"
                   }
               });
               _context.SaveChanges();
            }
        }

        /// <summary>
        /// Returns all TodoLists.
        /// </summary>
        /// <returns>All TodoLists</returns>
        /// <response code="200">Returns all items</response>
        [HttpGet]
        public IEnumerable<TodoList> GetAll()
        {
            return _context.TodoList.ToList();
        }


        /// <summary>
        /// Returns a TodoList.
        /// </summary>
        /// <returns>A TodoLists</returns>
        /// <response code="200">Returns an items</response>
        /// <response code="404">If the id is not found</response>
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
        /// Creates a TodoList.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /TodoList
        ///     {
        ///        "id": 1,
        ///        "name": "List1",
        ///        "owner": USEROBJECT
        ///     }
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly-created TodoList</returns>
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

        /// <summary>
        /// Updates a TodoList.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /TodoList
        ///     {
        ///        "id": 1,
        ///        "name": "List1",
        ///        "owner": USEROBJECT
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <response code="400">If the item is null</response>
        /// <response code="404">If the id is not found</response>
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
        /// <response code="404">If the id is not found</response>
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