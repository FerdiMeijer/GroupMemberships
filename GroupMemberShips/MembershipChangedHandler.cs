using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
                        ? g.Select(gi => new MembershipGroup(gi.Id, String.Empty, new List<MembershipIdentity>() as MembershipIdentity
                        : g.Select(i => new MembershipIdentity(i.Id, String.Empty)).ToList() as IEnumerable<MembershipIdentity>;

                    return members;
                });
            var membershipGroup = new MembershipGroup(group.Id, group.Name, members);

            _membershipGroups.AddOrUpdate(membershipGroup);
        }
    }
}
