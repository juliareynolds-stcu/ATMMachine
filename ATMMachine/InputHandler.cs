using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;

namespace ATMMachine;

internal class InputHandler
{
    ATM atm;
    User user;

    /// <summary>
    /// Constructor
    /// </summary>
    public InputHandler()
    {
        ATMCLI.WithdrawSelected += WithdrawHandled;
        ATMCLI.ViewCurrentStateSelected += ViewCurrentStateHandled;
        ATMCLI.DepositSelected += DepositHandled;
        ATMCLI.ViewAccountBalanceSelected += ViewAccountBalanceHandled;
        ATMCLI.ViewPocketContentsSelected += ViewPocketContentsHandled;

        this.atm = new();
        this.user = new();
    }
    
    /// <summary>
    /// Triggered when user selects withdraw option.
    /// Handles withdrawing from the atm.
    /// </summary>
    private void WithdrawHandled()
    {
        Console.WriteLine();
        Console.WriteLine("How much would you like to withdraw?");

        var userBalance = this.user.GetCheckingBalance();
        var atmBalance = this.atm.GetTotalAvailable();

        Console.WriteLine($"${((userBalance < atmBalance) ? userBalance : atmBalance)} is available for withdrawl.");
        Console.Write(">  ");

        var withdrawlAmt = -1.0;

        while(true)
        {
            try
            {
                var userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    throw new ArgumentException();
                }

                userInput = Regex.Replace(userInput, "[^0-9.-]", "");

                withdrawlAmt = double.Parse(userInput);

                break;
            }
            catch
            {
                Console.WriteLine("Invalid input. Please enter a number > 0");
                Console.Write(">  ");
            }
        }

        if (withdrawlAmt < 1)
        {
            Console.WriteLine("\r\nNothing to withdraw. Returning to menu.\r\n");
            return;
        }

        if (userBalance < withdrawlAmt)
        {
            Console.WriteLine("\r\nInsufficient funds. Could not complete your withdrawl. \r\n");
            return;
        }

        Dictionary<double, int>? result = this.atm.Withdraw(withdrawlAmt);

        if (result is null)
        {
            Console.WriteLine("\r\nThere are not enough funds in this ATM to complete " +
                "your request at this time. Please visit the next nearest ATM.\r\n");

            return;
        }

        foreach (var type in result)
        {
            this.user.PlaceInPocket(type.Key, type.Value);
        }

        var total = PrintCurrency(result);

        this.user.WithdrawFromChecking(total);

        Console.WriteLine("Withdraw Successful!");
        Console.WriteLine($"Total:  ${total}\r\n");
    }

    /// <summary>
    /// Takes a dictionary (int value, int quantity) of currency and prints to the console
    /// </summary>
    /// <param name="currency">the dictionary to print</param>
    /// <returns>total currency represented in the dictionary</returns>
    private double PrintCurrency(Dictionary<double, int> currency)
    {
        Console.WriteLine();
        Console.WriteLine("| Value | Type | Quantity of Units |");
        Console.WriteLine("|-------|------|-------------------|");

        var total = 0.0;

        // Another way to output the currency
        //foreach (var value in currency.Keys)
        //{
        //    var quantity = currency[value];

        //    Console.WriteLine($"{quantity} ${value} {atm.GetCurrencyType(value)}{((quantity > 1) ? "s" : "")}");
            
        //    total += (value * quantity);
        //}

        foreach (var value in currency.Keys)
        {
            var quantity = currency[value];

            if (quantity != 0)
            {
                total += Math.Round((value * quantity), 2);

                Console.WriteLine($"| {value}\t| {atm.GetCurrencyType(value)} | {quantity}\t           |");
            }
        }

        Console.WriteLine();

        return Math.Round(total, 2);
    }

    /// <summary>
    /// Triggered when user selects view current state option.
    /// Displays the current state of the ATM.
    /// </summary>
    private void ViewCurrentStateHandled()
    {
        var total = PrintCurrency(atm.GetCurrentState());

        Console.WriteLine($"\r\nMax Withdrawl Amount:  ${total}\r\n\r\n");
    }

    /// <summary>
    /// Triggered when user selects deposit option from menu.
    /// Handles getting the user's desired deposit and depositing it into the ATM.
    /// </summary>
    private void DepositHandled()
    {
        Dictionary<double, int> depositRequest = new(); // double bill/coin value, int quantity
        List<double> cantDeposit = new();
        string? userInput;
        string[]? valuesToDeposit;

        Console.WriteLine("\r\nYou have the following in your pocket:");

        ViewPocketContentsHandled();

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
                    var currencyValue = double.Parse(value);

                    if ((atm.GetCurrencyType(currencyValue) is null) || (user.GetQuantityInPocket(currencyValue) < 1))
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
                Console.WriteLine("\r\nInvalid input. Please enter a list of valid currency values only (1 for $1 bill, 0.25 for 25 cent coin).");
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

        var thereIsCashToDeposit = false;
        List<double> insufficientFunds = new();

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

            if (quantity > 0)
            {
                if ((user.GetQuantityInPocket(value) >= quantity))
                {
                    thereIsCashToDeposit = true;

                    depositRequest[value] = quantity;
                }
                else
                {
                    insufficientFunds.Add(value);
                }
            }
        }

        if (!thereIsCashToDeposit)
        {
            Console.WriteLine("\r\nCannot make deposit. Exiting to menu.\r\n");

            return;
        }

        Console.WriteLine("\r\nThe following can be deposited:");

        var total = PrintCurrency(depositRequest);

        Console.WriteLine($"Total: ${total}");

        if (insufficientFunds.Count > 0)
        {
            Console.WriteLine("\r\nYou do not have enough of the following in your pocket to deposit the requested quantity: ");

            foreach (var value in insufficientFunds)
            {
                Console.WriteLine($"${value} {atm.GetCurrencyType(value)}s");
            }
        }

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

        Dictionary<double, int> successful = new();

        foreach(var value in depositRequest.Keys)
        {
            var quantity = depositRequest[value];
            bool result = this.atm.Deposit(value, quantity);

            if (!result)
            {
                Console.WriteLine($"\r\nSomething went wrong with your request to deposit {quantity} ${value} {atm.GetCurrencyType(value)}{((quantity > 1) ? "s" : "")}");
            }
            else
            {
                this.user.RemoveFromPocket(value, quantity);
                this.user.DepositIntoChecking(value, quantity);

                successful[value] = quantity;
            }
        }

        if (successful.Count is 0)
        {
            Console.WriteLine("Nothing was deposited successfully.\r\n");

            return;
        }

        Console.WriteLine("\r\nThe following were successfully deposited:");

        total = PrintCurrency(successful);

        Console.WriteLine($"\r\nThank you for your ${total} deposit\r\n");
    }

    /// <summary>
    /// Triggered when user selects view account balance option from menu.
    /// Handles printing the user's current balance.
    /// </summary>
    private void ViewAccountBalanceHandled()
    {
        Console.WriteLine("\r\nYour Accounts:");
        Console.WriteLine($"  *  Checking - ${this.user.GetCheckingBalance()}\r\n");
    }

    /// <summary>
    /// Triggered when user selects to view the contents of their pocket.
    /// Handles printing the user's pocket cash.
    /// </summary>
    private void ViewPocketContentsHandled()
    {
        var total = PrintCurrency(this.user.GetPocketContents());

        Console.WriteLine($"Total in cash: ${total}");
        Console.WriteLine();
    }
}
