using System;

namespace RazorPagesMovie.Models
{
    public class TodoListUser
    {
        public Guid CollaboratorID { get; set; }

        public User Collaborator { get; set; }

        public Guid TodoListID { get; set; }

        public TodoList Todolist { get; set; }}
}
