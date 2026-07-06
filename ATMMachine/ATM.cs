using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ATMMachine;

public class ATM
{
    /// <summary>
    /// The available currency values and amount currently in the ATM.
    /// int value, int amtAvailable
    /// </summary>
    private Dictionary<int, int> availableCurrency;

    /// <summary>
    /// The currency values and types of currency allowed in the ATM.
    /// int value, CurrencyType typeOfCurrency
    /// </summary>
    private Dictionary<int, CurrencyType> typeOfCurrency;

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
    public ATM()
    {
        InitATMCurency();
    }

    /// <summary>
    /// Sets up the ATM with a starting set of currencies.
    /// Links the type and amount to the value of the currency.
    /// </summary>
    private void InitATMCurency()
    {
        this.typeOfCurrency = new Dictionary<int, CurrencyType>();
        this.availableCurrency = new Dictionary<int, int>();

        this.typeOfCurrency[500] = CurrencyType.BILL;
        this.availableCurrency[500] = 2;

        this.typeOfCurrency[200] = CurrencyType.BILL;
        this.availableCurrency[200] = 3;

        this.typeOfCurrency[100] = CurrencyType.BILL;
        this.availableCurrency[100] = 5;

        this.typeOfCurrency[50] = CurrencyType.BILL;
        this.availableCurrency[50] = 12;

        this.typeOfCurrency[20] = CurrencyType.BILL;
        this.availableCurrency[20] = 20;

        this.typeOfCurrency[10] = CurrencyType.BILL;
        this.availableCurrency[10] = 50;

        this.typeOfCurrency[5] = CurrencyType.BILL;
        this.availableCurrency[5] = 100;

        this.typeOfCurrency[2] = CurrencyType.COIN;
        this.availableCurrency[2] = 250;

        this.typeOfCurrency[1] = CurrencyType.COIN;
        this.availableCurrency[1] = 500;
    }

    /// <summary>
    /// Returns if the currency value is a bill or coin.
    /// </summary>
    /// <param name="currencyValue">The value of the bill/coin</param>
    /// <returns>string representing the type of currency. "none" if it doesn't exist</returns>
    public string GetCurrencyType(int currencyValue)
    {
        CurrencyType result;

        try
        {
            result = this.typeOfCurrency[currencyValue];
        }
        catch
        {
            return "none";
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
    /// Determines the amount of each value of currency to withdraw.
    /// </summary>
    /// <param name="quantity">Desired total to withdraw</param>
    /// <returns>
    /// A dictionary with the value of the currency and the associated amount
    /// or null if there's not enough in the ATM to return the requested quantity
    /// </returns>
    public Dictionary<int, int>? Withdraw(int quantity)
    {
        // sort available currency values from low to high
        List<int> currencyValues = new(this.availableCurrency.Keys);

        currencyValues.Sort();

        // work from largest to smallest, adding to result until = quantity
        var remainder = quantity;
        Dictionary<int, int> result = new();

        for (int currencyIdx = currencyValues.Count - 1; currencyIdx >= 0; currencyIdx--)
        {
            var value = currencyValues[currencyIdx];
            var totalGiven = 0;

            while (value <= remainder)
            {
                remainder -= value;
                totalGiven += 1;
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

        return result;
    }
}
