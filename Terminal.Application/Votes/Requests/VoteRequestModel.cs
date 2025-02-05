using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.Application.Votes.Requests
{
    public class VoteRequestModel
    {
        public int DefinitionId { get; set; }
        public bool? Likes { get; set; }
    }
}
