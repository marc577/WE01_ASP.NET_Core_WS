using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebEngineering01_ASP.NetCore.Models
{
    public class User
    {
        public virtual Guid Id { get; set; }

        [Required]
        public virtual string LastName { get; set; }

        [Required]
        public virtual string FirstName { get; set; }

        [Required]
        public virtual string MailAdress { get; set; }

        [Required]
        public virtual string Password { get; set; }

        public virtual ICollection<TodoListUser> Lists { get; set; }
    }
}
