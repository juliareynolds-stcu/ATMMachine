using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;

namespace ATMMachine;

internal class InputHandler
{
    ATM atm;

    /// <summary>
    /// Constructor
    /// </summary>
    public InputHandler()
    {
        ATMCLI.WithdrawSelected += WithdrawHandled;
        ATMCLI.ViewCurrentStateSelected += ViewCurrentStateHandled;
        ATMCLI.DepositSelected += DepositHandled;

        this.atm = new();
    }
    
    /// <summary>
    /// Triggered when user selects withdraw option.
    /// Handles withdrawing from the atm.
    /// </summary>
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

        var total = PrintCurrency(result);

        Console.WriteLine($"Total:  {total}\r\n");
    }

    /// <summary>
    /// Takes a dictionary (int value, int quantity) of currency and prints to the console
    /// </summary>
    /// <param name="currency">the dictionary to print</param>
    /// <returns>int total currency represented in the dictionary</returns>
    private int PrintCurrency(Dictionary<int, int> currency)
    {
        Console.WriteLine();
        
        var total = 0;

        foreach (var value in currency.Keys)
        {
            var quantity = currency[value];

            Console.WriteLine($"{quantity} ${value} {atm.GetCurrencyType(value)}{((quantity > 1) ? "s" : "")}");
            
            total += (value * quantity);
        }

        Console.WriteLine();

        return total;
    }

    /// <summary>
    /// Triggered when user selects view current state option.
    /// Displays the current state of the ATM.
    /// </summary>
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

    /// <summary>
    /// Triggered when user selects deposit option from menu.
    /// Handles getting the user's desired deposit and depositing it into the ATM.
    /// </summary>
    private void DepositHandled()
    {
        Dictionary<int, int> depositRequest = new(); // int bill/coin value, int quantity
        List<int> cantDeposit = new();
        string? userInput;
        string[]? valuesToDeposit;

        Console.WriteLine();

        Console.WriteLine("\r\nPlease enter the value of the bills/coins you would like to deposit separated by commas");

        while (true)
        {
            Console.Write(">  ");

            try
            {
                userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    throw new ArgumentException();
                }

                userInput = Regex.Replace(userInput, "[a-zA-Z]", "");

                valuesToDeposit = userInput.Replace("$", "").Split(",");

                foreach (var value in valuesToDeposit)
                {
                    var currencyValue = int.Parse(value);

                    if (atm.GetCurrencyType(currencyValue) is null)
                    {
                        cantDeposit.Add(currencyValue);
                    }
                    else
                    {
                        depositRequest[currencyValue] = 0;
                    }
                }

                break;
            }
            catch
            {
                Console.WriteLine("\r\nInvalid input. Please enter a list of integers only");
            }
        }

        if (cantDeposit.Count > 0)
        {
            Console.WriteLine("\r\nThe following are not able to be deposited: ");

            foreach (var value in cantDeposit)
            {
                Console.WriteLine(value);
            }
        }

        if (depositRequest.Count == 0)
        {
            Console.WriteLine("\r\nNo funds to deposit. Exiting to menu.\r\n");

            return;
        }

        Console.WriteLine("\r\nPlease enter the quantity of each of the following that you would like to deposit or enter q to quit:");

        foreach (var value in depositRequest.Keys)
        {
            var quantity = -1;

            while (true)
            {
                Console.Write($"${value} {atm.GetCurrencyType(value)}s >  ");

                userInput = Console.ReadLine();

                try
                {
                    if (string.IsNullOrWhiteSpace(userInput))
                    {
                        throw new ArgumentException();
                    }

                    if (userInput == "q" || userInput == "Q")
                    {
                        Console.WriteLine("\r\nDeposit abandoned. No deposit made.\r\n");
                        return;
                    }

                    quantity = int.Parse(userInput);

                    break;
                }
                catch
                {
                    Console.WriteLine("\r\nInvalid input. Please enter a non-zero integer only\r\n");
                }
            }

            depositRequest[value] = quantity;
        }

        Console.WriteLine("\r\nYou requested to deposit the following:");

        var total = PrintCurrency(depositRequest);

        Console.WriteLine($"Total: ${total}");

        Console.WriteLine("\r\nWould you like to procede with the deposit? (y/n)");
        Console.Write(">  ");

        switch (Console.ReadLine())
        {
            case "y" or "Y" or "yes" or "Yes" or "YES":
                break;
            default:
                Console.WriteLine("\r\nDeposit abandoned. No deposit made.\r\n");
                return;
        }

        Dictionary<int, int> successful = new();

        foreach(var value in depositRequest.Keys)
        {
            var quantity = depositRequest[value];
            bool result = this.atm.Deposit(value, depositRequest[value]);

            if (!result)
            {
                Console.WriteLine($"\r\nSomething went wrong with your request to deposit {quantity} ${value} {atm.GetCurrencyType(value)}{((quantity > 1) ? "s" : "")}");
            }
            else
            {
                successful[value] = quantity;
            }
        }

        if (successful.Count is 0)
        {
            Console.WriteLine("Nothing was deposited successfully.");

            return;
        }

        Console.WriteLine("\r\nThe following were successfully deposited:");

        total = PrintCurrency(successful);

        Console.WriteLine($"\r\nThank you for your ${total} deposit\r\n");
    }
}
