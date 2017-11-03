using System;
using System.Collections.Generic;
using System.Text;

namespace ValueAtRisk.Models
{
    public class OptionInput
    {
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
        public OptionInput(double assetPrice,  DateTime endPricingDate, DateTime valuationDate, double riskFreeRate, double volatility, double costOfCarry = 0)
        {
            AssetPrice = assetPrice;
            //TODO
            // TtoMaturity = ConvertToYears(endPricingDate,valuationDate);
            TtoMaturity = ConvertToYears(valuationDate, endPricingDate);
            RiskFreeRate = ConvertToContinuous(riskFreeRate);
            CostOfCarry = ConvertToContinuous(costOfCarry);
            Volatility = volatility;
        }

        /// <summary>
        /// if the number is less than 1=>the time period is less than 1 year.
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public double ConvertToYears(DateTime startDate, DateTime endDate)
        {
            //TODO change to Utilities.Utilities.BusinessDaysCount when provided with a list of holidays
            var days = Utilities.Utilities.GetBusinessDays(endDate, startDate);
            double year = 365;
            double res = (double)(days / year);
            return res;
        }

        public double ConvertToContinuous(double rate)
        {
            return Math.Log(1 + rate);
        }
    }
}
