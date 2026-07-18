using System;
using System.Collections.Generic;
using System.Text;

namespace ATMMachine;

public class User
{
    private ATM pocket;
    private double checkingBalance;

    private static readonly Dictionary<double, int> defaultPocket = new() { [100] = 1, [20] = 5, [5] = 3, [1] = 5, [.25] = 2 };
    private static readonly int defaultCheckingBalance = 10000;

    /// <summary>
    /// Default Constructor
    /// </summary>
    public User() : this(defaultCheckingBalance, defaultPocket) { }

    /// <summary>
    /// Constructs a custom user
    /// </summary>
    /// <param name="checkingBalance">The amount in the user's checking</param>
    /// <param name="pocket">The cash in the user's pocket</param>
    public User(int checkingBalance, Dictionary<double, int> pocket)
    {
        if (checkingBalance >= 0)
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

    /// <summary>
    /// Returns the user's checking balance
    /// </summary>
    /// <returns></returns>
    public double GetCheckingBalance()
    {
        return this.checkingBalance;
    }

    /// <summary>
    /// Deposits an amount into the user's checking account.
    /// </summary>
    /// <param name="amount">the amount to deposit</param>
    /// <returns>true on success, false on failure</returns>
    public bool DepositIntoChecking(double amount)
    {
        if (amount < 0)
        {
            return false;
        }

        this.checkingBalance = Math.Round((this.checkingBalance + amount), 2);

        return true;
    }

    /// <summary>
    /// Deposits a quanity of the specified cash value into the user's checking account.
    /// </summary>
    /// <param name="value">The value of the bill/coin</param>
    /// <param name="quantity">the number of that value to deposit</param>
    /// <returns>true on success, false on failure</returns>
    public bool DepositIntoChecking(double value, int quantity)
    {
        if ((value < 0) || (quantity < 0) 
            || (this.pocket.GetCurrencyType(value) is null))
        {
            return false;
        }

        return DepositIntoChecking(Math.Round((value * quantity), 2));
    }

    /// <summary>
    /// Withdraws the specified amount from the user's checking
    /// </summary>
    /// <param name="amount">The amount to withdraw</param>
    /// <returns>the total withdrawn</returns>
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

    /// <summary>
    /// Returns the total cash value in the user's pocket
    /// </summary>
    /// <returns>the total in $</returns>
    public double GetPocketBalance()
    {
        return this.pocket.GetTotalAvailable();
    }

    /// <summary>
    /// Returns the contents of the user's pocket
    /// </summary>
    /// <returns>Dictionary with values and quantities of cash</returns>
    public Dictionary<double, int> GetPocketContents()
    {
        return this.pocket.GetCurrentState();
    }

    /// <summary>
    /// Returns the quantity of the specified value of bill/coin in the user's pocket
    /// </summary>
    /// <param name="value">the $ value of the bill/coin</param>
    /// <returns>the quanity or -1 on failure</returns>
    public int GetQuantityInPocket(double value)
    {
        return this.pocket.GetQuantityAvailable(value);
    }

    /// <summary>
    /// Places the specified value and quantity of cash in the user's pocket
    /// </summary>
    /// <param name="value">The value of the bill/coin</param>
    /// <param name="quantity">The amount of the bill/coin</param>
    /// <returns>true on success, false on failure</returns>
    public bool PlaceInPocket(double value, int quantity)
    {
        if ((value < 0) || (quantity < 0))
        {
            return false;
        }

        var result = this.pocket.Deposit(value, quantity);

        return result;
    }

    /// <summary>
    /// Removes the specified $ amount from the user's pocket
    /// </summary>
    /// <param name="amount">The $ to remove</param>
    /// <returns>A Dictionary with the value and amount of the cash removed from the pocket</returns>
    public Dictionary<double, int>? RemoveFromPocket(double amount)
    {
        if (amount < 0)
        {
            return null;
        }

        return this.pocket.Withdraw(Math.Round(amount, 2));
    }

    /// <summary>
    /// Removes the specified value of bill/coin and the associated quantity from the user's pocket
    /// </summary>
    /// <param name="value">the $ value of the bill/coin</param>
    /// <param name="quantity">the quantity of that bill/coin to remove</param>
    /// <returns>true on success, false on failure</returns>
    public Dictionary<double, int>? RemoveFromPocket(double value, int quantity)
    {
        if ((value < 0) || (quantity < 0))
        {
            return null;
        }

        return this.pocket.Withdraw(value, quantity);
    }
}
