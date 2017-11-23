using System;
using System.Collections.Generic;
using System.Text;

namespace ValueAtRisk.Models.Inputs
{
    public class SwapInput
    {
       public double MarketUnderlyingPrice { get; set; }
        public SwapInput(double marketUnderlyingPrice)
        {
            MarketUnderlyingPrice = marketUnderlyingPrice;
        }
    }
}
