using System;
using System.Collections.Generic;
using System.Text;

namespace ValueAtRisk.Models
{
    public class MarkToMarketLevel
    {
        public double MTMAmount { get; set; }
        public DateTime PriceScenarioDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mtmAmount"></param>
        /// <param name="priceScenarioDate"></param>
        public MarkToMarketLevel(double mtmAmount, DateTime priceScenarioDate)
        {
            MTMAmount = mtmAmount;
            PriceScenarioDate = priceScenarioDate;
        }
    }
}
