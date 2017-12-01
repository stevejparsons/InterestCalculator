using BusinessLayer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class InterestCalculator : IInterestCalculator
    {
        private const int MonthsInYear = 12;

        public IEnumerable<decimal> Calculate(decimal lumpSumInvestment, decimal monthlyInvestment, decimal interestRatePercentage)
        {
            yield return Math.Round(lumpSumInvestment, 2, MidpointRounding.AwayFromZero);

            var decimalInterestRate = 1m + (interestRatePercentage / 100m);

            var monthlyInterestRate = (decimal)(Math.Pow((double)decimalInterestRate, (1.0 / MonthsInYear)));

            var runningTotal = lumpSumInvestment;

            var yearNumber = 0;

            while (true)
            {
                yearNumber++;

                for (int month = 0; month < MonthsInYear; month++)
                {
                    runningTotal *= monthlyInterestRate;

                    runningTotal += monthlyInvestment;
                }

                var roundedAmount = Math.Round(runningTotal, 2, MidpointRounding.AwayFromZero);

                yield return roundedAmount;
            }
        }
    }
}
