using System;
using System.Collections.Generic;
using System.Text;

namespace ATMMachine;

public class ATM
{
    private Dictionary<int, int> availableCurrency; // int value, int available
    private Dictionary<int, CurrencyType> typeOfCurrency; // int value, string type;

    private enum CurrencyType
    {
        BILL,
        COIN
    }
    
    public ATM()
    {
        this.availableCurrency = new Dictionary<int, int>();
        

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
    }
}
