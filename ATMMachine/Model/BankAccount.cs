using System;
using System.Collections.Generic;
using System.Text;

namespace ATMMachine.Model;

internal class BankAccount
{
    private double balance { get; set; } = 0;
    private AccountType accountType;

    public enum AccountType
    {
        Checking
        , Savings
    }

    /// <summary>
    /// Constructs bank account of the specified type with $0 starting balance
    /// </summary>
    public BankAccount(AccountType type)
    { 
        this.accountType = type;
    }

    /// <summary>
    /// Constructs bank account with specified starting balance, or 0 if specified balance is negative
    /// </summary>
    /// <param name="initialBalance">the account starting balance</param>
    public BankAccount(double initialBalance, AccountType type) : this(type)
    {
        if (initialBalance > 0)
        {
            this.balance = initialBalance;
        }
    }

    public AccountType GetAccountType()
    {
        return this.accountType;
    }

    /// <summary>
    /// Returns the account balance
    /// </summary>
    /// <returns>double - the account balance</returns>
    public double GetBalance()
    {
        return this.balance;
    }

    /// <summary>
    /// Deposits an amount into the account.
    /// </summary>
    /// <param name="amount">the amount to deposit</param>
    /// <returns>true on success, false on failure</returns>
    public bool Deposit(double amount)
    {
        if (amount < 0)
        {
            return false;
        }

        this.balance = Math.Round((this.balance + amount), 2);

        return true;
    }

    /// <summary>
    /// Deposits a quanity of the specified cash value into the account.
    /// </summary>
    /// <param name="value">The value of the currency</param>
    /// <param name="quantity">the number of that value to deposit</param>
    /// <returns>true on success, false on failure</returns>
    public bool Deposit(double value, int quantity)
    {
        if ((value < 0) || (quantity < 0))
        {
            return false;
        }

        return Deposit(Math.Round((value * quantity), 2));
    }

    /// <summary>
    /// Withdraws the specified amount from the account
    /// </summary>
    /// <param name="amount">The amount to withdraw</param>
    /// <returns>the total withdrawn</returns>
    public double Withdraw(double amount)
    {
        if (amount < 0)
        {
            return 0;
        }

        amount = Math.Round(amount, 2);

        var newBalance = Math.Round((this.balance - amount), 2);

        if (newBalance < 0)
        {
            return 0;
        }

        this.balance = newBalance;

        return amount;
    }
}
