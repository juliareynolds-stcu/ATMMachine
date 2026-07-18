namespace ATMMachineTests;

using ATMMachine;
using AwesomeAssertions;

public class UserTests
{
    private User user;

    [SetUp]
    public void Setup()
    {
        this.user = new(0, new());
    }

    [Test]
    public void CreatesEmptyUser()
    {
        this.user.GetCheckingBalance().Should().Be(0);
        this.user.GetPocketBalance().Should().Be(0);
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(1234.56)]
    [TestCase(0.01)]
    public void AddToChecking(double deposit)
    {
        var result = this.user.DepositIntoChecking(deposit);

        result.Should().BeTrue();
        this.user.GetCheckingBalance().Should().Be(deposit);
    }

    [TestCase(1, 5)]
    [TestCase(50, 1)]
    [TestCase(100, 1000)]
    [TestCase(0.05, 12)]
    [TestCase(0.25, 4)]
    public void AddCashToChecking(double value, int quantity)
    {
        var total = Math.Round((value * quantity), 2);

        var result = this.user.DepositIntoChecking(value, quantity);

        result.Should().BeTrue();
        this.user.GetCheckingBalance().Should().Be(total);
    }

    [TestCase(-1)]
    [TestCase(-0.0001)]
    public void InvalidDepositShouldFail(double deposit)
    {
        var result = this.user.DepositIntoChecking(deposit);

        result.Should().BeFalse();
        this.user.GetCheckingBalance().Should().Be(0);
    }

    [TestCase(1, 5)]
    [TestCase(50, 1)]
    [TestCase(100, 1000)]
    [TestCase(0.05, 12)]
    [TestCase(0.25, 4)]
    public void AddToPocket(double value, int quantity)
    {
        var total = Math.Round((value * quantity), 2);

        var result = this.user.PlaceInPocket(value, quantity);

        result.Should().BeTrue();
        this.user.GetPocketBalance().Should().Be(total);
    }
}
