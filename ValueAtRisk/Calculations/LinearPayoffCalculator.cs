using System;
using System.Collections.Generic;
using System.Text;
using ValueAtRisk.Models;

namespace ValueAtRisk.Calculations
{
    public class LinearPayoffCalc
    {
        /// <summary>
        /// This function computes the MTM for transactions with a linear payoff
        /// Ex: Swaps, Futures, Broken Period Swaps 
        /// </summary>
        /// <param name="transaction">Transactions information for which the MTM is to be computed</param>
        /// <param name="marketPrice">The current market price of the product</param>
        /// <param name="fromWhosPerspective">The short name of the company from who's perspective we compute the MTM</param>
        /// <returns></returns>
        public double MtmLinearPayoff(Transaction transaction, double marketPrice, string fromWhosPerspective)
        {
            if (fromWhosPerspective.ToLower() == transaction.BuyerId)
            {
                return (marketPrice - transaction.FixedPrice) * transaction.Quantity;
            }
            return -(marketPrice - transaction.FixedPrice) * transaction.Quantity;
        }
        /// <summary>
        /// A method that calculates mtm of yesterday
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="price"></param>
        /// <param name="fromWhosPerspective"></param>
        /// <returns></returns>
        public double MtmLinear(Transaction transaction, Price price, string fromWhosPerspective="")
        {
           var mtm= transaction.Quantity * transaction.FixedPrice;
           return mtm;
        }

    }
}
