using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GroupMemberShips
{
    public class Memberships : IEnumerable<MembershipGroup>
    {
        private readonly ConcurrentDictionary<Guid, MembershipGroup> _memberships = new();

        public void AddOrUpdate(MembershipGroup membership)
        {
            _memberships[membership.Id] = membership;
        }

        public MembershipGroup Get(Guid id)
        {
            return _memberships[id];
        }

        public IEnumerator<MembershipGroup> GetEnumerator()
        {
            return _memberships.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Remove(Guid identityGroupId)
        {
            if (!_memberships.TryRemove(identityGroupId, out MembershipGroup _))
            {
                Console.WriteLine($"    Warning!: allready removed {identityGroupId}.");
            }
        }
    }

    public class MembershipValidator
    {
        private readonly Memberships _memberships;
        private HashSet<Guid> _checkedGroups = new HashSet<Guid>();

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
            foreach(var member in group.Members)
            {
                if(member is MembershipGroup membershipGroup)
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