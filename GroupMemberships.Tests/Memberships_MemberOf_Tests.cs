using FluentAssertions;
using GroupMemberShips;
using System.Diagnostics;

namespace GroupMemberships.Tests;

public class Memberships_MemberOf_Tests
{
    [Fact]
    public void FindPersonInGroup_WhenPersonGroupIsInResourceGroup()
    {
        var person1 = new AnyPerson().Build();
        var person2 = new AnyPerson().Build();
        var personGroup = new AnyGroup().WithMembers(person1, person2).Build();
        var resourceGroup = new AnyGroup().WithMembers(personGroup).Build();

        var memberships = new Memberships();
        memberships.AddOrUpdate(personGroup);
        memberships.AddOrUpdate(resourceGroup);

        memberships.PrintMemberships();

        var validator = new MembershipValidator(memberships);
        var result = validator.IsMemberOf(person1.Id, resourceGroup.Id);

        result.Should().BeTrue();
    }

    [Fact]
    public void FindPersonInGroup_WhenPersonGroupIsInNestedGroupAndIsInResourceGroup()
    {
        var person1 = new AnyPerson().Build();
        var person2 = new AnyPerson().Build();
        var person3 = new AnyPerson().Build();
        var person4 = new AnyPerson().Build();

        var personGroup1 = new AnyGroup().WithMembers(person1, person2).Build();
        var personGroup2 = new AnyGroup().WithMembers( personGroup1, person3, person4 ).Build();
        var resourceGroup = new AnyGroup().WithMembers(personGroup2).Build();

        var memberships = new Memberships();
        memberships.AddOrUpdate(personGroup1);
        memberships.AddOrUpdate(personGroup2);
        memberships.AddOrUpdate(resourceGroup);

        memberships.PrintMemberships();

        var validator = new MembershipValidator(memberships);
        var result = validator.IsMemberOf(person1.Id, resourceGroup.Id);
        result.Should().BeTrue();
    }

    [Fact]
    public void FindPersonInGroup_WhenPersonGroupIsInDoubleNestedGroupAndIsInResourceGroup()
    {
        var person1 = new AnyPerson().Build();
        var person2 = new AnyPerson().Build();
        var person3 = new AnyPerson().Build();
        var person4 = new AnyPerson().Build();

        var personGroup1 = new AnyGroup().WithMembers(person1, person2).Build();
        var personGroup2 = new AnyGroup().WithMembers(personGroup1, person3, person4).Build();
        var personGroup3 = new AnyGroup().WithMembers(personGroup2).Build();

        var resourceGroup = new AnyGroup().WithMembers(personGroup3).Build();

        var memberships = new Memberships();
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