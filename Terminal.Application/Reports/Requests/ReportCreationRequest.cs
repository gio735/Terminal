using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.Application.Reports.Requests
{
    public class ReportCreationRequest
    {
        public string Content { get; set; }
        public int DefinitionId { get; set; }
    }
}
