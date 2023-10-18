using System.ComponentModel.DataAnnotations;

namespace AutoPoint.ViewModel.UserVM
{
    public class LoginVM
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [StringLength(15,MinimumLength = 8)]
        public string password { get; set; }

        public int keepMeLoged { get; set; }
    }
}
