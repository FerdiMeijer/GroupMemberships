using GroupMemberShips;

namespace GroupMemberships.Tests;

public class AnyMembershipGroup
{
    private readonly Guid _id;
    private IReadOnlyCollection<MembershipIdentity> _members = new List<MembershipIdentity>();

    public AnyMembershipGroup()
    {
        _id = Guid.NewGuid();
    }

    public AnyMembershipGroup WithMembers(params MembershipIdentity[] members)
    {
        _members = members;

        return this;
    }

    public AnyMembershipGroup WithMembers(IReadOnlyCollection<MembershipIdentity> members)
    {
        _members = members;
        return this;
    }

    public MembershipGroup Build()
    {
        return new MembershipGroup(_id, $"Group-{_id}", _members);
    }
}