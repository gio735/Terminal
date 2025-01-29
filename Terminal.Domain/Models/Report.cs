using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.Domain.Models
{
    public class Report : EntityModel
    {
        public string Content { get; set; }
        public int DefinitionId { get; set; }
        public ReportState ReportState { get; set; }
        public User Author { get; set; }
        public int? RevieverId { get; set; }
    }
}
