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

    public MembershipGroup Build()
    {
        return new MembershipGroup(_id, $"Group-{_id}", _members);
    }

    public AnyMembershipGroup WithMembers(params MembershipIdentity[] members)
    {
        _members = Clone(members);

        return this;
    }

    public AnyMembershipGroup WithMembers(IReadOnlyCollection<MembershipIdentity> members)
    {
        _members = Clone(members);

        return this;
    }

    private IReadOnlyCollection<MembershipIdentity> Clone(IEnumerable<MembershipIdentity> members)
    {
        return members.OfType<MembershipGroup>()
           .Select(group => new MembershipGroup(group.Id, group.Name, new List<MembershipIdentity>()))
           .Concat(members.OfType<MembershipIdentity>()
               .Select(identity => new MembershipIdentity(identity.Id, identity.Name)))
           .ToList();
    }
}