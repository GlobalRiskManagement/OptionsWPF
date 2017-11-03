using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace ValueAtRisk.Calculations
{
    public class VolatilityCalculator
    {
        /// <summary>
        /// A method that returns a list of variances, given a list of means. E.g. given a list of 6 means, we 
        /// should get a list of 5 variances.
        /// </summary>
        /// <param name="listOfMeans"></param>
        /// <returns></returns>
        public List<double> Variances(List<double> listOfMeans)
        {
            //get a list of all returns for the given means
            List<double> Returns = Utilities.Utilities.LogRetuns(listOfMeans);
            List<double> ListOfVariances = new List<double>();
            for (int index = 0; index < Returns.Count; index++)
            {
                if (index == 0)
                {
                    // return pow2
                    var firstVariance = Constants.Lambda * Math.Pow(Returns.FirstOrDefault(), 2) +
                                        (1 - Constants.Lambda) *
                                        Math.Pow(Returns.FirstOrDefault(), 2);
                    ListOfVariances.Add(firstVariance);
                }
                else if (index > 0)
                {
                    var nextVariance = Constants.Lambda * ListOfVariances[index - 1] + (1 - Constants.Lambda) *
                                       Math.Pow(Returns[index], 2);
                    ListOfVariances.Add(nextVariance);
                }
            }
            return ListOfVariances;
        }

        /// <summary>
        /// Returns a list of volatilities, given a list of variances(see previous method).
        /// The number of items in the list should be ==to the number of items in variances.
        /// </summary>
        /// <param name="listOfVariances"></param>
        /// <returns></returns>
        public List<double> Volatilities(List<double> listOfVariances)
        {
            List<double> volatilities = new List<double>();
            foreach (double index in listOfVariances)
            {
                var volatilityAtIndex = Math.Sqrt(index * 250);
                volatilities.Add(volatilityAtIndex);
            }
            return volatilities;
        }
    }
}
