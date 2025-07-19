using Task.OvertimeCalculator;

namespace Task.Tests.Calculator;

public class OvertimeCalcTests
{
    [Fact]
    public void CalcBasedOnType_FirstCalculator_ReturnsDoubledSum()
    {
        // Arrange
        IOverTimeCalc calc = new OverTimeCalc();
        decimal allowance = 100m;
        decimal basic = 200m;

        // Act
        decimal result = calc.CalcBasedOnType(1, allowance, basic);

        // Assert
        Assert.Equal((allowance + basic) * 2, result);
    }

    [Fact]
    public void CalcBasedOnType_SecondCalculator_ReturnsTripledSum()
    {
        IOverTimeCalc calc = new OverTimeCalc();
        decimal allowance = 50m;
        decimal basic = 100m;

        decimal result = calc.CalcBasedOnType(2, allowance, basic);

        Assert.Equal((allowance + basic) * 3, result);
    }

    [Fact]
    public void CalcBasedOnType_ThirdCalculator_ReturnsQuadrupledSum()
    {
        IOverTimeCalc calc = new OverTimeCalc();
        decimal allowance = 30m;
        decimal basic = 70m;

        decimal result = calc.CalcBasedOnType(3, allowance, basic);

        Assert.Equal((allowance + basic) * 4, result);
    }

    [Fact]
    public void CalcBasedOnType_InvalidType_ThrowsArgumentException()
    {
        IOverTimeCalc calc = new OverTimeCalc();

        Assert.Throws<ArgumentException>(() => calc.CalcBasedOnType(0, 10m, 10m));
    }
}
