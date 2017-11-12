using BusinessLayer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebService.Contract;

namespace BusinessLayer
{
    public class InterestFactory : IInterestFactory
    {
        private readonly IInterestCalculator _interestCalculator;

        public InterestFactory(IInterestCalculator interestCalculator)
        {
            _interestCalculator = interestCalculator;
        }

        public CalculationResponseDto MakeCalculation(CalculationRequestDto request)
        {
            if (request.LumpSumInvestment < 0)
                return new CalculationResponseDto("LumpSumInvestment cannot be less than zero");

            if (request.LumpSumInvestment > 1000000000)
                return new CalculationResponseDto("LumpSumInvestment cannot be greater than 1000000000");

            if (request.MonthlyInvestment < 0)
                return new CalculationResponseDto("MonthlyInvestment cannot be less than zero");

            if (request.MonthlyInvestment > 1000000000)
                return new CalculationResponseDto("MonthlyInvestment cannot be greater than 1000000000");

            if (request.NarrowBoundPercentageLower < -100m)
                return new CalculationResponseDto("NarrowBoundPercentageLower cannot be less than -100");

            if (request.NarrowBoundPercentageLower > 100m)
                return new CalculationResponseDto("NarrowBoundPercentageLower cannot be great than 100");

            if (request.NarrowBoundPercentageUpper < -100m)
                return new CalculationResponseDto("NarrowBoundPercentageUpper cannot be less than -100");

            if (request.NarrowBoundPercentageUpper > 100m)
                return new CalculationResponseDto("NarrowBoundPercentageUpper cannot be great than 100");

            if (request.TimescaleInYears < 1)
                return new CalculationResponseDto("TimescaleInYears cannot be less than 1");

            if (request.TimescaleInYears > 100)
                return new CalculationResponseDto("TimescaleInYears cannot be greater than 100");

            if (request.TargetValue < 0)
                return new CalculationResponseDto("TargetValue cannot be less than zero");

            if (request.TargetValue > 1000000000)
                return new CalculationResponseDto("TargetValue cannot be greater than 1000000000");

            if (request.WideBoundPercentageLower < -100m)
                return new CalculationResponseDto("WideBoundPercentageLower cannot be lower than -100");

            if (request.WideBoundPercentageLower > 100m)
                return new CalculationResponseDto("WideBoundPercentageLower cannot be greater than 100");

            if (request.WideBoundPercentageUpper < -100m)
                return new CalculationResponseDto("WideBoundPercentageUpper cannot be lower than -100");

            if (request.WideBoundPercentageUpper > 100m)
                return new CalculationResponseDto("WideBoundPercentageUpper cannot be greater than 100");

            if (request.NarrowBoundPercentageLower >= request.NarrowBoundPercentageUpper)
                return new CalculationResponseDto("NarrowBoundPercentageLower must be less than NarrowBoundPercentageUpper");

            if (request.WideBoundPercentageLower >= request.WideBoundPercentageUpper)
                return new CalculationResponseDto("WideBoundPercentageLower must be less than WideBoundPercentageUpper");

            if (request.WideBoundPercentageLower >= request.NarrowBoundPercentageLower)
                return new CalculationResponseDto("WideBoundPercentageLower must be less than NarrowBoundPercentageLower");

            if (request.NarrowBoundPercentageUpper >= request.WideBoundPercentageUpper)
                return new CalculationResponseDto("NarrowBoundPercentageUpper must be less than WideBoundPercentageUpper");

            var response = CreateResponse(request);

            return response;
        }

        private CalculationResponseDto CreateResponse(CalculationRequestDto request)
        {
            var totalInvested = _interestCalculator.Calculate(request.LumpSumInvestment, request.MonthlyInvestment, 0)
                .Take(request.TimescaleInYears).Last();

            var wideBoundUpperSeries = _interestCalculator.Calculate(request.LumpSumInvestment, request.MonthlyInvestment, request.WideBoundPercentageUpper)
                .Take(request.TimescaleInYears);

            var wideBoundLowerSeries = _interestCalculator.Calculate(request.LumpSumInvestment, request.MonthlyInvestment, request.WideBoundPercentageLower)
                .Take(request.TimescaleInYears);

            var narrowBoundLowerSeries = _interestCalculator.Calculate(request.LumpSumInvestment, request.MonthlyInvestment, request.NarrowBoundPercentageLower)
                .Take(request.TimescaleInYears);

            var narrowBoundUpperSeries = _interestCalculator.Calculate(request.LumpSumInvestment, request.MonthlyInvestment, request.NarrowBoundPercentageUpper)
                .Take(request.TimescaleInYears);

            var years = Enumerable.Range(1, request.TimescaleInYears);

            var targetYears = Enumerable.Repeat(request.TargetValue, request.TimescaleInYears);

            var respone = new CalculationResponseDto(totalInvested, wideBoundUpperSeries, wideBoundLowerSeries, narrowBoundLowerSeries, narrowBoundUpperSeries, years, targetYears);

            return respone;
        }
    }
}
