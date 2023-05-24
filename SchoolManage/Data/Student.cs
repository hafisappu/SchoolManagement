using System.ComponentModel.DataAnnotations;

namespace SchoolManage.Data
{
    public class Student
    {
        public int StudentId { get; set; }
         
        [Required(ErrorMessage = "Enter Student Name")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "Enter Age")]
        [Range(15, int.MaxValue, ErrorMessage = "Age must be above 15")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Select a Standard")]
        [Range(1, int.MaxValue, ErrorMessage = "Select a Standard")]
        public int StandardId { get; set; }

        public Standard Standard { get; set; }
    }

}
