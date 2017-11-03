using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValueAtRisk.Models;

namespace ValueAtRisk.Calculations
{
    public class MarkingToMarket
    {
        LinearPayoffCalc lpc = new LinearPayoffCalc();
        public MarkingToMarket(object transaction, Price price,string fromWhosPerspective="", OptionInput optionInput = null, OptionInputAsian optionInputAsian=null)
        {
            var a = transaction.GetType().FullName;
            OptionCalculator oc = new OptionCalculator();
            LinearPayoffCalc lc=new LinearPayoffCalc();
            //options calculator for MTM to be inserted here
            if (a == typeof(Option).FullName)
            {
                var instance = (Option)Activator.CreateInstance(a.GetType());
                if (instance.ChosenOptionStyle == Option.OptionStyle.Asian)
                {
                    oc.DiscreteAsianHHM(instance, optionInputAsian);
                }
                else if (instance.ChosenOptionStyle == Option.OptionStyle.European)
                {
                    oc.DiscreteEuropeanHHM(instance, optionInput);
                }
            }
            //use the linear payoff calculator for anything besides Options
            else
            {
                var res = (Transaction)Activator.CreateInstance(a.GetType());
                lc.MtmLinear(transaction: res,  price:price,fromWhosPerspective:"");
            }
        }
        /// <summary>
        /// This function calculates the option price for a given option style. Choose the type of formula for Asian style
        ///  by using the third argument "asianFormulaType". If you leave "asianFormulaType" blank, you will calculate using 
        /// the European option style. 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="optionInput"></param>
        /// <param name="asianFormulaType"></param>
        /// <returns></returns>
        public double CalculateMTMAsian(Option option, OptionInputAsian optionInput)
        {
            OptionCalculator optionCalc = new OptionCalculator();
            double res = 0;
            if (option.ChosenOptionStyle == Option.OptionStyle.Asian)
            {
                return optionCalc.DiscreteAsianHHM(option, optionInput);
            }
            return res;
        }
        public double CalculateMTMEuropean(Option option, OptionInput optionInput)
        {
            OptionCalculator optionCalc = new OptionCalculator();
            double res = 0;
            if (option.ChosenOptionStyle == Option.OptionStyle.European)
            {
                res = optionCalc.DiscreteEuropeanHHM(option, optionInput);
            }
            return res;
        }
        /// <summary>
        /// Calculate MTM for everything excluding option
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="marketPrice"></param>
        /// <param name="fromWhosPerspective"></param>
        /// <returns></returns>
        public double CalculateMTM(Transaction transaction, double marketPrice, string fromWhosPerspective="")
        {
            return lpc.MtmLinearPayoff(transaction, marketPrice, fromWhosPerspective);
        }

        public List<double> DistributionOfMTMLevels(object transaction, List<Price> simulatedPrices)
        {   List<double> listOfMTMs=new List<double>();
            //todo figure out the price of yesterday!
            var trans = (Transaction)Activator.CreateInstance(transaction.GetType());
            var mtmOfYesterday = lpc.MtmLinear(trans, simulatedPrices.FirstOrDefault());
            //get the list of all mtms for simulated prices

            foreach (var price in simulatedPrices)
            {
                var item=lpc.MtmLinear(trans, price);
                listOfMTMs.Add(item);
            }
            return listOfMTMs;
        }
        /// <summary>
        /// list of profit/losses for a list of mtm distribution levels
        /// </summary>
        /// <param name="price">price of yesterday</param>
        /// <param name="listOfMTMs">result from DistributionOfMTMLevels method</param>
        /// <returns></returns>
        public List<ProfitLossFromMTM> CalculateProfitLossForMTMs(double price, List<double> listOfMTMs)
        {
            List<ProfitLossFromMTM> profitLossList=new List<ProfitLossFromMTM>();
            foreach (var mtm in listOfMTMs)
            {
                var pl=new ProfitLossFromMTM(price,mtm);
                profitLossList.Add(pl);
            }
            return profitLossList;
        }
    }
}
