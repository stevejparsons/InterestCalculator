using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService.Contract
{
    public class CalculationRequestDto
    {
        public CalculationRequestDto(decimal lumpSumInvestment, decimal monthlyInvestment, int timescaleInYears,
            decimal wideBoundPercentageUpper, decimal wideBoundPercentageLower, decimal narrowBoundPercentageUpper,
            decimal narrowBoundPercentageLower, decimal targetValue)
        {
            LumpSumInvestment = lumpSumInvestment;
            MonthlyInvestment = monthlyInvestment;
            TimescaleInYears = timescaleInYears;
            WideBoundPercentageUpper = wideBoundPercentageUpper;
            WideBoundPercentageLower = wideBoundPercentageLower;
            NarrowBoundPercentageUpper = narrowBoundPercentageUpper;
            NarrowBoundPercentageLower = narrowBoundPercentageLower;
            TargetValue = targetValue;
        }

        public decimal LumpSumInvestment { get; }

        public decimal MonthlyInvestment { get; }

        public int TimescaleInYears { get; }

        public decimal WideBoundPercentageUpper { get; }

        public decimal WideBoundPercentageLower { get; }

        public decimal NarrowBoundPercentageUpper { get; }

        public decimal NarrowBoundPercentageLower { get; }

        public decimal TargetValue { get; }
    }
}
