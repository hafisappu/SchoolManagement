using System.ComponentModel.DataAnnotations;

namespace SchoolManage.Data
{
    public class Standard
    {
        public int StandardId { get; set; }

        [Required(ErrorMessage = "Enter A Standard")]
        public string StandardName { get; set; }
    }

}
