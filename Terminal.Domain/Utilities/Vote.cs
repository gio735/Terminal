using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Domain.Models;

namespace Terminal.Domain.Utilities
{
    public class Vote
    {
        public int UserId { get; set; }
        public int DefinitionId { get; set; }
        public bool? Likes { get; set; }
    }
}
