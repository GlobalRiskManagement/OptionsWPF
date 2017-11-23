using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace ValueAtRisk.Models.Inputs
{
    public class AsianOptionInput
    {
        public double MarketUnderlyingPrice { get; set; }
        public double AssetPrice { get; set; }
        //number of business days from today until the averaging period ends</param>
        public double TtoMaturity { get; set; }
        public double RiskFreeRate { get; set; }
        public double CostOfCarry { get; set; }
        public double Volatility { get; set; }
        public double AverageSoFar { get; set; }
        public double TtoNextAverage { get; set; }
        public int NoOfFixings { get; set; }
        public int NoOfFixingsFixed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="averageSoFar">the average of already settled prices within the averaging period</param>
        /// <param name="tToNextAverage">number of business days until the averaging period's start</param>
        /// <param name="noOfFixings">number of business days within the averaging period</param>
        //   /// <param name="noOfFixingsFixed">number of days that have already settled within the averaging period</param>
        public AsianOptionInput(double assetPrice, DateTime startPricingDate, DateTime endPricingDate, DateTime valuationDate, double riskFreeRate,
            double volatility, double costOfCarry = 0, double averageSoFar = 0)
        {
            DateTime d1= new DateTime(2017,11,24);
            DateTime d2= new DateTime(2017,11,25);
            var test= Utilities.Utilities.BusinessDaysUntil(d1,d2);
            AverageSoFar = averageSoFar;
            TtoNextAverage = (double)(Utilities.Utilities.BusinessDaysUntil(valuationDate, startPricingDate)) / Constants.DaysInYear;
            NoOfFixings = Utilities.Utilities.BusinessDaysUntil(startPricingDate, endPricingDate);
            //-1 indicates that the price on the valuation date is not yet fixed
            NoOfFixingsFixed = Math.Max(0,(Utilities.Utilities.BusinessDaysUntil(startPricingDate, valuationDate)-1));
            AssetPrice = assetPrice;
            TtoMaturity = (double)(Utilities.Utilities.BusinessDaysUntil(valuationDate, endPricingDate)) / Constants.DaysInYear;
            RiskFreeRate = Utilities.Utilities.ConvertToContinuous(riskFreeRate);
            CostOfCarry = Utilities.Utilities.ConvertToContinuous(costOfCarry);
            Volatility = volatility;
        }
    }
}
