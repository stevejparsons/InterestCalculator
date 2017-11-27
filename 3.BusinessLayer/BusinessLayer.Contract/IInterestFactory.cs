using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contract
{
    public interface IInterestFactory
    {
        IInterestResult MakeCalculation(decimal lumpSumInvestment, decimal monthlyInvestment, int timescaleInYears,
            decimal wideBoundPercentageUpper, decimal wideBoundPercentageLower, decimal narrowBoundPercentageUpper,
            decimal narrowBoundPercentageLower, decimal targetValue);
    }
}
