using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contract
{
    public interface IInterestResult
    {
        bool Success { get; }

        decimal TotalInvested { get; }

        IEnumerable<decimal> WideBoundUpperSeries { get; }

        IEnumerable<decimal> WideBoundLowerSeries { get; }

        IEnumerable<decimal> NarrowBoundLowerSeries { get; }

        IEnumerable<decimal> NarrowBoundUpperSeries { get; }

        IEnumerable<int> Years { get; }

        IEnumerable<decimal> TargetValue { get; }

        string ErrorMessage { get; }
    }
}
