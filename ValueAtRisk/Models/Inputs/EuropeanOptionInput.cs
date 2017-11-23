using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace ValueAtRisk.Models.Inputs
{
   public class EuropeanOptionInput
    {
        public double MarketUnderlyingPrice { get; set; }
        public double AssetPrice { get; set; }
        //number of business days from today until the averaging period ends</param>
        public double TtoMaturity { get; set; }
        public double RiskFreeRate { get; set; }
        public double CostOfCarry { get; set; }
        public double Volatility { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetPrice">the underlying price(settle price or forward price)</param>
        /// <param name="riskFreeRate">absolute value, 20% would be 0.2</param>
        /// <param name="costOfCarry"></param>
        /// <param name="volatility">absolute value, 20% would be 0.2</param>
        public EuropeanOptionInput(double assetPrice, DateTime endPricingDate, DateTime valuationDate, double riskFreeRate, double volatility, double costOfCarry = 0)
        {
            AssetPrice = assetPrice;
            TtoMaturity = (double)(Utilities.Utilities.BusinessDaysUntil(valuationDate, endPricingDate)) / Constants.DaysInYear;
            RiskFreeRate = Utilities.Utilities.ConvertToContinuous(riskFreeRate);
            CostOfCarry = Utilities.Utilities.ConvertToContinuous(costOfCarry);
            Volatility = volatility;
        }
    }
}
