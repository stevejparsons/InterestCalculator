using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService.Contract
{
    public class CalculationResponseDto
    {
        public CalculationResponseDto(decimal totalInvested, IEnumerable<decimal> wideBoundUpperSeries, 
            IEnumerable<decimal> wideBoundLowerSeries, IEnumerable<decimal> narrowBoundLowerSeries, 
            IEnumerable<decimal> narrowBoundUpperSeries, IEnumerable<int> years, IEnumerable<decimal> targetValue)
        {
            Success = true;
            ErrorMessage = string.Empty;
            TotalInvested = totalInvested;
            WideBoundUpperSeries = wideBoundUpperSeries;
            WideBoundLowerSeries = wideBoundLowerSeries;
            NarrowBoundLowerSeries = narrowBoundLowerSeries;
            NarrowBoundUpperSeries = narrowBoundUpperSeries;
            Years = years;
            TargetValue = targetValue;
        }

        public CalculationResponseDto(string errorMessage)
        {
            Success = false;
            ErrorMessage = errorMessage;
        }

        public bool Success { get; }

        public decimal TotalInvested { get; }

        public IEnumerable<decimal> WideBoundUpperSeries { get; }

        public IEnumerable<decimal> WideBoundLowerSeries { get; }

        public IEnumerable<decimal> NarrowBoundLowerSeries { get; }

        public IEnumerable<decimal> NarrowBoundUpperSeries { get; }

        public IEnumerable<int> Years { get; }

        public IEnumerable<decimal> TargetValue { get; }

        public string ErrorMessage { get; }
    }
}
