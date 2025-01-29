using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.Application.Definitions.Requests
{
    public class DefinitionCreationRequest
    {
        public string GeorgianTitle { get; set; }
        public string EnglishTitle { get; set; }
        public string GeorgianContent { get; set; }
        public string EnglishContent { get; set; }
    }
}
