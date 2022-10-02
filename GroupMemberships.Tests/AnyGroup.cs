using GroupMemberShips;

namespace GroupMemberships.Tests;

public class AnyGroup
{
    private readonly Guid _id;
    private IReadOnlyCollection<MembershipIdentity> _members = new List<MembershipIdentity>();

    public AnyGroup()
    {
        _id = Guid.NewGuid();
    }

    public AnyGroup WithMembers(params MembershipIdentity[] members)
    {
        _members = members;

        return this;
    }

    public AnyGroup WithMembers(IReadOnlyCollection<MembershipIdentity> members)
    {
        _members = members;
        return this;
    }

    public MembershipGroup Build()
    {
        return new MembershipGroup(_id, $"Group-{_id}", _members);
    }
}