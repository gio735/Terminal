using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Domain.Utilities;

namespace Terminal.Domain.Models
{
    public class User : EntityModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Rank UserRank { get; set; }
        public Status UserStatus { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Report> Reports { get; set; }
    }
}
