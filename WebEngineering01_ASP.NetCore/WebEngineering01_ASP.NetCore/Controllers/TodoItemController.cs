using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebEngineering01_ASP.NetCore.Models;

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
               _context.TodoItem.Add(new TodoItem { Name = "Item1" });
               _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return _context.TodoItem.ToList();
        }

        [HttpGet("{id}", Name = "GetTodoItem")]
        public IActionResult GetById(long id)
        {
            var item = _context.TodoItem.FirstOrDefault(t => t.Id == id);
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
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.TodoItem.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodoItem", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TodoItem item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var todoItem = _context.TodoItem.FirstOrDefault(t => t.Id == id);
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
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todoItem = _context.TodoItem.FirstOrDefault(t => t.Id == id);
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