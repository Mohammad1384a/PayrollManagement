
namespace Task.OvertimeCalculator;
public class OverTimeCalc() : IOverTimeCalc {

    public decimal CalcBasedOnType(byte calcType , decimal allowance, decimal basicSalary) {
        return calcType switch {
            1 => FirstCalculator(allowance, basicSalary),
            2 => SecondCalculator(allowance, basicSalary),
            3 => ThirdCalculator(allowance, basicSalary),
            _ => throw new ArgumentException("Invalid calculation type")
        };
    }

    private static decimal FirstCalculator(decimal allowance, decimal basicSalary) {
        return decimal.Multiply(allowance + basicSalary, 2);
    }

    private static decimal SecondCalculator(decimal allowance, decimal basicSalary) {
        return decimal.Multiply(allowance + basicSalary, 3);
    }

    private static decimal ThirdCalculator(decimal allowance, decimal basicSalary) {
        return decimal.Multiply(allowance + basicSalary, 4);
    }
}
