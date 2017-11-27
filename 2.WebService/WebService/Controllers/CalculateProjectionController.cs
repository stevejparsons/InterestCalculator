using BusinessLayer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebService.Contract;

namespace WebService.Controllers
{
    public class CalculateProjectionController : ApiController, ICalculationService
    {
        private readonly IInterestFactory _factory;

        public CalculateProjectionController(IInterestFactory factory)
        {
            _factory = factory;
        }

        [HttpPost]
        public CalculationResponseDto CalculateProjection(CalculationRequestDto request)
        {
            var result = _factory.MakeCalculation(request.LumpSumInvestment, request.MonthlyInvestment, request.TimescaleInYears,
                request.WideBoundPercentageUpper, request.WideBoundPercentageLower, request.NarrowBoundPercentageUpper, 
                request.NarrowBoundPercentageLower, request.TargetValue);

            if (result.Success)
            {
                var response = new CalculationResponseDto(result.TotalInvested, result.WideBoundUpperSeries, result.WideBoundLowerSeries,
                    result.NarrowBoundLowerSeries, result.NarrowBoundUpperSeries, result.Years, result.TargetValue);

                return response;
            }
            else
            {
                var response = new CalculationResponseDto(result.ErrorMessage);

                return response;
            }
        }
    }
}
