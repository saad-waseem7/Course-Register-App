using System.ComponentModel.DataAnnotations;

namespace SCRS.Models
{
    public class Course
    {
        public int CourseID { get; set; }

        [Required]
        [Display(Name = "Course Code")]
        [StringLength(10)]
        public string CourseCode { get; set; }

        [Required]
        [Display(Name = "Course Name")]
        [StringLength(100)]
        public string CourseName { get; set; }

        [Required]
        [Range(1, 6)]
        public int Credits { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
} 