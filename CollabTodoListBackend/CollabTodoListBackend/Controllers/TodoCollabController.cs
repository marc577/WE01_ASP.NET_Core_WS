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
        public IEnumerable<TodoList> GetAllUserLists(long id)
        {
            var allLists = _context.TodoList.ToList();
            List<TodoList> result = new List<TodoList>();

            foreach (TodoList list in allLists)
            {
                if (list.Owner.Id == id)
                {
                    result.Add(list);
                }
                else
                {
                    foreach (User collaborator in list.Collaborators)
                    {
                        if (collaborator.Id == id) result.Add(list);
                    }
                }
            }
            return _context.TodoList.ToList();
        }
    }
}
