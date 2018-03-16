using Microsoft.EntityFrameworkCore;

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
            modelBuilder.Entity<TodoListUser>().HasKey(t => new { t.CollaboratorID, t.TodoListID });
        }
    }
}
