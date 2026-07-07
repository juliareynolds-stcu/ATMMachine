using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ATMMachine;

public class ATM
{
    /// <summary>
    /// The available currency values and amount currently in the ATM.
    /// double value, int amtAvailable
    /// </summary>
    private Dictionary<double, int> availableCurrency;

    /// <summary>
    /// The currency values and types of currency allowed in the ATM.
    /// double value, CurrencyType typeOfCurrency
    /// </summary>
    private Dictionary<double, CurrencyType> typeOfCurrency;

    /// <summary>
    /// Types of currency.
    /// Currently only bill or coin
    /// </summary>
    private enum CurrencyType
    {
        BILL,
        COIN
    }
    
    /// <summary>
    /// Constructor that initializes the starting state of the ATM
    /// </summary>
    public ATM() : this(1) {}

    /// <summary>
    /// Constructor that initializes the starting state of the ATM based on caller option.
    /// 0 - empty ATM,
    /// 1 - standard ATM,
    /// 2 - max ATM,
    /// Defaults to standard ATM on invalid input
    /// </summary>
    public ATM(int option)
    {
        InitATMCurency();

        if (option == 0)
        {
            foreach (var value in this.availableCurrency!.Keys)
            {
                this.availableCurrency[value] = 0;
            }
        }

        if (option == 2)
        {
            foreach (var value in this.availableCurrency!.Keys)
            {
                this.availableCurrency[value] = int.MaxValue;
            }
        }

    }

    /// <summary>
    /// Sets up the ATM with a starting set of currencies.
    /// Links the type and amount to the value of the currency.
    /// </summary>
    private void InitATMCurency()
    {
        this.typeOfCurrency = new Dictionary<double, CurrencyType>();
        this.availableCurrency = new Dictionary<double, int>();

        this.typeOfCurrency[100] = CurrencyType.BILL;
        this.availableCurrency[100] = 10;

        this.typeOfCurrency[50] = CurrencyType.BILL;
        this.availableCurrency[50] = 20;

        this.typeOfCurrency[20] = CurrencyType.BILL;
        this.availableCurrency[20] = 50;

        this.typeOfCurrency[10] = CurrencyType.BILL;
        this.availableCurrency[10] = 100;

        this.typeOfCurrency[5] = CurrencyType.BILL;
        this.availableCurrency[5] = 200;

        this.typeOfCurrency[2] = CurrencyType.BILL;
        this.availableCurrency[2] = 500;

        this.typeOfCurrency[1] = CurrencyType.BILL;
        this.availableCurrency[1] = 1000;

        this.typeOfCurrency[0.25] = CurrencyType.COIN;
        this.availableCurrency[0.25] = 400;

        this.typeOfCurrency[0.10] = CurrencyType.COIN;
        this.availableCurrency[0.10] = 100;

        this.typeOfCurrency[0.05] = CurrencyType.COIN;
        this.availableCurrency[0.05] = 200;

        this.typeOfCurrency[0.01] = CurrencyType.COIN;
        this.availableCurrency[0.01] = 1000;
    }

    /// <summary>
    /// Returns the current state of the ATM - the number of each value of currency currently in the ATM.
    /// </summary>
    /// <returns>Available currency in the form of a dictionary</returns>
    public Dictionary<double, int> GetCurrentState()
    {
        return new(this.availableCurrency);
    }

    /// <summary>
    /// Returns if the currency value is a bill or coin.
    /// </summary>
    /// <param name="currencyValue">The value of the bill/coin</param>
    /// <returns>string representing the type of currency. "none" if it doesn't exist</returns>
    public string? GetCurrencyType(double currencyValue)
    {
        if (!this.typeOfCurrency.TryGetValue(currencyValue, out CurrencyType result))
        {
            return null;
        }
        
        if (result == CurrencyType.BILL)
        {
            return "bill";
        }
        else
        {
            return "coin";
        }
    }

    /// <summary>
    /// Returns the amount of the specified currency value remaining in the ATM
    /// </summary>
    /// <param name="currencyValue">The value of the bill/coin</param>
    /// <returns>The number of that bill/coin in the ATM</returns>
    public int GetQuantityAvailable(double currencyValue)
    {
        var result = -1;

        try
        {
            result = this.availableCurrency[currencyValue];
        }
        catch
        {
            return result;
        }

        return result;
    }


    /// <summary>
    /// Returns the total $ amount available for withdrawl
    /// </summary>
    /// <returns>int total $ in the atm</returns>
    public double GetTotalAvailable()
    {
        var result = 0.0;

        foreach (var value in this.availableCurrency.Keys)
        {
            result += (value * this.availableCurrency[value]);
        }

        if (result < 0)
        {
            return double.MaxValue;
        }

        return result;
    }

    /// <summary>
    /// Determines the amount of each value of currency to withdraw.
    /// </summary>
    /// <param name="quantity">Desired total to withdraw</param>
    /// <returns>
    /// A dictionary with the value of the currency and the associated amount
    /// or null if there's not enough in the ATM to return the requested quantity
    /// </returns>
    public Dictionary<double, int>? Withdraw(double quantity)
    {
        // sort available currency values from low to high
        List<double> currencyValues = new(this.availableCurrency.Keys);

        currencyValues.Sort();

        // work from largest to smallest, adding to result until = quantity
        decimal remainder = (decimal) quantity;
        Dictionary<double, int> remainingCurrency = GetCurrentState();
        Dictionary<double, int> result = new();

        for (int currencyIdx = currencyValues.Count - 1; currencyIdx >= 0; currencyIdx--)
        {
            var value = currencyValues[currencyIdx];
            var totalGiven = 0;

            while ((decimal) value <= remainder && remainingCurrency[value] > 0)
            {
                remainder -= (decimal) value;
                totalGiven += 1;

                remainingCurrency[value] -= 1;
            }

            if (totalGiven > 0)
            {
                result[value] = totalGiven;
            }
        }

        // return null if quantity was unable to be met
        if (remainder > 0)
        {
            return null;
        }

        // if quantity met, update avaiable cash
        this.availableCurrency = remainingCurrency;

        return result;
    }


    /// <summary>
    /// Deposits quantity of the specified value into the ATM if possible.
    /// </summary>
    /// <param name="value">The value of the bill/coin</param>
    /// <param name="quantity">The number of that value to deposit</param>
    /// <returns>true on success, false on failure</returns>
    public bool Deposit(double value, int quantity)
    {
        if (value < 0 || quantity < 0)
        {
            return false;
        }

        if (this.availableCurrency.TryGetValue(value, out var currQuantity))
        {
            var total = currQuantity + quantity; // if this goes over MaxValue, it should become negative

            if (total > 0)
            {
                this.availableCurrency[value] = currQuantity + quantity;

                return true;
            }
        }

        return false;
    }
}
