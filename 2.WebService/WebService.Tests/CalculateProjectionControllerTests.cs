using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebService.Controllers;
using BusinessLayer.Contract;
using Moq;
using WebService.Contract;

namespace WebService.Tests
{
    [TestClass]
    public class CalculateProjectionControllerTests
    {
        private CalculateProjectionController _controller;
        private Mock<IInterestFactory> _factory;
        private Mock<IInterestResult> _result;

        [TestInitialize]
        public void Init()
        {
            _factory = new Mock<IInterestFactory>();
            _controller = new CalculateProjectionController(_factory.Object);
            _result = new Mock<IInterestResult>();
        }

        [TestMethod]
        public void CalculateProjection__MapsParameters__Correctly()
        {
            var request = new CalculationRequestDto(1m, 2m, 3, 4m, 5m, 6m, 7m, 8m);

            _result.SetupGet(g => g.Success).Returns(true);

            _factory.Setup(m => m.MakeCalculation(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<int>(), It.IsAny<decimal>(),
                It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(_result.Object);

            _controller.CalculateProjection(request);

            _factory.Verify(m => m.MakeCalculation(It.Is<decimal>(d => (d == 1m)), It.Is<decimal>(d => (d == 2m)), It.Is<int>(d => (d == 3)),
                It.Is<decimal>(d => (d == 4m)), It.Is<decimal>(d => (d == 5m)), It.Is<decimal>(d => (d == 6m)), It.Is<decimal>(d => (d == 7m)),
                It.Is<decimal>(d => (d == 8m))), Times.Once);
        }

        [TestMethod]
        public void CalculateProjection__ReturnsSuccess__Correctly()
        {
            var request = new CalculationRequestDto(1m, 2m, 3, 4m, 5m, 6m, 7m, 8m);

            _result.SetupGet(g => g.Success).Returns(true);
            _result.SetupGet(g => g.TotalInvested).Returns(9m);
            _result.SetupGet(g => g.WideBoundUpperSeries).Returns(new decimal[] { 10m, 11m });
            _result.SetupGet(g => g.WideBoundLowerSeries).Returns(new decimal[] { 12m, 13m });
            _result.SetupGet(g => g.NarrowBoundUpperSeries).Returns(new decimal[] { 14m, 15m });
            _result.SetupGet(g => g.NarrowBoundLowerSeries).Returns(new decimal[] { 16m, 17m });
            _result.SetupGet(g => g.Years).Returns(new int[] { 0, 1 });
            _result.SetupGet(g => g.TargetValue).Returns(new decimal[] { 18m, 19m });

            _factory.Setup(m => m.MakeCalculation(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<int>(), It.IsAny<decimal>(),
                It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(_result.Object);

            var response = _controller.CalculateProjection(request);

            Assert.AreEqual(true, response.Success);
            Assert.AreEqual(string.Empty, response.ErrorMessage);
            Assert.AreEqual(9m, response.TotalInvested);

            var wideBoundUpper = response.WideBoundUpperSeries.ToList();

            Assert.AreEqual(2, wideBoundUpper.Count);
            Assert.AreEqual(10m, wideBoundUpper[0]);
            Assert.AreEqual(11m, wideBoundUpper[1]);

            var wideBoundLower = response.WideBoundLowerSeries.ToList();

            Assert.AreEqual(2, wideBoundLower.Count);
            Assert.AreEqual(12m, wideBoundLower[0]);
            Assert.AreEqual(13m, wideBoundLower[1]);

            var narrowBoundUpper = response.NarrowBoundUpperSeries.ToList();

            Assert.AreEqual(2, narrowBoundUpper.Count);
            Assert.AreEqual(14m, narrowBoundUpper[0]);
            Assert.AreEqual(15m, narrowBoundUpper[1]);

            var narrowBoundLower = response.NarrowBoundLowerSeries.ToList();

            Assert.AreEqual(2, narrowBoundLower.Count);
            Assert.AreEqual(16m, narrowBoundLower[0]);
            Assert.AreEqual(17m, narrowBoundLower[1]);

            var years = response.Years.ToList();

            Assert.AreEqual(2, years.Count);
            Assert.AreEqual(0, years[0]);
            Assert.AreEqual(1, years[1]);

            var targetValue = response.TargetValue.ToList();

            Assert.AreEqual(2, targetValue.Count);
            Assert.AreEqual(18m, targetValue[0]);
            Assert.AreEqual(19m, targetValue[1]);
        }

        [TestMethod]
        public void CalculateProjection__ReturnsFailure__Correctly()
        {
            var request = new CalculationRequestDto(1m, 2m, 3, 4m, 5m, 6m, 7m, 8m);

            _result.SetupGet(g => g.Success).Returns(false);
            _result.SetupGet(g => g.ErrorMessage).Returns("Error message");

            _factory.Setup(m => m.MakeCalculation(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<int>(), It.IsAny<decimal>(),
                It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(_result.Object);

            var response = _controller.CalculateProjection(request);

            Assert.AreEqual(false, response.Success);
            Assert.AreEqual("Error message", response.ErrorMessage);
        }
    }
}
