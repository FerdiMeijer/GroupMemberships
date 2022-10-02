using GroupMemberShips;

namespace GroupMemberships.Tests;

public class AnyPerson
{
    private readonly MembershipIdentity _identity;

    public AnyPerson()
    {
        var id = Guid.NewGuid();
        _identity = new MembershipIdentity(id, $"Person-{id}");
    }

    public MembershipIdentity Build()
    {
        return this._identity;
    }
}
