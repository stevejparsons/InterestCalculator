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
    }
}
