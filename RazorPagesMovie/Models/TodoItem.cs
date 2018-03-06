using System;
using System.Collections.Generic;

namespace RazorPagesMovie.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public Guid ListID { get; set; }
        public TodoList List { get; set; }
        public DateTime Until { get; set; }

    }
}
