using System.ComponentModel.DataAnnotations;

namespace tourismApplication.Models
{
    public class AgencyLogin
    {
        [Required(ErrorMessage = "Please enter your email.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
