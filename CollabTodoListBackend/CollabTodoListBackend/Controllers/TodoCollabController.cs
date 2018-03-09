using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebEngineering01_ASP.NetCore.Models;

namespace CollabTodoListBackend.Controllers
{
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
        ///        "CollaboratorID": ownerID
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
        public IActionResult Create([FromBody] TodoListUser user)
        {
            
            if (user == null)
            {
                return BadRequest();
            }
            var list = _context.TodoList.First(l => l.Id.Equals(user.TodoListID));
            if(list == null){
                return NotFound();
            }
            var fUser = _context.User.First(u => u.Id.Equals(user.CollaboratorID));
            if(fUser == null){
                return NotFound();
            }
            list.Collaborators.Add(user);

            //fUser.Lists.Add(user);
            _context.SaveChanges();
            //return CreatedAtRoute("GetTodoList", new { id = item.Id }, item);
            return new NoContentResult();
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
        ///        "CollaboratorID": ownerID
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
            if(collaberator == null){
                return NotFound();
            }
            list.Collaborators.Remove(collaberator);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
