using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.Domain
{
    public abstract class EntityModel
    {
        [Key]
        public int Id { get; set; }
        public State State { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime DeletionDate { get; set; }
    }
}