using System;
using System.Collections.Generic;

namespace GroupMemberShips
{
    public class MembershipValidator
    {
        private readonly HashSet<Guid> _checkedGroups = new();
        private readonly MembershipGroups _membershipGroups;

        public MembershipValidator(MembershipGroups membershipGroups)
        {
            _membershipGroups = membershipGroups;
        }

        public bool IsMemberOf(Guid identityId, Guid groupId)
        {
            if (_checkedGroups.TryGetValue(groupId, out Guid _))
            {
                return false;
            }

            var group = _membershipGroups.Get(groupId);
            _checkedGroups.Add(groupId);

            var result = false;
            foreach (var member in group.Members)
            {
                if (member is MembershipGroup membershipGroup)
                {
                    result = IsMemberOf(identityId, membershipGroup.Id);
                }
                else
                {
                    result = member.Id == identityId;
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