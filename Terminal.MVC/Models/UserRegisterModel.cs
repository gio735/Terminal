using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Terminal.MVC.Models
{
    public class UserRegisterModel
    {
        [MaxLength(30)]
        [MinLength(4)]
        public string UserName { get; set; }
        [EmailAddress]
        [MaxLength(64)]
        public string Email { get; set; }
        [DisallowNull]
        public string Password { get; set; }
    }
}
