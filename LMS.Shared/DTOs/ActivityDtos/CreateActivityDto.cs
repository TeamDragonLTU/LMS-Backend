using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.ActivityDtos
{
    public class CreateActivityDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; } = null!;
        public Guid ActivityTypeId { get; set; }
        public Guid ModuleId { get; set; }
    }
}
