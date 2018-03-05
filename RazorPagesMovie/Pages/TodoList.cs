using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Models
{
    public class TodoList
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public User Owner { get; set; }

        [DefaultValue(null)]
        public List<TodoItem> TodoItems { get; set; }

        [DefaultValue(null)]
        public List<User> Collaborators { get; set; }
    }
}
