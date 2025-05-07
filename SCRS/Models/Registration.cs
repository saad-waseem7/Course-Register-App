using System;
using System.ComponentModel.DataAnnotations;

namespace SCRS.Models
{
    public class Registration
    {
        [Required]
        [Display(Name = "Student")]
        public int StudentID { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseID { get; set; }

        [Display(Name = "Grade")]
        public string Grade { get; set; }

        [Display(Name = "Registration Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RegistrationDate { get; set; }

        // Navigation properties to display names
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }

        [Display(Name = "Course Name")]
        public string CourseName { get; set; }
    }
} 