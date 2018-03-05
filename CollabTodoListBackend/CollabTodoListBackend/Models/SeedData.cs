using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebEngineering01_ASP.NetCore.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new TodoContext(
                serviceProvider.GetRequiredService<DbContextOptions<TodoContext>>()))
            {
                // Look for any movies.
                if (context.TodoItem.Any())
                {
                    return;   // DB has been seeded
                }

                context.TodoItem.AddRange(
                     new TodoItem
                     {
                         Name = "Do Stuff",
                         List = new TodoList
                         {
                             Name = "Stuff",
                             Owner = new User
                             {
                                 LastName = "Admin",
                                 FirstName = "Sys",
                                 MailAdress = "sys@admin.de",
                                 Password = "123456"
                             }
                         }
                     }

                );
                context.SaveChanges();
            }
        }
    }
}
