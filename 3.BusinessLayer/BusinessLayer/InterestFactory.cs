using BusinessLayer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class InterestFactory : IInterestFactory
    {
        private readonly IInterestCalculator _interestCalculator;

        public InterestFactory(IInterestCalculator interestCalculator)
        {
            _interestCalculator = interestCalculator;
        }

        public IInterestResult MakeCalculation(decimal lumpSumInvestment, decimal monthlyInvestment, int timescaleInYears,
            decimal wideBoundPercentageUpper, decimal wideBoundPercentageLower, decimal narrowBoundPercentageUpper,
            decimal narrowBoundPercentageLower, decimal targetValue)
        {
            if (lumpSumInvestment < 0)
                return new InterestResult("LumpSumInvestment cannot be less than zero");

            if (lumpSumInvestment > 1000000000)
                return new InterestResult("LumpSumInvestment cannot be greater than 1000000000");

            if (monthlyInvestment < 0)
                return new InterestResult("MonthlyInvestment cannot be less than zero");

            if (monthlyInvestment > 1000000000)
                return new InterestResult("MonthlyInvestment cannot be greater than 1000000000");

            if (narrowBoundPercentageLower < -100m)
                return new InterestResult("NarrowBoundPercentageLower cannot be less than -100");

            if (narrowBoundPercentageLower > 100m)
                return new InterestResult("NarrowBoundPercentageLower cannot be great than 100");

            if (narrowBoundPercentageUpper < -100m)
                return new InterestResult("NarrowBoundPercentageUpper cannot be less than -100");

            if (narrowBoundPercentageUpper > 100m)
                return new InterestResult("NarrowBoundPercentageUpper cannot be great than 100");

            if (timescaleInYears < 1)
                return new InterestResult("TimescaleInYears cannot be less than 1");

            if (timescaleInYears > 100)
                return new InterestResult("TimescaleInYears cannot be greater than 100");

            if (targetValue < 0)
                return new InterestResult("TargetValue cannot be less than zero");

            if (targetValue > 1000000000)
                return new InterestResult("TargetValue cannot be greater than 1000000000");

            if (wideBoundPercentageLower < -100m)
                return new InterestResult("WideBoundPercentageLower cannot be lower than -100");

            if (wideBoundPercentageLower > 100m)
                return new InterestResult("WideBoundPercentageLower cannot be greater than 100");

            if (wideBoundPercentageUpper < -100m)
                return new InterestResult("WideBoundPercentageUpper cannot be lower than -100");

            if (wideBoundPercentageUpper > 100m)
                return new InterestResult("WideBoundPercentageUpper cannot be greater than 100");

            if (narrowBoundPercentageLower >= narrowBoundPercentageUpper)
                return new InterestResult("NarrowBoundPercentageLower must be less than NarrowBoundPercentageUpper");

            if (wideBoundPercentageLower >= wideBoundPercentageUpper)
                return new InterestResult("WideBoundPercentageLower must be less than WideBoundPercentageUpper");

            if (wideBoundPercentageLower >= narrowBoundPercentageLower)
                return new InterestResult("WideBoundPercentageLower must be less than NarrowBoundPercentageLower");

            if (narrowBoundPercentageUpper >= wideBoundPercentageUpper)
                return new InterestResult("NarrowBoundPercentageUpper must be less than WideBoundPercentageUpper");

            var response = CreateResult(lumpSumInvestment, monthlyInvestment, timescaleInYears, wideBoundPercentageUpper, 
                wideBoundPercentageLower, narrowBoundPercentageUpper, narrowBoundPercentageLower, targetValue);

            return response;
        }

        private InterestResult CreateResult(decimal lumpSumInvestment, decimal monthlyInvestment, int timescaleInYears,
            decimal wideBoundPercentageUpper, decimal wideBoundPercentageLower, decimal narrowBoundPercentageUpper,
            decimal narrowBoundPercentageLower, decimal targetValue)
        {
            var totalInvested = _interestCalculator.Calculate(lumpSumInvestment, monthlyInvestment, 0)
                .Take(timescaleInYears + 1).Last();

            var wideBoundUpperSeries = _interestCalculator.Calculate(lumpSumInvestment, monthlyInvestment, wideBoundPercentageUpper)
                .Take(timescaleInYears + 1);

            var wideBoundLowerSeries = _interestCalculator.Calculate(lumpSumInvestment, monthlyInvestment, wideBoundPercentageLower)
                .Take(timescaleInYears + 1);

            var narrowBoundLowerSeries = _interestCalculator.Calculate(lumpSumInvestment, monthlyInvestment, narrowBoundPercentageLower)
                .Take(timescaleInYears + 1);

            var narrowBoundUpperSeries = _interestCalculator.Calculate(lumpSumInvestment, monthlyInvestment, narrowBoundPercentageUpper)
                .Take(timescaleInYears + 1);

            var years = Enumerable.Range(0, timescaleInYears + 1);

            var targetYears = Enumerable.Repeat(targetValue, timescaleInYears + 1);

            var response = new InterestResult(totalInvested, wideBoundUpperSeries, wideBoundLowerSeries, narrowBoundLowerSeries, narrowBoundUpperSeries, years, targetYears);

            return response;
        }
    }
}
