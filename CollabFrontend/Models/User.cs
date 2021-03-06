﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPagesMovie.Models
{
    public class User
    {
        
        public User(){
            //Lists = new List<TodoListUser>();
        }
        public virtual Guid Id { get; set; }

        [Required]
        public virtual string LastName { get; set; }

        [Required]
        public virtual string FirstName { get; set; }

        [Required]
        public virtual string MailAdress { get; set; }

        [Required]
        public virtual string Password { get; set; }

        //public virtual ICollection<TodoListUser> Lists { get; } = new List<TodoListUser>();
    }

    [NotMapped]
    public class UserRequest {

        [Required]
        public virtual string MailAdress { get; set; }

        [Required]
        public virtual string Password { get; set; }
    }

}
