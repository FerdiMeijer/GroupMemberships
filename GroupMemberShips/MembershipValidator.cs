using System;
using System.Collections.Generic;

namespace GroupMemberShips
{
    public class MembershipValidator
    {
        private readonly HashSet<Guid> _checkedGroups = new();
        private readonly Memberships _memberships;

        public MembershipValidator(Memberships memberships)
        {
            _memberships = memberships;
        }

        public bool IsMemberOf(Guid personId, Guid groupId)
        {
            if (_checkedGroups.TryGetValue(groupId, out Guid _))
            {
                return false;
            }

            var group = _memberships.Get(groupId);
            _checkedGroups.Add(groupId);

            var result = false;
            foreach (var member in group.Members)
            {
                if (member is MembershipGroup membershipGroup)
                {
                    result = IsMemberOf(personId, membershipGroup.Id);
                }
                else
                {
                    result = member.Id == personId;
                }

                if (result)
                {
                    break;
                }
            }

            return result;
        }
    }
}