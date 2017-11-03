using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValueAtRisk.Models;

namespace ValueAtRisk.Calculations
//todo: Ask for historic prices by asking for prices for specific dates instead of just asking for the last known 500 prices.
//todo: This way we will ask for the same dates for prices for all the products. Also use the same idea to follow historic returns into the ditribution of MTM level.
{
    //todo: make the MtmDistributionWithNetting class more clean/efficient
    //todo: maybe split it into MtmDistributionOneTrade and MtmDistributionSeveralTrades
    /// <summary>
    /// This class will compute a distribution of MTM levels for a set of transactions.
    /// The market levels for each transactions will be netted out across scenarios with the same dates.
    /// Thus one has to include only transactions for which he knows that netting is allowed.
    /// The inputs to the function should be:
    /// a. the transactions
    /// b. from who's perspective VAR is computed from
    /// c. Simulation date (the date in the future for which the VAR should be computed for)
    /// </summary>
    class MtmDistributionWithNetting
    {
        #region Properties
        public List<Transaction> Transactions;
        //The ID of the company from who's perspective the MTM should be computed from
        private string _fromWhosPerspective;
        //Private variable that will be set by a function
        private List<List<MtmValueOneTransaction>> _mtmDistribAllTransactionsWithDates;
        //Private variable that will be set by a function
        private List<double> _mtmDistribAllTransactionsWithNetting;
        public DateTime SimulationDate;
        //Create a default constructor that will ask for all the required inputs for the calculations in the class
        public MtmDistributionWithNetting(List<Transaction> transactions, string fromWhosPerspective, DateTime simualationDate)
        {
            Transactions = transactions;
            FromWhosPerspective = fromWhosPerspective;
            SimulationDate = simualationDate;
        }
        //The transactions for which the total MTM should be computed for     
        //Public getter for the private variable _mtmDistribAllTransactionsWithNetting
        public List<double> MtmDistribAllTransactionsWithNetting { get { return _mtmDistribAllTransactionsWithNetting; } }
        //Public getter for the private variable _mtmDistribAllTransactionsWithNetting
        public List<List<MtmValueOneTransaction>> MtmDistribAllTransactionsWithDates { get { return _mtmDistribAllTransactionsWithDates; } }
        //public getter and setter for _fromWhosPerspective. While setting the string will convert it to lower case.
        public string FromWhosPerspective
        {
            get {return _fromWhosPerspective;}
            set {_fromWhosPerspective = value.ToLower();}
        }
        #endregion

        #region Functions Several Trades
        /// <summary>
        /// This function computes a distribution of MTM levels according to a set of simulated market prices
        /// The function applies to contracts with a linear payoff only (to be extended in the future to non-linear payoff as well)
        /// </summary>
        /// <param name="transaction">The transactions information for which MTM is to be computed for</param>
        /// <param name="simulatedPricesList">The set of simulated market prices</param>
        /// <returns>Returns a list of MTM levels</returns>
        private List<double> MtmDistribLinearPayoff(Transaction transaction, IList<double> simulatedPricesList)
        {
            double[] mtmDistributionList = new double[simulatedPricesList.Count];
            for (int i = 0; i < simulatedPricesList.Count; i++)
            {
                mtmDistributionList[i] = MarkingToMarket.CalculateMTM(
                    transaction: transaction,
                    marketPrice: simulatedPricesList[i],
                    fromWhosPerspective: _fromWhosPerspective);
            }
            return mtmDistributionList.ToList();
        }
        /// <summary>
        /// This function computes a distribution of MTM levels according to the transactions type and a set of simulated market prices.
        /// </summary>
        /// <param name="transaction">The transactions information for which MTM is to be computed for</param>
        /// <param name="simulatedPricesList">The set of simulated market prices</param>
        /// <param name="fromWhosPerspective">The ID of the company from who's perspective the MTM shoudl be computed from</param>
        /// <returns>Returns a list of MTM levels</returns>
        public List<double> MtmDistributionAllTypeTransactions(object transaction, IList<double> simulatedPricesList,OptionInput optionInput= null)
        {
             var a = transaction.GetType().FullName;
            OptionCalculator oc=new OptionCalculator();
             //options calculator for MTM to be inserted here
             if (a==typeof(Option).FullName)
             {
                 var instance = (Option)Activator.CreateInstance(a.GetType());
                 if (instance.ChosenOptionStyle==Option.OptionStyle.Asian)
                 {
                   oc.DiscreteAsianHHM(instance, optionInput);
                 }
                 else if (instance.ChosenOptionStyle == Option.OptionStyle.European)
                 {
                     oc.DiscreteEuropeanHHM(instance, optionInput);
                 }
             }
                //use the linear payoff calculator for anything besides Options
             else
             {
                 var res = (Transaction) Activator.CreateInstance(a.GetType());
                 return MtmDistribLinearPayoff(transaction:res, simulatedPricesList: simulatedPricesList);
             }
        }
        /// <summary>
        /// This function will work exactly like MtmDistribAllTransactions but will keep the dates of returns used to compute MTM levels
        /// </summary>
        /// <returns>Returns a list of lists. Each nested list is the historic MTM Distribution for each transactions</returns>
        public List<List<MtmValueOneTransaction>> ComputeMtmDistribAllTransactionsWithDates()
        {

            List<List<MtmValueOneTransaction>> _mtmDistribAllTransactionsWithDates = new List<List<MtmValueOneTransaction>>();
            foreach (Transaction transaction in Transactions)
            {
                _mtmDistribAllTransactionsWithDates.Add(HistoricMtmDistribOneTransactionWithDates(transaction: transaction));
            }
            return _mtmDistribAllTransactionsWithDates;
        }
        /// <summary>
        /// The function will apply the MTM distribution function for all given transactions
        /// </summary>
        /// <returns>Returns a list of lists. Each nested list is the historic MTM Distribution for each transactions</returns>
        private List<List<double>> MtmDistribAllTransactions()
        {
            List<List<double>> mtmDistribAllTransactions = new List<List<double>>();
            foreach (Transaction transaction in Transactions)
            {
                mtmDistribAllTransactions.Add(HistoricMtmDistribOneTransaction(transaction: transaction));
            }
            return mtmDistribAllTransactions;
        }
        /// <summary>
        /// The function will compute the sum of historic MTM distribution for all given transactions for each historic market scenario.
        /// It can be applied as a VAR for one company only as it summs up all the MTM distributions.
        /// (Remember that netting out exposures from different counterparts is wrong)
        /// </summary>
        public void ComputeMtmDistribAllTransactionsWithNetting()
        {
            _mtmDistribAllTransactionsWithNetting = Utilities.Utilities.SumElementsAtSameIndex(MtmDistribAllTransactions());
        }
        #endregion

        #region Functions One Trade
        //The following two functions are made just for testing purposes of VAR
        /// <summary>
        /// This function will work exactly like HistoricMtmDistribOneTransaction but will keep the dates of returns used to compute MTM levels
        /// </summary>
        /// <param name="transaction">The transactions information for which the MTM should be computed for</param>
        /// <returns>Returns a list of MTM levels, one for each historic return scenario</returns>
        //todo: replace the Tuple<DateTime, double> with MtmValueOneTransaction and the List<Tuple<DateTime, double>> with a class named MtmDistribOneTransaction
        private MtmDistribOneTransaction HistoricMtmDistribOneTransactionWithDates(Transaction transaction)
        {
            //TODO:INSERT AN SQL QUERY HERE TO IMPORT THE CORRECT PRICES BASED ON TRADE INFO
            List<Price> historicPrices = new List<Price>();
            //simulate prices by using historic approach
            PriceSimulationFromHistPrices priceSimulationFromHistPrices = new PriceSimulationFromHistPrices(historicPrices, SimulationDate);
            priceSimulationFromHistPrices.SimPrices();
            List<Price> simulatedPrices = priceSimulationFromHistPrices.SimulatedPrices;

            List<double> mtmDistributionOneTransaction = MtmDistributionAllTypeTransactions(transaction: transaction,
                simulatedPricesList: simulatedPrices.Select(price => price.Value).ToList());

            //TODO:insert a code that will convert the MtmDistributionAllTypeTransactions to usd from any other fx
            //Recombine the historic dates for who's returns have been used to compute
            //the future simulated MTM levels with the actual MTM levels
            List<DateTime> listOfDates = simulatedPrices.Select(price => price.Date).ToList();
            var tupleList = Utilities.Utilities.TwoListsToListOfTuples(listOfDates, mtmDistributionOneTransaction);
            //convert the list of touples to a list of type MtmDistribOneTransaction
            MtmDistribOneTransaction historicMtmDistribOneTransaction =new MtmDistribOneTransaction(
                tupleList.Select(t => new MtmValueOneTransaction(t)).ToList());
            return historicMtmDistribOneTransaction;
        }
        /// <summary>
        /// A function that will run all the required steps to compute a MTM distribution by using historic returns as an input
        /// </summary>
        /// <param name="transaction">The transactions information for which the MTM should be computed for</param>
        /// <returns>Returns a list of MTM levels, one for each historic return scenario</returns>
        private List<double> HistoricMtmDistribOneTransaction(Transaction transaction)
        {
            //TODO:INSERT AN SQL QUERY HERE TO IMPORT THE CORRECT PRICES BASED ON TRADE INFO
            List<Price> historicPrices = new List<Price>();
            //simulate prices by using historic approach
            PriceSimulationFromHistPrices priceSimulationFromHistPrices = new PriceSimulationFromHistPrices(historicPrices, SimulationDate);
            priceSimulationFromHistPrices.SimPrices();
            List<Price> simulatedPrices = priceSimulationFromHistPrices.SimulatedPrices;

            List<double> mtmDistributionOneTransaction = MtmDistributionAllTypeTransactions(transaction: transaction,
                simulatedPricesList: simulatedPrices.Select(price => price.Value).ToList());

            //TODO:insert a code that will convert the MtmDistributionAllTypeTransactions to usd from any other fx
            return mtmDistributionOneTransaction;
        }
        #endregion
    }
}
