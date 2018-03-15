using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebEngineering01_ASP.NetCore.Models
{
    public class TodoList
    {

        public TodoList()
        {
            TodoItems = new List<TodoItem>();
            Collaborators = new List<TodoListUser>();
        }

        public virtual Guid Id { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [Required]
        [ForeignKey("Owner")]
        public virtual Guid OwnerID { get; set; }

        public virtual User Owner { get; set; }

        public virtual ICollection<TodoItem> TodoItems { get; set; }

        public virtual ICollection<TodoListUser> Collaborators { get; set; }
    }
}
