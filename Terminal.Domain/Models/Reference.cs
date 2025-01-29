using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.Domain.Models
{
    public class Reference : EntityModel
    {
        public string GeorgianTitle { get; set; }
        public string EnglishTitle { get; set; }
        public int ReferenceId { get; set; }
    }
}
