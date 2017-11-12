using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace BusinessLayer.Tests
{
    [TestClass]
    public class InterestCalculatorTests
    {
        private InterestCalculator _interestCalculator;

        [TestInitialize]
        public void Init()
        {
            _interestCalculator = new InterestCalculator();
        }

        [TestMethod]
        public void Calculate__100000LumpSum_2000MonthlyInvestment_ZeroPercent__CalculatesFirst10YearsCorrectly()
        {
            var interest = _interestCalculator.Calculate(100000m, 2000, 0);

            var firstTenYears = interest.Take(10).ToList();

            Assert.AreEqual(124000m, firstTenYears[0]);
            Assert.AreEqual(148000m, firstTenYears[1]);
            Assert.AreEqual(172000m, firstTenYears[2]);
            Assert.AreEqual(196000m, firstTenYears[3]);
            Assert.AreEqual(220000m, firstTenYears[4]);
            Assert.AreEqual(244000m, firstTenYears[5]);
            Assert.AreEqual(268000m, firstTenYears[6]);
            Assert.AreEqual(292000m, firstTenYears[7]);
            Assert.AreEqual(316000m, firstTenYears[8]);
            Assert.AreEqual(340000m, firstTenYears[9]);
        }

        [TestMethod]
        public void Calculate__100000LumpSum_2000MonthlyInvestment_ZeroPercent__CalculatesNext10YearsCorrectly()
        {
            var interest = _interestCalculator.Calculate(100000m, 2000, 0);

            var firstTenYears = interest.Skip(10).Take(10).ToList();

            Assert.AreEqual(364000m, firstTenYears[0]);
            Assert.AreEqual(388000m, firstTenYears[1]);
            Assert.AreEqual(412000m, firstTenYears[2]);
            Assert.AreEqual(436000m, firstTenYears[3]);
            Assert.AreEqual(460000m, firstTenYears[4]);
            Assert.AreEqual(484000m, firstTenYears[5]);
            Assert.AreEqual(508000m, firstTenYears[6]);
            Assert.AreEqual(532000m, firstTenYears[7]);
            Assert.AreEqual(556000m, firstTenYears[8]);
            Assert.AreEqual(580000m, firstTenYears[9]);
        }

        [TestMethod]
        public void Calculate__100000LumpSum_NoMonthlyInvestment_5Percent__CalculatesFirst10YearsCorrectly()
        {
            var interest = _interestCalculator.Calculate(100000m, 0, 5);

            var firstTenYears = interest.Take(10).ToList();

            Assert.AreEqual(105000m, firstTenYears[0]);
            Assert.AreEqual(110250m, firstTenYears[1]);
            Assert.AreEqual(115762.5m, firstTenYears[2]);
            Assert.AreEqual(121550.63m, firstTenYears[3]);
            Assert.AreEqual(127628.16m, firstTenYears[4]);
            Assert.AreEqual(134009.56m, firstTenYears[5]);
            Assert.AreEqual(140710.04m, firstTenYears[6]);
            Assert.AreEqual(147745.54m, firstTenYears[7]);
            Assert.AreEqual(155132.82m, firstTenYears[8]);
            Assert.AreEqual(162889.46m, firstTenYears[9]);
        }

        [TestMethod]
        public void Calculate__100000LumpSum_NoMonthlyInvestment_5Percent__CalculatesNext10YearsCorrectly()
        {
            var interest = _interestCalculator.Calculate(100000m, 0, 5);

            var firstTenYears = interest.Skip(10).Take(10).ToList();

            Assert.AreEqual(171033.94m, firstTenYears[0]);
            Assert.AreEqual(179585.63m, firstTenYears[1]);
            Assert.AreEqual(188564.91m, firstTenYears[2]);
            Assert.AreEqual(197993.16m, firstTenYears[3]);
            Assert.AreEqual(207892.82m, firstTenYears[4]);
            Assert.AreEqual(218287.46m, firstTenYears[5]);
            Assert.AreEqual(229201.83m, firstTenYears[6]);
            Assert.AreEqual(240661.92m, firstTenYears[7]);
            Assert.AreEqual(252695.02m, firstTenYears[8]);
            Assert.AreEqual(265329.77m, firstTenYears[9]);
        }

        [TestMethod]
        public void Calculate__100000LumpSum_1000MonthlyInvestment_5Percent__CalculatesFirst10YearsCorrectly()
        {
            var interest = _interestCalculator.Calculate(100000m, 1000, 5);

            var firstTenYears = interest.Take(10).ToList();

            Assert.AreEqual(117272.58m, firstTenYears[0]);
            Assert.AreEqual(135408.78m, firstTenYears[1]);
            Assert.AreEqual(154451.80m, firstTenYears[2]);
            Assert.AreEqual(174446.97m, firstTenYears[3]);
            Assert.AreEqual(195441.89m, firstTenYears[4]);
            Assert.AreEqual(217486.57m, firstTenYears[5]);
            Assert.AreEqual(240633.47m, firstTenYears[6]);
            Assert.AreEqual(264937.72m, firstTenYears[7]);
            Assert.AreEqual(290457.19m, firstTenYears[8]);
            Assert.AreEqual(317252.62m, firstTenYears[9]);
        }

        [TestMethod]
        public void Calculate__100000LumpSum_1000MonthlyInvestment_5Percent__CalculatesNext10YearsCorrectly()
        {
            var interest = _interestCalculator.Calculate(100000m, 1000, 5);

            var firstTenYears = interest.Skip(10).Take(10).ToList();

            Assert.AreEqual(345387.83m, firstTenYears[0]);
            Assert.AreEqual(374929.80m, firstTenYears[1]);
            Assert.AreEqual(405948.87m, firstTenYears[2]);
            Assert.AreEqual(438518.89m, firstTenYears[3]);
            Assert.AreEqual(472717.41m, firstTenYears[4]);
            Assert.AreEqual(508625.86m, firstTenYears[5]);
            Assert.AreEqual(546329.73m, firstTenYears[6]);
            Assert.AreEqual(585918.80m, firstTenYears[7]);
            Assert.AreEqual(627487.31m, firstTenYears[8]);
            Assert.AreEqual(671134.26m, firstTenYears[9]);
        }
    }
}