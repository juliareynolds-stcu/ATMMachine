using System;
using System.Collections.Generic;
using System.Text;

namespace ATMMachine;

public class User
{
    private ATM pocket;
    private int checkingBalance;

    private static readonly Dictionary<double, int> defaultPocket = new() { [100] = 1, [20] = 5, [5] = 3, [1] = 5, [.25] = 2 };
    private static readonly int defaultCheckingBalance = 1000;

    public User() : this(defaultCheckingBalance, defaultPocket) { }

    public User(int checkingBalance, Dictionary<double, int> pocket)
    {
        if (checkingBalance > 0)
        {
            this.checkingBalance = checkingBalance;
        }
        else
        {
            this.checkingBalance = defaultCheckingBalance;
        }

        this.pocket = new ATM(0);

        if ((pocket is not null) && (pocket.Count > 0))
        {
            foreach (var value in pocket.Keys)
            {
                this.pocket.Deposit(value, pocket[value]);
            }
        }
    }
}
