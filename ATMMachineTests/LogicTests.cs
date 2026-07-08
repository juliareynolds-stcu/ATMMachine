namespace ATMMachineTests;

using AwesomeAssertions;
using ATMMachine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using System.Runtime.InteropServices.Marshalling;

public class LogicTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void SetsUpCallerDesiredATMType()
    {
        // Arrange
        ATM sut = new(0);
        ATM sut2 = new(2);

        // Act
        var empty = sut.GetCurrentState();
        var max = sut2.GetCurrentState();

        // Assert
        empty.Should().NotBeNull();

        foreach (var value in empty.Keys)
        {
            empty[value].Should().Be(0);
        }

        max.Should().NotBeNull();

        foreach (var value in max.Keys)
        {
            max[value].Should().Be(int.MaxValue);
        }
    }

    [Test]
    public void ReturnsNullWhenGettingInvalidCurrency()
    {
        // Arrange
        ATM sut = new();

        // Act
        var currencyType = sut.GetCurrencyType(-1);

        // Assert
        currencyType.Should().BeNull();
    }

    [Test]
    public void ReturnsAppropriateCurrencyType()
    {
        // Arrange
        ATM sut = new();

        // Act
        var shouldBeBill = sut.GetCurrencyType(100);
        var shouldBeCoin = sut.GetCurrencyType(0.01);

        // Assert
        shouldBeBill.Should().Be("bill");
        shouldBeCoin.Should().Be("coin");
    }

    [Test]
    public void ReturnsNegativeWhenAttemptingToRetirieveAmtOfInvalidCurrency()
    {
        // Arrange
        ATM sut = new();

        // Act
        var available = sut.GetQuantityAvailable(-5);

        // Assert
        available.Should().BeLessThan(0);
    }

    [Test]
    public void ReturnsCorrectAvailableOfCurrencyType()
    {
        // Arrange
        ATM sut = new();

        // Act
        var available = sut.GetQuantityAvailable(100);

        // Assert
        available.Should().Be(10);
    }

    [Test]
    public void ReturnsCurrentStateOfATM()
    {
        // Arrange
        ATM sut = new();

        // Act
        var result = sut.GetCurrentState();

        // Assert
        result.Should().NotBeNull();
        //result[500].Should().Be(2);
        //result[200].Should().Be(3);
        //result[100].Should().Be(5);
        //result[50].Should().Be(12);
        //result[20].Should().Be(20);
        //result[10].Should().Be(50);
        //result[5].Should().Be(100);
        //result[2].Should().Be(250);
        //result[1].Should().Be(500);
    }

    [Test]
    public void WithdrawReturnsCorrectAmount()
    {
        // Arrange
        ATM sut = new();

        // Act
        Dictionary<double, int>? result = sut.Withdraw(434);
        var sum = 0.0;

        if (result is not null)
        {
            foreach (var value in result.Keys)
            {
                sum += (value * result[value]);
            }
        }

        // Assert
        sum.Should().Be(434);
    }

    [Test]
    public void WithdrawReturnsCorrectDecimalAmount()
    {
        // Arrange
        ATM sut = new();

        // Act
        Dictionary<double, int>? result = sut.Withdraw(101.63);
        var sum = 0.0;

        if (result is not null)
        {
            foreach (var value in result.Keys)
            {
                sum += (value * result[value]);
            }
        }

        // Assert
        sum.Should().Be(101.63);
    }

    [Test]
    public void ReturnsNullIfNotEnoughCash()
    {
        // Arrange
        ATM sut = new();

        // Act
        var result = sut.Withdraw(99999999);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void OnlyWithdrawsExistingCash()
    {
        // Arrange
        ATM sut = new();

        // Act
        Dictionary<double, int>? result = sut.Withdraw(1100);

        // Assert
        result.Should().NotBeNull();

        result[100].Should().Be(10);
        result[50].Should().Be(2);
    }

    [Test]
    public void UpdatesRemainingCashAfterWithdrawl()
    {
        // Arrange
        ATM sut = new();

        // Act
        sut.Withdraw(1);

        // Assert
        sut.GetCurrentState()[1].Should().Be(999);
        sut.GetQuantityAvailable(1).Should().Be(999);

        //sut.GetCurrentState()[1].Should().Be(499);
        //sut.GetQuantityAvailable(1).Should().Be(499);
    }

    [Test]
    public void DoesntRemoveCashFromATMIfUnableToCompleteTransaction()
    {
        // Arrange
        ATM sut = new();

        // Act
        var withdrawn = sut.Withdraw(9999999);
        //sut.Withdraw(5101);

        Dictionary<double, int>? result = sut.GetCurrentState();

        // Assert
        result.Should().NotBeNull();
        result[100].Should().Be(10);
        withdrawn.Should().BeNull();
    }

    [Test]
    public void SimpleDepositsWork()
    {
        // Arrange
        ATM sut = new(0);

        // Act
        bool result = sut.Deposit(1, 1);

        // Assert
        result.Should().BeTrue();
        sut.GetQuantityAvailable(1).Should().Be(1);
    }

    [Test]
    public void ReturnsFalseOnInvalidInput()
    {
        // Arrange
        ATM sut = new(0);

        // Act
        bool result = sut.Deposit(-1, 1);
        bool result2 = sut.Deposit(1, -1);
        bool result3 = sut.Deposit(int.MaxValue, 1);

        sut.Deposit(1, 1);

        bool result4 = sut.Deposit(1, int.MaxValue);

        // Assert
        result.Should().BeFalse();
        result2.Should().BeFalse();
        result3.Should().BeFalse();
        result4.Should().BeFalse();

        sut.GetQuantityAvailable(1).Should().Be(1);
    }

    [Test]
    public void AccuratelyReturnsTotalValueInATM()
    {
        // Arrange
        ATM sut = new();
        ATM sut2 = new(0);

        // Act
        var result = sut.GetTotalAvailable();
        var result2 = sut2.GetTotalAvailable();

        // Assert
        result.Should().Be(7130);
        result2.Should().Be(0);
    }

    [Test]
    public void WithdrawSpecifiedBill()
    {
        // Arrange
        ATM sut = new();

        // Act
        Dictionary<double, int>? result = sut.Withdraw(1, 5);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(1);
        result.TryGetValue(1, out var quantity).Should().BeTrue();
        quantity.Should().Be(5);
        sut.GetQuantityAvailable(1).Should().Be(995);
    }

    [Test]
    public void WithdrawBillDoesntWithdrawPastAvailableQuantity()
    {
        // Arrange
        ATM sut = new(0);
        ATM sut2 = new(0);

        sut2.Deposit(1, 4);

        // Act
        Dictionary<double, int>? result = sut.Withdraw(1, 1);
        Dictionary<double, int>? result2 = sut2.Withdraw(1, 5);

        // Assert
        result.Should().BeNull();
        sut.GetCurrentState()[1].Should().Be(0);

        result2.Should().BeNull();
        sut2.GetCurrentState()[1].Should().Be(4);
    }

    [Test]
    public void WithdrawBillIgnoresInvalidInput()
    {
        // Arrange
        ATM sut = new(0);

        sut.Deposit(1, 5);

        // Act
        Dictionary<double, int>? result = sut.Withdraw(-1, 1);
        Dictionary<double, int>? result2 = sut.Withdraw(1, -1);

        // Assert
        result.Should().BeNull();
        result2.Should().BeNull();
        sut.GetQuantityAvailable(1).Should().Be(5);
    }
}
