using System.ComponentModel.DataAnnotations;

namespace Terminal.MVC.Models
{
    public class UserConfirmRegistrationModel
    {
        [EmailAddress]
        [MaxLength(64)]
        public string Email { get; set; }
        public string RegistrationToken { get; set; }
    }
}
