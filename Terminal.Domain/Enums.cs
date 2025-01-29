using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.Domain
{
    public enum State
    {
        Active,
        Deleted
    }

    public enum Status
    {
        User,
        Management
    }

    public enum Rank
    {
        User,
        Moderator,
        Manager,
        Administrator,
        Owner
    }
    public enum ReportState
    {
        Pending,
        Approved,
        Rejected
    }
    public enum DefinitionState
    {
        Pending,
        Approved,
        Rejected
    }
}
