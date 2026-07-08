using System;
using System.Collections.Generic;
using System.Text;

namespace ATMMachine;

public class User
{
    private ATM pocket;
    private double checkingBalance;

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

    public double GetCheckingBalance()
    {
        return this.checkingBalance;
    }

    public bool DepositIntoChecking(double amount)
    {
        if (amount < 0)
        {
            return false;
        }

        this.checkingBalance = Math.Round((this.checkingBalance + amount), 2);

        return true;
    }

    public bool DepositIntoChecking(double value, int quantity)
    {
        if ((value < 0) || (quantity < 0) 
            || (this.pocket.GetCurrencyType(value) is null))
        {
            return false;
        }

        return DepositIntoChecking(value * quantity);
    }

    public double WithdrawFromChecking(double amount)
    {
        if (amount < 0)
        {
            return 0;
        }

        amount = Math.Round(amount, 2);

        var newBalance = Math.Round((this.checkingBalance - amount), 2);

        if (newBalance < 0)
        {
            return 0;
        }

        this.checkingBalance = newBalance;

        return amount;
    }

    public double GetPocketBalance()
    {
        return this.pocket.GetTotalAvailable();
    }

    public bool PlaceInPocket(double value, int quantity)
    {
        if ((value < 0) || (quantity < 0))
        {
            return false;
        }

        var result = this.pocket.Deposit(value, quantity);

        return result;
    }

    public Dictionary<double, int>? RemoveFromPocket(double amount)
    {
        if (amount < 0)
        {
            return null;
        }

        return this.pocket.Withdraw(Math.Round(amount, 2));
    }

    public Dictionary<double, int>? RemoveFromPocket(double value, int quantity)
    {
        if ((value < 0) || (quantity < 0))
        {
            return null;
        }

        return this.pocket.Withdraw(value, quantity);
    }
}
