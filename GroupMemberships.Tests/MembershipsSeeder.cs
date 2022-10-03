using GroupMemberShips;
using System.Collections.Concurrent;

namespace GroupMemberships.Tests;

/// <summary>
/// Generates and keeps track of all generated Memberships
/// </summary>
public class MembershipsSeeder
{
    private const int DefaultNoPersons = 10;
    private const int DefaultNoResourceGroups = 10;
    private const int DefaultNoTeamMemberGroups = 10;
    private readonly List<MembershipGroup> _memberGroups;
    private readonly List<MembershipIdentity> _persons;
    private readonly Random _random = new();
    private readonly ConcurrentDictionary<Guid, IdentityType> _seeds = new();

    public MembershipsSeeder(MembershipGroups membershipGroups,
        int noPersons = DefaultNoPersons,
        int noResourceGroups = DefaultNoResourceGroups,
        int noTeamMemberGroups = DefaultNoTeamMemberGroups)
    {
        _persons = Enumerable.Range(0, noPersons).Select(i => new AnyMembershipIdentity().Build()).ToList();
        _memberGroups = new List<MembershipGroup>();
        for (int i = 0; i < noTeamMemberGroups; i++)
        {
            var members = GetSomeExistingMembers(0, 5);
            var membergroup = new AnyMembershipGroup().WithMembers(members).Build();

            _memberGroups.Add(membergroup); // add to member groups
            membershipGroups.AddOrUpdate(membergroup); // add to memberships

            _seeds[membergroup.Id] = IdentityType.IdentityGroup;
        }

        for (int i = 0; i < noResourceGroups; i++)
        {
            var members = GetSomeExistingMembers(5, 10);
            var resourcegroup = new AnyMembershipGroup().WithMembers(members).Build();
            membershipGroups.AddOrUpdate(resourcegroup); // add to memberships

            _seeds[resourcegroup.Id] = IdentityType.IdentityGroup;
        }
    }

    public IReadOnlyCollection<MembershipIdentity> GetSomeExistingMembers(int groupCount, int personCount)
    {
        var newChildren = new List<MembershipIdentity>();

        for (int i = 0; i < groupCount; i++)
        {
            var randomGroupId = GetRandom(IdentityType.IdentityGroup);
            var group = _memberGroups.Single(g => g.Id == randomGroupId);

            if (!newChildren.Any(c => c.Id == group.Id))
                newChildren.Add(group);
        }

        for (int i = 0; i < personCount; i++)
        {
            var randomPersonId = GetRandom(IdentityType.Person);
            var person = _persons.Single(g => g.Id == randomPersonId);

            if (!newChildren.Any(c => c.Id == person.Id))
                newChildren.Add(person);
        }

        return newChildren.ToList();
    }

    public Guid PopAnExistingResourceGroup()
    {
        var group = GetRandom(IdentityType.IdentityGroup);

        _seeds.Remove(group, out IdentityType _);

        return group;
    }

    private Guid GetRandom(IdentityType identityType)
    {
        var identities = identityType == IdentityType.IdentityGroup
            ? _memberGroups.Select(mg => mg.Id).ToList()
            : _persons.Select(p => p.Id).ToList();

        return identities
            .Skip(_random.Next(0, identities.Count - 1))
            .Take(1)
            .Single();
    }
}
