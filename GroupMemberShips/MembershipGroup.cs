using System;
using System.Collections.Generic;

namespace GroupMemberShips
{
    public record MembershipGroup(Guid Id, string Name, IReadOnlyCollection<MembershipIdentity> Members)
        : MembershipIdentity(Id, Name) ;
}