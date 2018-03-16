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
    public class TodoListController : Controller
    {
        private readonly TodoContext _context;

        public TodoListController(TodoContext context)
        {
            _context = context;
        }

        private bool isOwner(TodoList list)
        {
            if (list == null) return false;
            var currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == JwtRegisteredClaimNames.Jti))
            {
                string guidstring = currentUser.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
                if (list.OwnerID.ToString().Equals(guidstring))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a TodoList.
        /// </summary>
        /// <returns>A TodoLists</returns>
        /// <response code="200">Returns an items</response>
        /// <response code="404">If the id is not found</response>
        [HttpGet("{id}", Name = "GetTodoList")]
        public IActionResult GetById(Guid id)
        {
            var item = _context.TodoList.FirstOrDefault(t => t.Id.Equals(id));
            if (item == null)
            {
                return NotFound();
            }
            foreach (TodoItem i in item.TodoItems)
            {
                if (i.WorkerID != null)
                {
                    var fuse = _context.User.Find(i.WorkerID);
                    if (fuse != null)
                    {
                        i.Worker = fuse;
                    }
                }
            }
            if(isOwner(item)){
                return new ObjectResult(item);
            }
            return Unauthorized();
        }

        /// <summary>
        /// Creates a TodoList.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /TodoList
        ///     {
        ///        "name": "List1",
        ///        "ownerID": ownerID
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
        ///        "name": "List1"
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns>The updated Item</returns>
        /// <response code="200">If the item was succesfully updated</response>
        /// <response code="400">If the item is null</response>
        /// <response code="404">If the id is not found</response>
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] TodoList item)
        {
            if (item == null || !item.Id.Equals(id) || item.Name == null)
            {
                return BadRequest();
            }

            var todoList = _context.TodoList.FirstOrDefault(t => t.Id.Equals(id));
            if (todoList == null)
            {
                return NotFound();
            }

            if(isOwner(todoList)){
                todoList.Name = item.Name;

                _context.TodoList.Update(todoList);
                _context.SaveChanges();
                return new OkObjectResult(todoList);
            }
            return Unauthorized();

        }

        /// <summary>
        /// Deletes a specific TodoItem.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204">If the user was successfully deleted</response>
        /// <response code="404">If the id is not found</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var todoList = _context.TodoList.FirstOrDefault(t => t.Id.Equals(id));
            if (todoList == null)
            {
                return NotFound();
            }

            if(isOwner(todoList)){
                _context.TodoList.Remove(todoList);
                _context.SaveChanges();
                return new NoContentResult();
            }
            return Unauthorized();

        }
    }
}