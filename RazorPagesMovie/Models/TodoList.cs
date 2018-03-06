using System;
using System.Collections.Generic;

namespace RazorPagesMovie.Models
{
    public class TodoList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid OwnerID { get; set; }
        public User Owner { get; set; }
        public ICollection<TodoItem> TodoItems { get; set; }
        public ICollection<TodoListUser> Collaborators { get; set; }
    }
}
