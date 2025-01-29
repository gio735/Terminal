using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.Infrastructure
{
    public class InexistentEntityException : Exception
    {
        public InexistentEntityException() : base("Requested entity doesn't exist!")
        { }
        public InexistentEntityException(Exception innerEx) : base("Requested entity doesn't exist!", innerEx)
        { }
    }
    public class AttemptToUseDeletedEntityException : Exception
    {
        public AttemptToUseDeletedEntityException() : base("This entity is deleted. Deleted entity can not be used in any ways.")
        { }
        public AttemptToUseDeletedEntityException(Exception innerEx) : base("This entity is deleted. Deleted entity can not be used in any ways.", innerEx)
        { }
    }
    public class ExistingUsernameException : Exception
    {
        public ExistingUsernameException() : base("Username you are trying to register is already in use.")
        { }
        public ExistingUsernameException(Exception innerEx) : base("Username you are trying to register is already in use.", innerEx)
        { }
    }
    public class ExistingEmailException : Exception
    {
        public ExistingEmailException() : base("Email you are trying to register is already in use.")
        { }
        public ExistingEmailException(Exception innerEx) : base("Email you are trying to register is already in use.", innerEx)
        { }
    }
    public class UnauthorizedAttemptToChangeRankException : Exception
    {
        public UnauthorizedAttemptToChangeRankException() : base("Attempt to promote someone to same or higher than one's rank or to demote under minimal rank.")
        { }
        public UnauthorizedAttemptToChangeRankException(Exception innerEx) : base("Attempt to promote someone to same or higher than one's rank or to demote under minimal rank.", innerEx)
        { }
    }
}
