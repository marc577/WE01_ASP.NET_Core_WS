﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebEngineering01_ASP.NetCore.Models;
using System.IdentityModel.Tokens.Jwt;

namespace CollabTodoListBackend.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/TodoCollab")]
    public class TodoCollabController : Controller
    {
        private readonly TodoContext _context;

        public TodoCollabController(TodoContext context)
        {
            _context = context;
        }



        /// <summary>
        /// Returns all TodoLists for one User incl. CollabedLists.
        /// </summary>
        /// <returns>All TodoLists for one User incl. CollabedLists</returns>
        /// <response code="200">Returns all items</response>
        [HttpGet("{id}", Name = "GetTodoCollabs")]
        public IEnumerable<TodoList> GetAllUserLists(Guid id)
        {
            List<TodoList> result = new List<TodoList>();
            var currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == JwtRegisteredClaimNames.Jti))
            {
                string guidstring = currentUser.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
                if (!id.ToString().Equals(guidstring))
                {
                    return result;
                }
            }else{
                return result;
            }


            foreach (TodoList list in _context.TodoList)
            {
                if (list.OwnerID.Equals(id))
                {
                    result.Add(list);
                }
                else
                {
                    foreach (TodoListUser collaborator in list.Collaborators)
                    {
                        if (collaborator.CollaboratorID.Equals(id)) result.Add(list);
                    }
                }
            }
            foreach(TodoList l in result){
                foreach(TodoItem i in l.TodoItems){
                    if (i.WorkerID != null)
                    {
                        var fuse = _context.User.Find(i.WorkerID);
                        if (fuse != null)
                        {
                            i.Worker = fuse;
                        }
                    }
                }

            }
            return result;
        }

        /// <summary>
        /// Adds an user as collaberator to a list.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /TodoCollab
        ///     {
        ///        "TodoListID": "listID",
        ///        "MailAdress": "mailadresse"
        ///     }
        ///
        /// </remarks>
        /// <returns>NoContentResult</returns>
        /// <response code="204">If the user was successfully added as collab to list</response>
        /// <response code="400">If the user or list was null</response>
        /// <response code="404">If the user or list was not found</response>
        //[HttpPost("{listID}")]
        [HttpPost]
        [ProducesResponseType(typeof(TodoItem), 204)]
        [ProducesResponseType(typeof(TodoItem), 400)]
        [ProducesResponseType(typeof(TodoItem), 404)]
        public IActionResult Create([FromBody] CollabRequest collabRequest)
        {
            
            if (collabRequest == null)
            {
                return BadRequest();
            }
            var list = _context.TodoList.First(l => l.Id.Equals(collabRequest.TodoListID));
            if(list == null){
                return NotFound();
            }
            var fUser = _context.User.First(u => u.MailAdress.Equals(collabRequest.MailAdress));
            if(fUser == null){
                return NotFound();
            }
            if(fUser.Id.Equals(list.OwnerID)){
                return BadRequest();
            }
            var currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == JwtRegisteredClaimNames.Jti))
            {
                string guidstring = currentUser.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
                if(list.OwnerID.ToString().Equals(guidstring)){

                    try{
                        var Col = new TodoListUser() { TodoListID = collabRequest.TodoListID, CollaboratorID = fUser.Id };
                        list.Collaborators.Add(Col);
                        _context.SaveChanges();
                        return new NoContentResult();
                    }catch(InvalidOperationException ex){
                        return BadRequest(ex.Message);
                    }
                }
            }

            return BadRequest();
        }

        /// <summary>
        /// Removes a collaberator from a list.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /TodoCollab
        ///     {
        ///        "TodoListID": "listID",
        ///        "CollaboratorID": "collabID"
        ///     }
        ///
        /// </remarks>
        /// <response code="204">If the collaberator was successfully removed</response>
        /// <response code="400">If the user or list was null</response>
        /// <response code="404">If the collaberator or the list was not found</response>
        [HttpDelete]
        [ProducesResponseType(typeof(TodoItem), 204)]
        [ProducesResponseType(typeof(TodoItem), 400)]
        [ProducesResponseType(typeof(TodoItem), 404)]
        public IActionResult Delete([FromBody] TodoListUser user)
        {
            
            if (user == null)
            {
                return BadRequest();
            }
            var list = _context.TodoList.First(l => l.Id.Equals(user.TodoListID));
            if (list == null)
            {
                return NotFound();
            }
            var fUser = _context.User.First(u => u.Id.Equals(user.CollaboratorID));
            if (fUser == null)
            {
                return NotFound();
            }


            var collaberator = _context.TodoListUser.Find(user.CollaboratorID, user.TodoListID);
            if (collaberator == null)
            {
                return NotFound();
            }

            var currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == JwtRegisteredClaimNames.Jti))
            {
                string guidstring = currentUser.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
                if(list.OwnerID.ToString().Equals(guidstring) || user.CollaboratorID.ToString().Equals(guidstring)){
                    list.Collaborators.Remove(collaberator);
                    _context.SaveChanges();
                    return new NoContentResult();
                }
            }

            return BadRequest();
        }
    }
}
