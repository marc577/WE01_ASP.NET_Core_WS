using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebEngineering01_ASP.NetCore.Models;
using System;

namespace CollabTodoListBackend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class InitTestDataController:Controller
    {
        private readonly TodoContext _context;

        public InitTestDataController(TodoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Put some testdata into the database.
        /// </summary>
        /// <returns>The test data ids</returns>
        /// <response code="200">If the testdata was succesfully written into database</response>
        /// <response code="400">If users are already defined in databse</response> 
        /// <response code="500">Something went wrong</response>
        [HttpPut]
        public IActionResult Input()
        {
            if(_context.User.Count() == 0){
                var collab = new User
                {
                    LastName = "Collab",
                    FirstName = "User",
                    MailAdress = "col@lab.de",
                    Password = "12345"
                };
                _context.User.Add(collab);
                var admin = new User
                {
                    LastName = "Admin",
                    FirstName = "Sys",
                    MailAdress = "sys@admin.de",
                    Password = "123456"
                };
                _context.User.Add(admin);
                var list = new TodoList()
                {
                    Name = "List42",
                    OwnerID = admin.Id
                };
                _context.TodoList.Add(list);

                list.TodoItems.Add(new TodoItem()
                {
                    Name = "Item1",
                    ListID = list.Id
                });
                var todoListuser = new TodoListUser()
                {
                    CollaboratorID = collab.Id,
                    TodoListID = list.Id
                };
                list.Collaborators.Add(todoListuser);
				_context.TodoList.Add(list);
                _context.SaveChanges();
                var ret = new List<Object>(){
                    admin, collab
                };
                return new ObjectResult(ret);
            }
            return BadRequest();
        }
    }
}
