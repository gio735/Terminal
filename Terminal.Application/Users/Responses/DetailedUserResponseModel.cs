using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Domain;

namespace Terminal.Application.Users.Responses
{
    public class DetailedUserResponseModel : UserResponseModel
    {
        public Rank UserRank { get; set; }
        public Status UserStatus { get; set; }
    }
}
