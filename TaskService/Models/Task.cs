using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskService.Models
{
    public partial class Task
    {
        [Key]
        public int TaskId { get; set; }
        public string TaskShortName { get; set; }
        public string TaskDescription { get; set; }
        public DateTime? DeadLine { get; set; }
        public bool? IsCompleted { get; set; }
        public int? Executor { get; set; }
        public int? ProjectId { get; set; }
    }
}
