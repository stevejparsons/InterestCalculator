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
            var response = _factory.MakeCalculation(request);

            return response;
        }
    }
}
