using System;
using System.Collections.Generic;
using System.Text;

namespace ATMMachine;

public class ATM
{
    private Dictionary<int, int> availableCurrency; // int value, int amtAvailable
    private Dictionary<int, CurrencyType> typeOfCurrency; // int value, string type;

    private enum CurrencyType
    {
        BILL,
        COIN
    }
    
    public ATM()
    {
        InitATMCurency();
    }

    /// <summary>
    /// Sets up the ATM with a starting set of currencies
    /// </summary>
    private void InitATMCurency()
    {
        this.typeOfCurrency = new Dictionary<int, CurrencyType>();

        this.typeOfCurrency.Add(500, CurrencyType.BILL);
        this.typeOfCurrency.Add(200, CurrencyType.BILL);
        this.typeOfCurrency.Add(100, CurrencyType.BILL);
        this.typeOfCurrency.Add(50, CurrencyType.BILL);
        this.typeOfCurrency.Add(20, CurrencyType.BILL);
        this.typeOfCurrency.Add(10, CurrencyType.BILL);
        this.typeOfCurrency.Add(5, CurrencyType.BILL);
        this.typeOfCurrency.Add(2, CurrencyType.COIN);
        this.typeOfCurrency.Add(1, CurrencyType.COIN);

        this.availableCurrency = new Dictionary<int, int>();

        this.availableCurrency.Add(500, 2);
        this.availableCurrency.Add(200, 3);
        this.availableCurrency.Add(100, 5);
        this.availableCurrency.Add(50, 12);
        this.availableCurrency.Add(20, 20);
        this.availableCurrency.Add(10, 50);
        this.availableCurrency.Add(5, 100);
        this.availableCurrency.Add(2, 250);
        this.availableCurrency.Add(1, 500);
    }

    public Dictionary<int, int> Withdraw(int quantity)
    {
        // find largest bills first
    }
}
