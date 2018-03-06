using System;
using System.Collections.Generic;

namespace RazorPagesMovie.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MailAdress { get; set; }
        public string Password { get; set; }
        public ICollection<TodoListUser> Lists { get; set; }
    }
}
