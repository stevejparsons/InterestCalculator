﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contract
{
    public interface IInterestCalculator
    {
        IEnumerable<decimal> Calculate(decimal lumpSumInvestment, decimal monthlyInvestment, decimal interestRatePercentage);
    }
}
