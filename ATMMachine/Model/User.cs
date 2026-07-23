using System;
using System.Collections.Generic;
using System.Text;

namespace ATMMachine.Model;

public class User
{
    private ATM pocket;
    private BankAccount? checking;
    private BankAccount? savings;

    private static readonly Dictionary<double, int> defaultPocket = new() { [100] = 1, [20] = 5, [5] = 3, [1] = 5, [.25] = 2 };
    private static readonly int defaultCheckingBalance = 1000;
    private static readonly int defaultSavingsBalance = 10000;

    /// <summary>
    /// Default Constructor.
    /// Constructs a user with the default checking balance, savings balance, and pocket.
    /// </summary>
    public User() : this(defaultCheckingBalance, defaultSavingsBalance, defaultPocket) { }

    /// <summary>
    /// Constructs a custom user with just a checking account
    /// </summary>
    /// <param name="checkingBalance">The amount in the user's checking</param>
    /// <param name="pocket">The cash in the user's pocket</param>
    public User(int checkingBalance, Dictionary<double, int> pocket) : this(checkingBalance, -1, pocket) { }

    /// <summary>
    /// Constructs a custom user with checking and savings accounts.
    /// </summary>
    /// <param name="checkingBalance">The amount in the user's checking or -1 for no checking account</param>
    /// <param name="savingsBalance">The amount in the user's savings or -1 for no savings account</param>
    /// <param name="pocket">The cash in the user's pocket</param>
    public User(int checkingBalance, int savingsBalance, Dictionary<double, int> pocket)
    {
        if (checkingBalance >= 0)
        {
            this.checking = new(checkingBalance, BankAccount.AccountType.Checking);
        }
        
        if (savingsBalance >= 0)
        {
            this.savings = new(savingsBalance, BankAccount.AccountType.Savings);
        }
        
        this.pocket = new(0);

        if ((pocket is not null) && (pocket.Count > 0))
        {
            foreach (var value in pocket.Keys)
            {
                this.pocket.Deposit(value, pocket[value]);
            }
        }
    }




    /*
     * -------------------                             ---------------------
     * ------------------- CHECKING ACCOUNT OPERATIONS ---------------------
     * -------------------                             ---------------------
     */

    /// <summary>
    /// Returns a boolean indicating whether the user has a checking account.
    /// </summary>
    /// <returns>true if there is a checking, false if not</returns>
    public bool HasChecking()
    {
        return (this.checking != null);
    }

    /// <summary>
    /// Returns the user's checking balance
    /// </summary>
    /// <returns>the user's checking balance or -1 if there is no checking account</returns>
    public double GetCheckingBalance()
    {
        if (this.checking == null)
        {
            return -1;
        }

        return this.checking.GetBalance();
    }

    /// <summary>
    /// Deposits an amount into the user's checking account.
    /// Will round to 2 decimal places if something with more decimal places is inputted.
    /// </summary>
    /// <param name="amount">the amount to deposit</param>
    /// <returns>true on success, false on failure</returns>
    public bool DepositIntoChecking(double amount)
    {
        if (this.checking == null)
        {
            return false;
        }

        return this.checking.Deposit(amount);
    }

    /// <summary>
    /// Deposits a quanity of the specified cash value into the user's checking account.
    /// </summary>
    /// <param name="value">The value of the bill/coin</param>
    /// <param name="quantity">the number of that value to deposit</param>
    /// <returns>true on success, false on failure</returns>
    public bool DepositIntoChecking(double value, int quantity)
    {
        if ((this.checking == null) || (this.pocket.GetCurrencyType(value) is null))
        {
            return false;
        }

        return this.checking.Deposit(value, quantity);
    }

    /// <summary>
    /// Withdraws the specified amount from the user's checking
    /// </summary>
    /// <param name="amount">The amount to withdraw</param>
    /// <returns>the total withdrawn or -1 if no checking account exists for the user</returns>
    public double WithdrawFromChecking(double amount)
    {
        if (this.checking == null)
        {
            return -1;
        }

        return this.checking.Withdraw(amount);
    }




    /*
     * -------------------                            ---------------------
     * ------------------- SAVINGS ACCOUNT OPERATIONS ---------------------
     * -------------------                            ---------------------
     */

    /// <summary>
    /// Returns a boolean indicating whether the user has a savings account.
    /// </summary>
    /// <returns>true if there is a savings, false if not</returns>
    public bool HasSavings()
    {
        return (this.savings != null);
    }

    /// <summary>
    /// Returns the user's savings balance
    /// </summary>
    /// <returns>the user's savings balance or -1 if there is no savings account</returns>
    public double GetSavingsBalance()
    {
        if (this.savings == null)
        {
            return -1;
        }

        return this.savings.GetBalance();
    }

    /// <summary>
    /// Deposits an amount into the user's savings account.
    /// Will round to 2 decimal places if something with more decimal places is inputted.
    /// </summary>
    /// <param name="amount">the amount to deposit</param>
    /// <returns>true on success, false on failure</returns>
    public bool DepositIntoSavings(double amount)
    {
        if (this.savings == null)
        {
            return false;
        }

        return this.savings.Deposit(amount);
    }

    /// <summary>
    /// Deposits a quanity of the specified cash value into the user's savings account.
    /// </summary>
    /// <param name="value">The value of the bill/coin</param>
    /// <param name="quantity">the number of that value to deposit</param>
    /// <returns>true on success, false on failure</returns>
    public bool DepositIntoSavings(double value, int quantity)
    {
        if ((this.savings == null) || (this.pocket.GetCurrencyType(value) is null))
        {
            return false;
        }

        return this.savings.Deposit(value, quantity);
    }

    /// <summary>
    /// Withdraws the specified amount from the user's checking
    /// </summary>
    /// <param name="amount">The amount to withdraw</param>
    /// <returns>the total withdrawn or -1 if no checking account exists for the user</returns>
    public double WithdrawFromSavings(double amount)
    {
        if (this.savings == null)
        {
            return -1;
        }

        return this.savings.Withdraw(amount);
    }




    /*
     * -------------------                   ---------------------
     * ------------------- POCKET OPERATIONS ---------------------
     * -------------------                   ---------------------
     */

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
