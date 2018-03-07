using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CollabTodoListBackendV2.Models;

namespace CollabTodoListBackendV2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //using (var db = new TodoListContext())
            //{
                //db.TodoLists.Add(new TodoList { Name = "List42" });
                //var count = db.SaveChanges();
                //Console.WriteLine("{0} records saved to database", count);

                //Console.WriteLine();
                //Console.WriteLine("All blogs in database:");
                //foreach (var blog in db.Blogs)
                //{
                 //   Console.WriteLine(" - {0}", blog.Url);
                //}
            //}

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
