using System;
using System.Collections.Generic;
using System.Text;

namespace ATMMachine;

internal class InputHandler
{
    ATM atm;

    public InputHandler()
    {
        ATMCLI.WithdrawSelected += WithdrawHandled;
        ATMCLI.ViewCurrentStateSelected += ViewCurrentStateHandled;
        ATMCLI.DepositSelected += DepositHandled;

        this.atm = new();
    }
    

    private void WithdrawHandled()
    {
        Console.WriteLine();
        Console.WriteLine("How much would you like to withdraw?");
        Console.Write(">  ");

        int withdrawlAmt = -1;

        try
        {
            withdrawlAmt = int.Parse(Console.ReadLine());
        }
        catch
        {
            Console.WriteLine("Invalid input. Please enter an integer > 0");
        }

        Dictionary<int, int>? result = this.atm.Withdraw(withdrawlAmt);

        if (result is null)
        {
            Console.WriteLine("\r\nThere are not enough funds in this ATM to complete " +
                "your request at this time. Please visit the next nearest ATM.\r\n");

            return;
        }

        Console.WriteLine("\r\nWithdraw Successful!");
        PrintResult(result);
    }

    private void PrintResult(Dictionary<int, int> result)
    {
        Console.WriteLine();
        
        var total = 0;

        foreach (var value in result.Keys)
        {
            var quantity = result[value];
            total += (value * quantity);

            Console.WriteLine($"{quantity} ${value} {atm.GetCurrencyType(value)}{((quantity > 1) ? "s" : "")}");
        }

        Console.WriteLine($"\r\nTotal:  {total}\r\n");
    }

    private void ViewCurrentStateHandled()
    {
        Console.WriteLine();
        Console.WriteLine("| Value | Type | quantity of units |");
        Console.WriteLine("|-------|------|-------------------|");

        var total = 0;
        var currentState = atm.GetCurrentState();

        foreach (var value in currentState.Keys)
        {
            var quantity = currentState[value];
            total += (value * quantity);

            Console.WriteLine($"| {value}\t| {atm.GetCurrencyType(value)} | {quantity}\t           |");
        }

        Console.WriteLine($"\r\nMax Withdrawl Amount:  {total}\r\n");
    }

    private void DepositHandled()
    {
        Console.WriteLine();
        Console.WriteLine("What value of currency would you like to deposit?");
        Console.Write(">  ");

        int depositValue = -1;

        try
        {
            depositValue = int.Parse(Console.ReadLine());
        }
        catch
        {
            Console.WriteLine("Invalid input. Please enter an integer > 0");
        }

        Console.WriteLine();
        Console.WriteLine("How many would you like to deposit?");
        Console.Write(">  ");

        int depositAmt = -1;

        try
        {
            depositAmt = int.Parse(Console.ReadLine());
        }
        catch
        {
            Console.WriteLine("Invalid input. Please enter an integer > 0");
        }

        Console.WriteLine();
        Console.WriteLine($"{depositValue} x {depositAmt} to be deposited");

        bool result = this.atm.Deposit(depositValue, depositAmt);

        if (!result)
        {
            Console.WriteLine("\r\nSomething went wrong with your request.\r\n");

            return;
        }

        Console.WriteLine("\r\nDeposit Successful!");
    }
}
