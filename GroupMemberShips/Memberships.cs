using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

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
}