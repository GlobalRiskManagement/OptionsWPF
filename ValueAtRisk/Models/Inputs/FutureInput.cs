using System;
using System.Collections.Generic;
using System.Text;

namespace ValueAtRisk.Models.Inputs
{
   public class FutureInput
    {
        public double MarketUnderlyingPrice { get; set; }
        public FutureInput(double marketUnderlyingPrice)
        {
            MarketUnderlyingPrice = marketUnderlyingPrice;
        }
    }
}
