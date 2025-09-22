using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.ActivityTypeDto
{
    public class ActivityTypeDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
