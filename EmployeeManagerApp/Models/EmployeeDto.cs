using System.ComponentModel.DataAnnotations;

namespace EmployeeManagerApp.Models
{
    public class EmployeeDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string DateOfBirth { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
