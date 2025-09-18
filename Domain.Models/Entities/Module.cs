using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    public class Module
    {
       
        public int Id { get; set; }
  
        [StringLength(100)]
        public required string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public required string Description { get; set; }= string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Foreign key
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();

    }
}
