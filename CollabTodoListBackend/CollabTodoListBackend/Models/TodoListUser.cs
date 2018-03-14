using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebEngineering01_ASP.NetCore.Models
{
    public class TodoListUser
    {

        [Required]
        [ForeignKey("Collaborator")]
        public Guid CollaboratorID { get; set; }

        public virtual User Collaborator { get; set; }

        [Required]
        [ForeignKey("Todolist")]
        public Guid TodoListID { get; set; }

        public virtual TodoList Todolist { get; set; }
    }

    [NotMapped]
    public class CollabRequest
    {
        [Required]
        public virtual Guid TodoListID { get; set; }

        [Required]
        public virtual string MailAdress { get; set; }
    }
}
