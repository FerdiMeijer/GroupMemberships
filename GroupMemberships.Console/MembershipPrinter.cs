﻿using GroupMemberShips;

namespace GroupMemberships.ConsoleApp;

public static class MembershipPrinter
{
    public static void PrintMemberships(this Memberships memberships)
    {
        memberships.ToList().ForEach(m =>
        {
            m.PrintIdentity(0);
        });
    }

    private static void PrintIdentity(this MembershipIdentity identity, int level)
    {
        if (level > 2)
        {
            return;
        }

        if (identity is MembershipGroup membershipGroup)
        {
            Console.WriteLine($"{GetTabs(level)}{identity.Name}");
            level++;
            membershipGroup.Members.ToList().ForEach(m => PrintIdentity(m, level));
        }
        else
        {
            Console.WriteLine($"{GetTabs(level)}{identity.Name}");
        }
    }

    private static string GetTabs(int tabs)
    {
        return Enumerable.Range(0, tabs).Aggregate("", (a, b) => a + "\t");
    }
}
