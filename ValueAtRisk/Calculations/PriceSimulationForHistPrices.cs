using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValueAtRisk.Models;

namespace ValueAtRisk.Calculations
{
    /// <summary>
    /// This class will use a distribution of returns from historic prices to simulate new possible prices.
    /// The main output of the class is a list of tuples. Each tuple consist of a date
    /// (the date from where the historic return was obtained) and a price (the price simulated by using that historic day's return).
    /// </summary>
    class PriceSimulationFromHistPrices
    //todo: rethink this class, see if you could make it more simple and straightforward
    {
        public PriceSimulationFromHistPrices(List<Price> historicPricesList,
            DateTime simualtionDate)
        {
            //Sort the list of tuples of dates/prices to be sure that we will use the possible historic scenarios chronologically
            //This step can be skiped if they are sorted when imported from DB
            //We use the lambda syntax and compare on the Item1 = date.
            HistoricPricesList = historicPricesList.OrderBy(price => price.Date).ToList();
            SimualtionDate = simualtionDate;
        }
        //The historic prices used as an input for the simulation of new prices
        public List<Price> HistoricPricesList;
        //The date for which we want to simulate prices for
        public DateTime SimualtionDate;
        //SimulatedPrices will have a private set as it is the main output of the class and none should temper with it
        public List<Price> SimulatedPrices { get; private set; }

        private int ComputeTimeScalingParam()
        {
            //Get the latest date for which we have any price
            List<DateTime> allDateTimes = HistoricPricesList.Select(price => price.Date).ToList();
            DateTime lastKnownPriceDate = allDateTimes.Max();
            //Compute the Time Scaling parameter for returns
            //It should be the number of business days between the last known price date and the simulation date
            //As BusinessDaysCount includes the first and last date, we have to reduce the interval by 1 day
            int timeScalingParam = Utilities.Utilities.BusinessDaysCount(lastKnownPriceDate, SimualtionDate) - 1;
            return timeScalingParam;
        }

        /// <summary>
        /// This function will scale all the returns in a list with time
        /// In other words it will multiple all the returns with the square root of time (in days)
        /// </summary>
        /// <param name="returnsList">The list of returns</param>
        /// <param name="timeScalingParam">The time scaling parameter in nr. of days</param>
        /// <returns>Returns a list of doubles</returns>
        private List<double> ScaleReturnsWithTime(IList<double> returnsList, int timeScalingParam = 1)
        {
            //multiply all the returns with the square root of time if we have to scale the returns
            if (timeScalingParam != 1)
            {
                double sqrTime = Math.Sqrt(timeScalingParam);
                returnsList = returnsList.Select(ret => ret * sqrTime).ToList();
            }
            return returnsList.ToList();
        }

        /// <summary>
        /// The function computes a set of new prices, based on the Last Known FixedPrice and a set of historic log returns
        /// </summary>
        /// <param name="lastKnownPrice">The latest available price in the market</param>
        /// <param name="historicLogReturnsList">A set of historic log returns</param>
        /// <returns>A list of simulated future prices based on historic returns and Last Known FixedPrice.</returns>
        private List<double> SimPricesFromHistReturns(double lastKnownPrice, IList<double> historicLogReturnsList)
        {
            double[] simulatedPricesArray = new double[historicLogReturnsList.Count];
            for (int i = 0; i < historicLogReturnsList.Count; i++)
            {
                simulatedPricesArray[i] = lastKnownPrice * Math.Exp(historicLogReturnsList[i]);
            }
            return simulatedPricesArray.ToList();
        }

        /// <summary>
        /// This function will simulate future prices by using the past prices.
        /// It will compute the historic returns and apply them to the latest known price from the list of historic prices.
        /// </summary>
        /// <param name="historicPricesList">The list of historic prices</param>
        /// <param name="timeScalingParam">Time scaling in nr of days. This is the number of days for which the return must be time scaled for
        /// For example if we want to simulate prices not for the next day but for 10 days from now we shall use sqr of time scaling to obtain it</param>
        /// <returns>Returns a list of simulated prices. 
        /// Keep in mind that the prices are simulated by using the historic returns chronologically.
        /// In other words the last price in the list is the one using the most recent return</returns>
        public List<double> SimPricesFromHistPrices(IList<Price> historicPricesList)
        {

            //get the Last Known FixedPrice
            Price lastKnownPrice = historicPricesList.OrderBy(price => price.Date).Last();
            //Compute the time scaling parameter
            int timeScalingParam = ComputeTimeScalingParam();
            //Compute the log returns from historic prices
            List<double> logReturns = Utilities.Utilities.LogRetuns(historicPricesList.Select(p => p.Value).ToList());
            //scale all the returns with time
            logReturns = ScaleReturnsWithTime(returnsList: logReturns, timeScalingParam: timeScalingParam);
            //simulate new prices by using historic log returns and the Last Known FixedPrice
            List<double> simulatedPrices = SimPricesFromHistReturns(lastKnownPrice: lastKnownPrice.Value, historicLogReturnsList: logReturns);
            return simulatedPrices;
        }
        /// <summary>
        /// This function will find out by how many days do we need to scale the historic returns
        /// It uses the date of the Last known price and the Simulation date to find the distance between them
        /// </summary>
        /// <returns>Returns an integer  which is the nr. of days for which the historic returns must be scaled for</returns>

        /// <summary>
        /// This function will simulate a future distribution of prices by keeping in mind the scaling of returns
        /// It will also attach return scenario days to the simulated prices
        /// </summary>
        /// <returns>Returns a dictionary with Key = Return scenario dates, Value = Simulated prices</returns>
        //todo: is this function necessary?
        public void SimPrices()
        {
            //Always run the simulation on a fresh list for results
            SimulatedPrices = new List<Price>();
            //simulate the new set of prices. Only pass the prices from each tuple to the SimPrices function
            List<double> simulatedPrices = SimPricesFromHistPrices(HistoricPricesList);


            //todo: introduce static dates for which the prices should have been asked for and add those to the final simualated price distribution
            //add back the scenario dates to the simualted prices
            //this way we know which return was used to simulate which price form the whole distribution
            List<DateTime> datesList = HistoricPricesList.Select(price => price.Date).ToList();
            //as each scenario is represented by a return and not a price there will be 1 less return than provided dates
            //therefore we have to skip one date from the Date/FixedPrice dictionary
            datesList = datesList.Skip(1).ToList();
            //combine prices with the dates for who's returns have been used to compute those prices
            for (int i = 0; i < simulatedPrices.Count; i++)
            {
                SimulatedPrices.Add(new Price(price: simulatedPrices[i], date: datesList[i]));
            }
        }
    }
}
