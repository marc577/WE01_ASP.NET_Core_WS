﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebEngineering01_ASP.NetCore.Models
{
    public class TodoItem
    {
        public virtual Guid Id { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [DefaultValue(false)]
        public virtual bool IsComplete { get; set; }

        [Required]
        [ForeignKey("List")]
        public virtual Guid ListID { get; set; }


        [Required]
        public virtual TodoList List { get; set; }

        [DefaultValue(null)]
        public virtual DateTime Until { get; set; }

    }
}
