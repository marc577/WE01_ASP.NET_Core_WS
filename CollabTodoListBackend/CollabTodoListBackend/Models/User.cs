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
        public long Id { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MailAdress { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
