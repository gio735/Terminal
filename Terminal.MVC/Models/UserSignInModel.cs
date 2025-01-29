using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Terminal.MVC.Models
{
    public class UserSignInModel
    {
        [EmailAddress]
        [MaxLength(64)]
        public string Mail { get; set; }
        [DisallowNull]
        public string Password { get; set; }
    }
}
