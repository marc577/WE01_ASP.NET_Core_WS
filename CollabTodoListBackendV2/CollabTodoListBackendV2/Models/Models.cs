using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollabTodoListBackendV2.Models
{
    public class TodoListContext : DbContext
    {
        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }

        public TodoListContext(DbContextOptions<TodoListContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=todolist.db");
            //optionsBuilder.UseLazyLoadin();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>().HasOne(e => e.List).WithMany(e => e.Items).HasForeignKey(e => e.TodoListId);
        }
    }

    public class TodoList
    {
        public TodoList(){
            Items = new List<TodoItem>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<TodoItem> Items { get; set; }
    }

    public class TodoItem
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey("List")]
        public Guid TodoListId { get; set; }
        public TodoList List { get; set; }
    }
}