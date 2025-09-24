using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.ActivityDtos
{
    internal class UpdateActivityDto
    {

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; } = null!;

        // public Guid ModuleID { get; set; } // Foreign key
        public Guid ActivityTypeID { get; set; } // Foreign key

        // Navigation property
        // public Module Module { get; set; } = null!;
       // public ActivityType ActivityType { get; set; } = null!;


    }
}
