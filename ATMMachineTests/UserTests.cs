namespace ATMMachineTests;

using ATMMachine.Model;
using AwesomeAssertions;

public class UserTests
{
    private User user;

    [SetUp]
    public void Setup()
    {
        this.user = new(0, 0, null);
    }

    [Test]
    public void CreatesEmptyUser()
    {
        this.user.GetCheckingBalance().Should().Be(0);
        this.user.HasChecking().Should().BeTrue();

        this.user.GetSavingsBalance().Should().Be(0);
        this.user.HasSavings().Should().BeTrue();

        this.user.GetPocketBalance().Should().Be(0);
    }

    [Test]
    public void CreatesUserWithNoChecking()
    {
        var sut = new User(-1, 0, null);

        sut.GetCheckingBalance().Should().Be(-1);
        sut.HasChecking().Should().BeFalse();

        sut.GetSavingsBalance().Should().Be(0);
        sut.HasSavings().Should().BeTrue();

        sut.GetPocketBalance().Should().Be(0);
    }

    [Test]
    public void CreatesUserWithNoSavings()
    {
        var sut = new User(0, -1, null);

        sut.GetCheckingBalance().Should().Be(0);
        sut.HasChecking().Should().BeTrue();

        sut.GetSavingsBalance().Should().Be(-1);
        sut.HasSavings().Should().BeFalse();

        sut.GetPocketBalance().Should().Be(0);
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
