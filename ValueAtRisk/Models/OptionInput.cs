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
        //public double ConvertToYears(double input_t)
        //{
        //    return input_t / 365;
        //}
        //if the number is less than 1=>the time period is less than 1 year.
        public double ConvertToYears(DateTime date1, DateTime date2)
        {
            //TODO change to Utilities.Utilities.BusinessDaysCount when provided with a list of holidays
            var days = Utilities.Utilities.GetBusinessDays(date2, date1);
            double year = 365;
            double res = (double)(days/year);
            return res;
        }

        public double ConvertToContinuous(double rate)
        {
            return Math.Log(1 + rate);
        }
    }
}
