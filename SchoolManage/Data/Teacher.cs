using System.ComponentModel.DataAnnotations;

namespace SchoolManage.Data
{
    public class Teacher
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter Mobile Number")]
        public string MobileNumber { get; set; }
        public int StandardId { get; set; }
        public Standard Standard { get; set; }
    }

}
