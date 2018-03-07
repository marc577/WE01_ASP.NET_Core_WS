using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CollabTodoListBackendV2.Models;

namespace CollabTodoListBackendV2.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TodoListController:Controller
    {
        private readonly TodoListContext _context;

        public TodoListController(TodoListContext context)
        {
            _context = context;
			if(_context.TodoLists.Count() == 0){
                var to = new TodoList { Name = "List42" };
                to.Items.Add(new TodoItem{Name = "Item1"});
                _context.TodoLists.Add(to);
                _context.SaveChanges();
                //Console.WriteLine("{0} records saved to database", count);
            }
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<TodoList> GetAll()
        {
            //var blog = _context.TodoLists.Single(b => b.Id == 1);



            var blogs = _context.TodoLists.Include(blog => blog.Items).ToList();
            return blogs;

            //_context.Entry(blog)
                //.Reference(b => b.)
                //.Load();
            //_context.TodoItems.
			//return _context.TodoLists.ToList<TodoList>(); 
   //         return _context.TodoLists.Include(g => g.Teams).Include(g => g.Teams.TeamsList.Select(t => t.Players)).ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

    }
}
