using BusinessLayer.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebService.Contract;

namespace BusinessLayer.Tests
{

    [TestClass]
    public class InterestFactoryTests
    {
        private InterestFactory _interestFactory;
        private Mock<IInterestCalculator> _interestCalculatorMock;

        [TestInitialize]
        public void Init()
        {
            _interestCalculatorMock = new Mock<IInterestCalculator>();

            _interestFactory = new InterestFactory(_interestCalculatorMock.Object);
        }

        [TestMethod]
        public void Make__ReturnsError_WhenLumpSumInvestment__IsLessThanZero()
        {
            var result = _interestFactory.MakeCalculation(-1, 1, 1, 4, 1, 3, 2, 10);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual("LumpSumInvestment cannot be less than zero", result.ErrorMessage);
        }

        [TestMethod]
        public void Make__ReturnsError_WhenLumpSumInvestment__IsGreaterThan1000000000()
        {
            var result = _interestFactory.MakeCalculation(1000000001, 1, 1, 4, 1, 3, 2, 10);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual("LumpSumInvestment cannot be greater than 1000000000", result.ErrorMessage);
        }

        [TestMethod]
        public void Make__ReturnsError_WhenMonthlyInvestment__IsLessThanZero()
        {
            var result = _interestFactory.MakeCalculation(1, -1, 1, 4, 1, 3, 2, 10);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual("MonthlyInvestment cannot be less than zero", result.ErrorMessage);
        }

        [TestMethod]
        public void Make__ReturnsError_WhenMonthlyInvestment__IsGreaterThan1000000000()
        {
            var result = _interestFactory.MakeCalculation(1, 1000000001, 1, 4, 1, 3, 2, 10);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual("MonthlyInvestment cannot be greater than 1000000000", result.ErrorMessage);
        }

        [TestMethod]
        public void Make__CallsInterestCalculator__Correctly()
        {
            var interestData = new List<decimal>() { 1m, 2m, 3m, 4m, 5m, 6m, };

            _interestCalculatorMock.Setup(m => m.Calculate(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(interestData);

            var result = _interestFactory.MakeCalculation(1000, 100, 5, 4, 1, 3, 2, 10);

            _interestCalculatorMock.Verify(m => m.Calculate(1000, 100, 4), Times.Once);
            _interestCalculatorMock.Verify(m => m.Calculate(1000, 100, 1), Times.Once);
            _interestCalculatorMock.Verify(m => m.Calculate(1000, 100, 3), Times.Once);
            _interestCalculatorMock.Verify(m => m.Calculate(1000, 100, 2), Times.Once);
        }

        [TestMethod]
        public void Make__ReturnsThe_Success_ErrorMessage_TotalInvested_Results__Correctly()
        {
            SetupCalculateResults();

            var calculation = _interestFactory.MakeCalculation(1000m, 100m, 5, 4m, 1m, 3m, 2m, 2000m);

            Assert.AreEqual(true, calculation.Success);
            Assert.AreEqual(string.Empty, calculation.ErrorMessage);

            Assert.AreEqual(0.5m, calculation.TotalInvested);
        }

        [TestMethod]
        public void Make__ReturnsTheWideBandLowerResult__Correctly()
        {
            SetupCalculateResults();

            var calculation = _interestFactory.MakeCalculation(1000m, 100m, 5, 4m, 1m, 3m, 2m, 2000m);

            var wideBoundLowerSeries = calculation.WideBoundLowerSeries.ToList();

            Assert.AreEqual(5, wideBoundLowerSeries.Count);
            Assert.AreEqual(1.1m, wideBoundLowerSeries[0]);
            Assert.AreEqual(1.2m, wideBoundLowerSeries[1]);
            Assert.AreEqual(1.3m, wideBoundLowerSeries[2]);
            Assert.AreEqual(1.4m, wideBoundLowerSeries[3]);
            Assert.AreEqual(1.5m, wideBoundLowerSeries[4]);
        }

        [TestMethod]
        public void Make__ReturnsTheNarrowBandLowerResult__Correctly()
        {
            SetupCalculateResults();

            var calculation = _interestFactory.MakeCalculation(1000m, 100m, 5, 4m, 1m, 3m, 2m, 2000m);

            var narrowBoundLowerSeries = calculation.NarrowBoundLowerSeries.ToList();

            Assert.AreEqual(5, narrowBoundLowerSeries.Count);
            Assert.AreEqual(2.1m, narrowBoundLowerSeries[0]);
            Assert.AreEqual(2.2m, narrowBoundLowerSeries[1]);
            Assert.AreEqual(2.3m, narrowBoundLowerSeries[2]);
            Assert.AreEqual(2.4m, narrowBoundLowerSeries[3]);
            Assert.AreEqual(2.5m, narrowBoundLowerSeries[4]);
        }

        [TestMethod]
        public void Make__ReturnsTheNarrowBandUpperResult__Correctly()
        {
            SetupCalculateResults();

            var calculation = _interestFactory.MakeCalculation(1000m, 100m, 5, 4m, 1m, 3m, 2m, 2000m);

            var narrowBoundUpperSeries = calculation.NarrowBoundUpperSeries.ToList();

            Assert.AreEqual(5, narrowBoundUpperSeries.Count);
            Assert.AreEqual(3.1m, narrowBoundUpperSeries[0]);
            Assert.AreEqual(3.2m, narrowBoundUpperSeries[1]);
            Assert.AreEqual(3.3m, narrowBoundUpperSeries[2]);
            Assert.AreEqual(3.4m, narrowBoundUpperSeries[3]);
            Assert.AreEqual(3.5m, narrowBoundUpperSeries[4]);
        }

        [TestMethod]
        public void Make__ReturnsTheWideBandUpperResult__Correctly()
        {
            SetupCalculateResults();

            var calculation = _interestFactory.MakeCalculation(1000m, 100m, 5, 4m, 1m, 3m, 2m, 2000m);

            var wideBoundUpperSeries = calculation.WideBoundUpperSeries.ToList();

            Assert.AreEqual(5, wideBoundUpperSeries.Count);
            Assert.AreEqual(4.1m, wideBoundUpperSeries[0]);
            Assert.AreEqual(4.2m, wideBoundUpperSeries[1]);
            Assert.AreEqual(4.3m, wideBoundUpperSeries[2]);
            Assert.AreEqual(4.4m, wideBoundUpperSeries[3]);
            Assert.AreEqual(4.5m, wideBoundUpperSeries[4]);
        }

        [TestMethod]
        public void Make__ReturnsTheYearResult__Correctly()
        {
            SetupCalculateResults();

            var calculation = _interestFactory.MakeCalculation(1000m, 100m, 5, 4m, 1m, 3m, 2m, 2000m);

            var years = calculation.Years.ToList();

            Assert.AreEqual(5, years.Count);
            Assert.AreEqual(0, years[0]);
            Assert.AreEqual(1, years[1]);
            Assert.AreEqual(2, years[2]);
            Assert.AreEqual(3, years[3]);
            Assert.AreEqual(4, years[4]);
        }

        [TestMethod]
        public void Make__ReturnsTheTargetValueResult__Correctly()
        {
            SetupCalculateResults();

            var calculation = _interestFactory.MakeCalculation(1000m, 100m, 5, 4m, 1m, 3m, 2m, 2000m);

            var targetValue = calculation.TargetValue.ToList();

            Assert.AreEqual(5, targetValue.Count);
            Assert.AreEqual(2000m, targetValue[0]);
            Assert.AreEqual(2000m, targetValue[1]);
            Assert.AreEqual(2000m, targetValue[2]);
            Assert.AreEqual(2000m, targetValue[3]);
            Assert.AreEqual(2000m, targetValue[4]);
        }

        private void SetupCalculateResults()
        {
            _interestCalculatorMock.Setup(m => m.Calculate(
                It.Is<decimal>(d => (d == 1000m)),
                It.Is<decimal>(d => (d == 100m)),
                It.Is<decimal>(d => (d == 0m))))
                    .Returns(new List<decimal>() { 0.1m, 0.2m, 0.3m, 0.4m, 0.5m });

            _interestCalculatorMock.Setup(m => m.Calculate(
                It.Is<decimal>(d => (d == 1000m)),
                It.Is<decimal>(d => (d == 100m)),
                It.Is<decimal>(d => (d == 1m))))
                    .Returns(new List<decimal>() { 1.1m, 1.2m, 1.3m, 1.4m, 1.5m });

            _interestCalculatorMock.Setup(m => m.Calculate(
                It.Is<decimal>(d => (d == 1000m)),
                It.Is<decimal>(d => (d == 100m)),
                It.Is<decimal>(d => (d == 2m))))
                    .Returns(new List<decimal>() { 2.1m, 2.2m, 2.3m, 2.4m, 2.5m });

            _interestCalculatorMock.Setup(m => m.Calculate(
                It.Is<decimal>(d => (d == 1000m)),
                It.Is<decimal>(d => (d == 100m)),
                It.Is<decimal>(d => (d == 3m))))
                    .Returns(new List<decimal>() { 3.1m, 3.2m, 3.3m, 3.4m, 3.5m });

            _interestCalculatorMock.Setup(m => m.Calculate(
                It.Is<decimal>(d => (d == 1000m)),
                It.Is<decimal>(d => (d == 100m)),
                It.Is<decimal>(d => (d == 4m))))
                    .Returns(new List<decimal>() { 4.1m, 4.2m, 4.3m, 4.4m, 4.5m });
        }
    }
}
