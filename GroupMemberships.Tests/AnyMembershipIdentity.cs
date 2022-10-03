using GroupMemberShips;

namespace GroupMemberships.Tests;

public class AnyMembershipIdentity
{
    private readonly MembershipIdentity _identity;

    public AnyMembershipIdentity()
    {
        var id = Guid.NewGuid();
        _identity = new MembershipIdentity(id, $"Person-{id}");
    }

    public MembershipIdentity Build()
    {
        return this._identity;
    }
}
