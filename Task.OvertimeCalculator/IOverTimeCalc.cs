using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.OvertimeCalculator;
public interface IOverTimeCalc {
    decimal CalcBasedOnType(byte calcType, decimal allowance, decimal basicSalary);
}
