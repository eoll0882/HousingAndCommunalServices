using System;
using System.Collections.Generic;

namespace ProjectService.Models
{
    public partial class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Location { get; set; }
    }
}
