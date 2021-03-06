﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebEngineering01_ASP.NetCore.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace TodoApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TodoItemController : Controller
    {
        private readonly TodoContext _context;

        public TodoItemController(TodoContext context)
        {
            _context = context;
        }

        private bool isUserOrCollab(TodoList list){
            if (list == null) return false;
            var currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == JwtRegisteredClaimNames.Jti))
            {
                string guidstring = currentUser.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
                if (list.OwnerID.ToString().Equals(guidstring))
                {
                    return true;
                }else{
                    var collab = list.Collaborators.FirstOrDefault(col => (col.CollaboratorID.ToString().Equals(guidstring) && col.TodoListID.Equals(list.Id)));
                    if(collab != null){
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// Returns a TodoItem.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A TodoItem</returns>
        /// <response code="200">Returns the item</response>
        /// <response code="404">If the id is not found</response> 
        [HttpGet("{id}", Name = "GetTodoItem")]
        public IActionResult GetById(Guid id)
        {
            var item = _context.TodoItem.FirstOrDefault(t => t.Id.Equals(id));
            if (item == null)
            {
                return NotFound();
            }
            var list = _context.TodoList.Find(item.ListID);
            if(isUserOrCollab(list)){
                return new ObjectResult(item);
            }
            return Unauthorized();
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

            var list = _context.TodoList.FirstOrDefault(e => e.Id.Equals(item.ListID));

            if(isUserOrCollab(list)){
                _context.TodoItem.Add(item);
                list.TodoItems.Add(item);
                _context.SaveChanges();

                return CreatedAtRoute("GetTodoItem", new { id = item.Id }, item);
            }

            return Unauthorized();
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
        public IActionResult Update(Guid id, [FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var todoItem = _context.TodoItem.FirstOrDefault(t => t.Id.Equals(id));
            if (todoItem == null)
            {
                return NotFound();
            }

            var list = _context.TodoList.FirstOrDefault(e => e.Id.Equals(todoItem.ListID));
            if(isUserOrCollab(list)){
                todoItem.IsComplete = item.IsComplete;
                todoItem.Name = item.Name;
                todoItem.Until = item.Until;

                if (!item.WorkerID.Equals(Guid.Empty))
                {
                    var l = _context.TodoList.Find(todoItem.ListID);
                    if (item.WorkerID.Equals(l.OwnerID))
                    {
                        var fuse = _context.User.Find(l.OwnerID);
                        if (fuse != null)
                        {
                            todoItem.WorkerID = fuse.Id;
                            todoItem.Worker = fuse;
                        }
                    }
                    else if (l != null)
                    {
                        foreach (TodoListUser u in l.Collaborators)
                        {
                            if (u.CollaboratorID.Equals(item.WorkerID))
                            {
                                var fuse = _context.User.Find(u.CollaboratorID);
                                if (fuse != null)
                                {
                                    todoItem.WorkerID = fuse.Id;
                                    todoItem.Worker = fuse;
                                }
                            }
                        }
                    }
                }
                else
                {
                    todoItem.Worker = null;
                    todoItem.WorkerID = Guid.Empty;
                }
                _context.TodoItem.Update(todoItem);
                _context.SaveChanges();
                return new OkObjectResult(todoItem);   
            }else{
                return Unauthorized();
            }
        }

        /// <summary>
        /// Deletes a specific TodoItem.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204">Item successfully deleted</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var todoItem = _context.TodoItem.FirstOrDefault(t => t.Id.Equals(id));
            if (todoItem == null)
            {
                return NotFound();
            }

            var list = _context.TodoList.FirstOrDefault(e => e.Id.Equals(todoItem.ListID));
            if(isUserOrCollab(list)){
                _context.TodoItem.Remove(todoItem);
                _context.SaveChanges();
                return new NoContentResult();
            }
            return Unauthorized();


        }
    }
}