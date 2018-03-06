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

            if (_context.TodoItem.Count() == 0)
            {
                new User
                {
                    LastName = "Collab",
                    FirstName = "User",
                    MailAdress = "col@lab.de",
                    Password = "12345"
                };
                new User
                {
                    LastName = "Admin",
                    FirstName = "Sys",
                    MailAdress = "sys@admin.de",
                    Password = "123456"
                };



                _context.TodoItem.Add(new TodoItem
                {
                    Name = "Do Stuff",
                    List = new TodoList
                    {
                        Name = "Stuff",
                        Owner = _context.User.FirstOrDefault(e=> e.FirstName.Equals("Sys")),
                        //Collaborators = collaborators
                    }
                });
                _context.SaveChanges();
                ICollection<TodoListUser> collaborators = new List<TodoListUser>();
                collaborators.Add(new TodoListUser
                {
                    Collaborator = _context.User.FirstOrDefault(e => e.LastName.Equals("Collab")),
                    Todolist = _context.TodoList.FirstOrDefault(e => e.Name.Equals("Stuff"))
                });
            }
        }



        /// <summary>
        /// Returns all TodoLists for one User incl. CollabedLists.
        /// </summary>
        /// <returns>All TodoLists for one User incl. CollabedLists</returns>
        /// <response code="200">Returns all items</response>
        [HttpGet("{id}", Name = "GetTodoCollabs")]
        public IEnumerable<TodoList> GetAllUserLists(Guid id)
        {
            var allLists = _context.TodoList.ToList();
            List<TodoList> result = new List<TodoList>();

            foreach (TodoList list in allLists)
            {
                if (list.Owner.Id.Equals(id))
                {
                    result.Add(list);
                }
                else
                {
                    foreach (TodoListUser collaborator in list.Collaborators)
                    {
                        //if (collaborator.Id.Equals(id)) result.Add(list);
                    }
                }
            }
            return _context.TodoList.ToList();
        }
    }
}
