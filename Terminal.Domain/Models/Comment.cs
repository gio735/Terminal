using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.Domain.Models
{
    public class Comment : EntityModel
    {
        public string Content { get; set; }
        public User Author { get; set; }
    }
}
