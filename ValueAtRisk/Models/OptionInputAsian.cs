using System;
using System.Collections.Generic;
using System.Text;

namespace ValueAtRisk.Models
{
    public class OptionInputAsian:OptionInput
    {
        public double AverageSoFar { get; set; }
        public double TtoNextAverage { get; set; }
        public double NoOfFixings { get; set; }
        public int NoOfFixingsFixed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="averageSoFar">the average of already settled prices within the averaging period</param>
        /// <param name="tToNextAverage">number of business days until the averaging period's start</param>
        /// <param name="noOfFixings">number of business days within the averaging period</param>
        /// <param name="noOfFixingsFixed">number of days that have already settled within the averaging period</param>
        public OptionInputAsian(double assetPrice, double averageSoFar, DateTime startPricingDate, DateTime endPricingDate, DateTime valuationDate,double riskFreeRate,
            double volatility, int noOfFixingsFixed, double costOfCarry=0) 
            : base(assetPrice, endPricingDate, valuationDate, riskFreeRate,volatility,costOfCarry)
        {
            AverageSoFar = averageSoFar;
            //TODO
            //TtoNextAverage = ConvertToYears(startPricingDate,valuationDate);
            TtoNextAverage = ConvertToYears(valuationDate, startPricingDate);
            NoOfFixings = ConvertToYears(startPricingDate, endPricingDate);
            NoOfFixingsFixed = noOfFixingsFixed;
        }
    }
}
