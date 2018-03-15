using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebEngineering01_ASP.NetCore.Models
{
    public class User
    {       
        public User(){
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
    }

    [NotMapped]
    public class UserRequest {
        [Required]
        public virtual string MailAdress { get; set; }

        [Required]
        public virtual string Password { get; set; }
    }

}
