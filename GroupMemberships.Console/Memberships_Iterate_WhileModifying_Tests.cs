using GroupMemberships.Tests;
using GroupMemberShips;
using System.Diagnostics;

namespace GroupMemberships.ConsoleApp;

public class Memberships_Iterate_WhileModifying_Tests
{
    private readonly MembershipGroups _membershipGroups = new();
    private readonly Random _random = new();
    private readonly Stopwatch _stopwatch = new();
    private readonly MembershipsSeeder _seeder;

    public Memberships_Iterate_WhileModifying_Tests()
    {
        _seeder = new(_membershipGroups);
        _membershipGroups.PrintMemberships();

        Console.WriteLine("Seeds initialized.");
        Console.ReadKey();
    }

    public void Run()
    {
        _stopwatch.Start();

        IterateFromNewThread("It.A", _random.Next(0, 2000));

        AddFromNewThread("Add.X");
        RemoveFromNewThread("Rem.X");

        IterateFromNewThread("It.B", _random.Next(1000, 2000));

        RemoveFromNewThread("Rem.Y");
        AddFromNewThread("Add.Y");

        IterateFromNewThread("It.C", _random.Next(2000, 2000));
        AddFromNewThread("Add.Z");

        _stopwatch.Stop();

        Console.ReadKey();
    }

    private void AddFromNewThread(string name)
    {
        Task.Run(() =>
        {
            for (int i = 0; i < 30; i++)
            {
                var members = _seeder.GetSomeExistingMembers(0, 3);
                var newMembership = new AnyMembershipGroup().WithMembers(members).Build();
                _membershipGroups.AddOrUpdate(newMembership);
                Console.WriteLine($"    {name}:Added: {newMembership.Id}");

                Thread.Sleep(_random.Next(100, 500));
            }

            Console.WriteLine($"Finished:{name}");
        });
    }

    private void IterateFromNewThread(string name, int startAt)
    {
        Task.Run(() =>
        {
            Thread.Sleep(startAt);
            var i = 0;
            var list = _membershipGroups.ToList();
            Console.WriteLine($"Started iterating over {list.Count}");
            foreach (var item in list)
            {
                i++;
                Console.WriteLine($"{_stopwatch.ElapsedMilliseconds}:{name}:{i} {item.Id}:" +
                    $" memberCount:{item.Members.Count} total: {_membershipGroups.Count()}");
                Thread.Sleep(_random.Next(10, 20));
            }
            Console.WriteLine($"Finished:{name}");
        });
    }

    private void RemoveFromNewThread(string name)
    {
        Task.Run(() =>
        {
            for (int i = 0; i < 30; i++)
            {
                var existingGroup = _seeder.PopAnExistingResourceGroup();
                _membershipGroups.Remove(existingGroup);
                Console.WriteLine($"    {name}:Removed: {existingGroup}");

                Thread.Sleep(_random.Next(100, 500));
            }
            Console.WriteLine($"Finished:{name}");
        });
    }
}