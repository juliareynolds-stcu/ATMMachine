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
    public void ReturnsNoneWhenGettingInvalidCurrency()
    {
        // Arrange
        ATM sut = new();

        // Act
        var currencyType = sut.GetCurrencyType(-1);

        // Assert
        currencyType.Should().Be("none");
    }

    [Test]
    public void ReturnsAppropriateCurrencyType()
    {
        // Arrange
        ATM sut = new();

        // Act
        var shouldBeBill = sut.GetCurrencyType(500);
        var shouldBeCoin = sut.GetCurrencyType(1);

        // Assert
        shouldBeBill.Should().Be("bill");
        shouldBeCoin.Should().Be("coin");
    }

    [Test]
    public void WithdrawReturnsCorrectAmount()
    {
        // Arrange
        ATM sut = new();

        // Act
        Dictionary<int, int>? result = sut.Withdraw(434);
        int sum = 0;

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
}
