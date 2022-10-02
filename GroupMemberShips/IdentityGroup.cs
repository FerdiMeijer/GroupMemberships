using System;
using System.Collections.Generic;

namespace GroupMemberShips;

public enum IdentityType
{
    Person,
    IdentityGroup
}

public enum ResourceType
{
    Organization,
    Team
}

public interface IHasId<TId>
{
    TId Id { get; set; }
}

public abstract class BaseEntity
{
    public Guid Id { get; set; }

    public DateTime UpdateDateTime { get; set; }
}

public class Identity : BaseEntity, IHasId<Guid>
{
    public ICollection<IdentityGroup> IdentityGroups { get; set; }

    public IdentityType Type { get; set; }
}

public class IdentityGroup : BaseEntity, IHasId<Guid>
{
    public List<IdentityType> AllowedIdentityTypes { get; set; }
    public bool CanBeNested { get; set; }
    public ICollection<Identity> Identities { get; set; }
    public string Name { get; set; }
    public Guid ResourceId { get; set; }
    public ResourceType ResourceType { get; set; }
}