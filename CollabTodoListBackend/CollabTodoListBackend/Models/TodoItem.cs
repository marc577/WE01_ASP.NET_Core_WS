using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebEngineering01_ASP.NetCore.Models
{
    public class TodoItem
    {
        [Key]
        public virtual Guid Id { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [DefaultValue(false)]
        public virtual bool IsComplete { get; set; }

        [Required]
        [ForeignKey("List")]
        public virtual Guid ListID { get; set; }

        [DefaultValue(null)]
        public virtual Guid WorkerID { get; set; }

        [DefaultValue(null)]
        [NotMapped]
        public virtual User Worker { get; set; }

        [DefaultValue(null)]
        public virtual DateTime Until { get; set; }

    }
}
