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
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [ForeignKey("Owner")]
        public Guid OwnerID { get; set; }

        [Required]
        public User Owner { get; set; }


        //[DefaultValue(List<TodoItem>)]
        public List<TodoItem> TodoItems { get; set; }
        

        //[DefaultValue(null)]
        public ICollection<TodoListUser> Collaborators { get; set; }
    }
}
