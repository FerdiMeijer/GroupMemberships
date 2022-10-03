using System.Collections.Generic;
using System.Linq;

namespace GroupMemberShips
{
    public class MembershipChangedHandler
    {
        private readonly MembershipGroups _membershipGroups;

        public MembershipChangedHandler(MembershipGroups membershipGroups)
        {
            this._membershipGroups = membershipGroups;
        }

        public void HandleGroupChanged(IdentityGroup group)
        {
            var members = group.Identities.ToLookup(i => i.Type)
                .SelectMany(g =>
                {
                    var members = g.Key == IdentityType.IdentityGroup
                        ? g.Select(gi => new MembershipGroup(gi.Id, string.Empty, new List<MembershipIdentity>()))
                        : g.Select(i => new MembershipIdentity(i.Id, string.Empty)).ToList() as IEnumerable<MembershipIdentity>;

                    return members;
                })
                .ToList();
            var membershipGroup = new MembershipGroup(group.Id, group.Name, members);

            _membershipGroups.AddOrUpdate(membershipGroup);
        }
    }
}