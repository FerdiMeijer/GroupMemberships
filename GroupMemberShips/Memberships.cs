using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GroupMemberShips
{
    public class Memberships : IEnumerable<MembershipGroup>
    {
        private readonly ConcurrentDictionary<Guid, MembershipGroup> _memberships = new();

        public MembershipGroup Get(Guid id)
        {
            return _memberships[id];
        }

        public void AddOrUpdate(MembershipGroup membership)
        {
            _memberships[membership.Id] = membership;
        }

        public void Remove(Guid identityGroupId)
        {
            if (!_memberships.TryRemove(identityGroupId, out MembershipGroup _))
            {
                Console.WriteLine($"    Warning!: allready removed {identityGroupId}.");
            }
        }

        /// <summary>
        /// The enumerator returned from the dictionary is safe to use concurrently with
        /// reads and writes to the dictionary, however it does not represent a 
        /// moment-in-time snapshot of the dictionary.
        /// The contents exposed through the enumerator may contain modifications made to
        /// the dictionary after GetEnumerator was called.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<MembershipGroup> GetEnumerator()
        {
            return _memberships.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}