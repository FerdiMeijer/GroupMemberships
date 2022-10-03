using FluentAssertions;
using GroupMemberShips;
using System.Diagnostics;

namespace GroupMemberships.Tests;

public class Memberships_MemberOf_Tests
{
    [Fact]
    public void FindPersonInGroup_WhenPersonGroupIsInResourceGroup()
    {
        var person1 = new AnyMembershipIdentity().Build();
        var person2 = new AnyMembershipIdentity().Build();
        var personGroup = new AnyMembershipGroup().WithMembers(person1, person2).Build();
        var resourceGroup = new AnyMembershipGroup().WithMembers(personGroup).Build();

        var membershipGroups = new MembershipGroups();
        membershipGroups.AddOrUpdate(personGroup);
        membershipGroups.AddOrUpdate(resourceGroup);

        membershipGroups.PrintMemberships();

        var validator = new MembershipValidator(membershipGroups);
        var result = validator.IsMemberOf(person1.Id, resourceGroup.Id);

        result.Should().BeTrue();
    }

    [Fact]
    public void FindPersonInGroup_WhenPersonGroupIsInNestedGroupAndIsInResourceGroup()
    {
        var person1 = new AnyMembershipIdentity().Build();
        var person2 = new AnyMembershipIdentity().Build();
        var person3 = new AnyMembershipIdentity().Build();
        var person4 = new AnyMembershipIdentity().Build();

        var personGroup1 = new AnyMembershipGroup().WithMembers(person1, person2).Build();
        var personGroup2 = new AnyMembershipGroup().WithMembers( personGroup1, person3, person4 ).Build();
        var resourceGroup = new AnyMembershipGroup().WithMembers(personGroup2).Build();

        var membershipGroups = new MembershipGroups();
        membershipGroups.AddOrUpdate(personGroup1);
        membershipGroups.AddOrUpdate(personGroup2);
        membershipGroups.AddOrUpdate(resourceGroup);

        membershipGroups.PrintMemberships();

        var validator = new MembershipValidator(membershipGroups);
        var result = validator.IsMemberOf(person1.Id, resourceGroup.Id);
        result.Should().BeTrue();
    }

    [Fact]
    public void FindPersonInGroup_WhenPersonGroupIsInDoubleNestedGroupAndIsInResourceGroup()
    {
        var person1 = new AnyMembershipIdentity().Build();
        var person2 = new AnyMembershipIdentity().Build();
        var person3 = new AnyMembershipIdentity().Build();
        var person4 = new AnyMembershipIdentity().Build();

        var personGroup1 = new AnyMembershipGroup().WithMembers(person1, person2).Build();
        var personGroup2 = new AnyMembershipGroup().WithMembers(personGroup1, person3, person4).Build();
        var personGroup3 = new AnyMembershipGroup().WithMembers(personGroup2).Build();

        var resourceGroup = new AnyMembershipGroup().WithMembers(personGroup3).Build();

        var memberships = new MembershipGroups();
        memberships.AddOrUpdate(personGroup1);
        memberships.AddOrUpdate(personGroup2);
        memberships.AddOrUpdate(personGroup3);
        memberships.AddOrUpdate(resourceGroup);

        memberships.PrintMemberships();

        var validator = new MembershipValidator(memberships);
        var result = validator.IsMemberOf(person1.Id, resourceGroup.Id);
        result.Should().BeTrue();
    }
}