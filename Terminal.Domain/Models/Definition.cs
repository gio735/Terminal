using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Domain.Utilities;

namespace Terminal.Domain.Models
{
    public class Definition : EntityModel
    {
        public string GeorgianTitle { get; set; }
        public string EnglishTitle { get; set; }
        public string GeorgianContent { get; set; }
        public string EnglishContent { get; set; }
        public DefinitionState DefinitionState { get; set; }
        public User Author { get; set; }
        public int UpvoteCount { get; set; }
        public int DownvoteCount { get; set; }
        public List<Reference> References { get; set; }
        public List<Definition> Similars { get; set; }
    }
}
