using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebEngineering01_ASP.NetCore.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItem { get; set; }
        public DbSet<TodoList> TodoList { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<TodoListUser> TodoListUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<TodoItem>().HasOne(e => e.List).WithMany(e => e.TodoItems).HasForeignKey(e => e.ListID);
            //modelBuilder.Entity<TodoItem>().ToTable("TodoItem");
            //modelBuilder.Entity<TodoList>().HasMany(e => e.TodoItems).WithOne(e => e.List).HasForeignKey(e => e.ListID);
            //modelBuilder.Entity<TodoList>().ToTable("TodoList");

            //modelBuilder.Entity<User>().ToTable("User");

            modelBuilder.Entity<TodoListUser>().HasKey(t => new { t.CollaboratorID, t.TodoListID });

            /*modelBuilder.Entity<TodoListUser>()
                .HasKey(e => new { e.CollaboratorID, e.TodoListID });
            modelBuilder.Entity<TodoListUser>()
                .HasOne(e => e.Collaborator)
                .WithMany(e => e.Lists)
                .HasForeignKey(e => e.CollaboratorID);
            modelBuilder.Entity<TodoListUser>()
                .HasOne(e => e.Todolist)
                .WithMany(e => e.Collaborators)
                .HasForeignKey(e => e.TodoListID);
            modelBuilder.Entity<TodoListUser>().ToTable("TodoListUser");*/
        }
    }
}
