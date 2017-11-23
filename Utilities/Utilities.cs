using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public static class Utilities
    {

        /// <summary>
        /// this function has been copied from ValueAtRisk.MathRisk as the reference did not work. It shoudl be deleted after refractoring.
        /// </summary>
        /// <param name="timeSeries"></param>
        /// <returns></returns>
        public static List<double> LogRetuns(IList<double> timeSeries)
        {
            List<double> logReturnsSeries = new List<double>();
            for (int i = 0; i < timeSeries.Count - 1; i++)
            {
                logReturnsSeries.Add(Math.Log(timeSeries[i + 1] / timeSeries[i]));
            }
            return logReturnsSeries;
        }
        /// <summary>
        /// This function computes a set of exponential probability weights.
        /// The formula for weights is: (1-lambda)*lambda^i
        /// </summary>
        /// <param name="lambda">The exponential smoothing parameter, to be input between 0.9 and 1</param>
        /// <param name="nrOfWeightsNeeded">The number of required exponential weights</param>
        /// <returns>Will return a list of Exponential weights</returns>
        static List<double> ExponentialProbWeights(double lambda, int nrOfWeightsNeeded)
        {
            double[] exponentialProbWeights = new double[nrOfWeightsNeeded];
            for (int i = 0; i < nrOfWeightsNeeded; i++)
            {
                exponentialProbWeights[i] = Math.Pow(lambda, i) * (1 - lambda);
            }
            return exponentialProbWeights.ToList();
        }
        /// <summary>
        /// The function will return a CHRONOLOGIC list of exponential weights
        /// Chronologic: last weights is the weight for the most recent outcome
        /// </summary>
        /// <param name="lambda">The exponential smoothing parameter</param>
        /// <param name="nrOfWeightsNeeded">The number of exponential weights you require</param>
        /// <returns></returns>
        static List<double> ExponentialProbWeightsChronologic(double lambda, int nrOfWeightsNeeded)
        {
            List<double> exponentialProbWeightsList = ExponentialProbWeights(lambda: lambda,
                nrOfWeightsNeeded: nrOfWeightsNeeded);
            exponentialProbWeightsList.Reverse();
            return exponentialProbWeightsList;
        }
        /// <summary>
        /// The function will check if all the lists within a list are of same lenght
        /// </summary>
        /// <param name="listOfLists"></param>
        /// <returns>Returns true of they are of same lenght and false otherwise</returns>
        public static bool CheckListsForSameLenght(IList<List<double>> listOfLists)
        {
            for (int i = 0; i < listOfLists.Count - 1; i++)
            {
                if (listOfLists[i].Count != listOfLists[i + 1].Count)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// This function will extract the elements at the same index from several lists
        /// </summary>
        /// <param name="listOfLists">The container of lists to work on</param>
        /// <param name="indexNr">The index at which the elements should be copied. Remember that index numbers start at 0 and not 1</param>
        /// <returns>Returns a list with extracted element from each separate list</returns>
        public static List<double> ExtractElementsAtSomeIndex(IList<List<double>> listOfLists, int indexNr)
        {
            double[] workArray = new double[listOfLists.Count];
            for (int i = 0; i < listOfLists.Count; i++)
            {
                workArray[i] = listOfLists[i][indexNr];
            }
            return workArray.ToList();
        }
        /// <summary>
        /// This function will sum all elements at the same index of a list of lists.
        /// In other words, for a 2d array it will sum the elements on the same row.
        /// </summary>
        /// <param name="listOfLists">The input 2d array as list of lists</param>
        /// <returns>Returns a list with each element being a sum at the same index across several lists</returns>
        public static List<double> SumElementsAtSameIndex(IList<List<double>> listOfLists)
        {
            if (CheckListsForSameLenght(listOfLists))
            {
                throw new Exception("Not all lists are of the same lenght");
            }
            double[] wokringArray = new double[listOfLists[0].Count];
            for (int indexNr = 0; indexNr < listOfLists[0].Count; indexNr++)
            {
                double sum = listOfLists.Sum(list => list[indexNr]);
                wokringArray[indexNr] = sum;
            }
            return wokringArray.ToList();
        }
        /// <summary>
        /// The function will asign probability weights to the a list of MTM values.
        /// Note: MTM values should be chronologically aranged (last MTM in the list is computed with the most recent market scenario)
        /// </summary>
        /// <param name="mtmDistributionList">The distribution of MTM values</param>
        /// <param name="lambda">The exponential smoothing parameter, between 0 and 1</param>
        /// <returns></returns>
        static Dictionary<double, double> AssignExpProbWToMtmDistrib(IList<double> mtmDistributionList,
            double lambda)
        {
            List<double> exponentialProbWeightsChronologic = ExponentialProbWeightsChronologic(lambda: lambda,
                nrOfWeightsNeeded: mtmDistributionList.Count);
            Dictionary<double, double> mtmWithProbWeightsDictionary = new Dictionary<double, double>();
            for (int i = 0; i < mtmDistributionList.Count; i++)
            {
                mtmWithProbWeightsDictionary.Add(exponentialProbWeightsChronologic[i], mtmDistributionList[i]);
            }
            return mtmWithProbWeightsDictionary;
        }
        /// <summary>
        /// The function will sort a inputDictionary ascendingly by the Value of each KeyValue pair
        /// </summary>
        /// <param name="dictioary">The inputDictionary to be sorted</param>
        /// <returns>Returns a sorted inputDictionary</returns>
        public static Dictionary<double, double> SortDictionaryByValues(Dictionary<double, double> dictioary)
        {
            //createa a Linq list made up of ordered key-val pair while orderin them by Value
            var linqList = dictioary.OrderBy(keyValPair => keyValPair.Value);
            //create a new inputDictionary and fill it with key value pairs from above ordered Linq list
            Dictionary<double, double> sortedDictionary = new Dictionary<double, double>();
            foreach (var keyValuePair in linqList)
            {
                sortedDictionary.Add(keyValuePair.Key, keyValuePair.Value);
            }
            return sortedDictionary;
        }
        /// <summary>
        ///This function will sort a dictionary by its keys
        /// </summary>
        /// <param name="inputDictionary">The dictionary to sort</param>
        /// <returns>Returns a sorted dictionary with keys in ascedning order</returns>
        public static Dictionary<DateTime, double> SortDictionaryByKeys(Dictionary<DateTime, double> inputDictionary)
        {
            Dictionary<DateTime, double> sortedDictionary = new Dictionary<DateTime, double>();
            List<DateTime> keysList = inputDictionary.Keys.ToList();
            keysList.Sort();
            foreach (DateTime key in keysList)
            {
                sortedDictionary.Add(key, inputDictionary[key]);
            }
            return sortedDictionary;
        }
        /// <summary>
        /// This function chooses a VAR level amongst MTM scenarios by using the corresponding prob weights
        /// Basically it sorts the MTM levels in an ascending order and 
        /// sums the corresponding prob wieghts untill it reaches the desired confidence level.
        /// </summary>
        /// <param name="confidenceLevel">A confidence level between 0 and 1. 
        /// Ex: If the confidence level is 95% then a 5% MTM will be chosen from the left tail of the distribution</param>
        /// <param name="mtmWithProbWsDict">A inputDictionary with MTMs and corresponding probability weights</param>
        /// <returns>A number representing the VAR level</returns>
        static double ChooseVarByCummulatingProbWeights(double confidenceLevel,
            Dictionary<double, double> mtmWithProbWsDict)
        {
            Dictionary<double, double> sortedMtmWithProbWsDict = SortDictionaryByValues(dictioary: mtmWithProbWsDict);
            double probTarget = 1 - confidenceLevel;
            double probSum = 0;
            foreach (var keyValPair in sortedMtmWithProbWsDict)
            {
                probSum += keyValPair.Key;
                if (probSum >= probTarget)
                {
                    return keyValPair.Value;
                }
            }
            //Proper exception handler to be created here
            //exception text shoudl be: "the probability sum does not converge to the desired confidence level"
            throw new Exception();
        }
        /// <summary>
        /// The function will choose a VAR level from a mark to market distribution by using exponential probability weights.
        /// Exponential probability weights depend on how recent is the market scenario that was used to compute the MTM
        /// The function computes the exponential probability weights, assignes them to MTM levels and chooses a specific MTM as VAR.
        /// </summary>
        /// <param name="mtmDistributionList">The list of simulated MTM levels</param>
        /// <param name="lambda">The exponential smoothing parameter, between 0 and 1</param>
        /// <param name="confidenceLevel">A confidence level between 0 and 1. 
        /// Ex: If the confidence level is 95% then a 5% MTM will be chosen from the left tail of the distribution</param>
        /// <returns></returns>
        public static double ChooseVarByExpProbWeights(List<double> mtmDistributionList, double lambda,
            double confidenceLevel)
        {
            Dictionary<double, double> mtmWithProbWeightsDict =
                AssignExpProbWToMtmDistrib(mtmDistributionList: mtmDistributionList, lambda: lambda);
            double var =
                ChooseVarByCummulatingProbWeights(confidenceLevel: confidenceLevel, mtmWithProbWsDict: mtmWithProbWeightsDict);
            return var;
        }
        /// <summary>
        /// Calculates number of business days, taking into account:
        ///  - weekends (Saturdays and Sundays)
        ///  - bank holidays in the middle of the week
        /// </summary>
        /// <param name="firstDay">First day in the time interval</param>
        /// <param name="lastDay">Last day in the time interval</param>
        /// <param name="bankHolidays">List of bank holidays excluding weekends</param>
        /// <returns>Number of business days between the two dates and including them</returns>
        public static int BusinessDaysCount(DateTime firstDay, DateTime lastDay, params DateTime[] bankHolidays)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
                throw new ArgumentException("First date " + firstDay.ToString("d") + " should be before the last date " + lastDay.ToString("d"));

            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = firstDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)firstDay.DayOfWeek;
                int lastDayOfWeek = lastDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            foreach (DateTime bankHoliday in bankHolidays)
            {
                DateTime bh = bankHoliday.Date;
                if (firstDay <= bh && bh <= lastDay)
                    --businessDays;
            }

            return businessDays;
        }
        /// <summary>
        /// This function will take two serparate lists and convert them into a list of tuples
        /// each tuple will have the first/second element from the first/second list
        /// </summary>
        /// <param name="listA"></param>
        /// <param name="listB"></param>
        /// <returns>Returns a list of tuples</returns>
        public static List<Tuple<T1, T2>> TwoListsToListOfTuples<T1, T2>(List<T1> listA, List<T2> listB)

        {
            Tuple<T1, T2>[] arrayOfTuples = new Tuple<T1, T2>[listA.Count];
            //check if both lists are of same length
            //if yes do the merge into a list of tuples
            if (listA.Count == listB.Count)
            {
                for (int i = 0; i < listA.Count; i++)
                {
                    arrayOfTuples[i] = new Tuple<T1, T2>(listA[i], listB[i]);
                }
                return arrayOfTuples.ToList();
            }
            else
            {
                throw new Exception("The two provided lists do not have the same lenght");

            }
        }

        /// <summary>
        ///     This function will extract the elements at the same index from several lists
        ///     Made to work with a list of any types
        /// </summary>
        /// <param name="listOfLists">The container of lists to work on</param>
        /// <param name="indexNr">
        ///     The index at which the elements should be copied. Remember that index numbers start at 0 and not
        ///     1
        /// </param>
        /// <returns>Returns a list with extracted element from each separate list</returns>
        public static List<T> ExtractElementsAtIndex<T>(IList<List<T>> listOfLists, int indexNr)
        {
            var workArray = new T[listOfLists.Count];
            for (var i = 0; i < listOfLists.Count; i++)
                workArray[i] = listOfLists[i][indexNr];
            return workArray.ToList();
        }

        /// <summary>
        ///     This function will compute the average of all elements at the same index number among several lists
        /// </summary>
        /// <param name="listOfLists">The container of lists to work on</param>
        /// <param name="indexNr">
        ///     The index at which the elements should be averaged. Remember that index numbers start at 0 and
        ///     not 1
        /// </param>
        /// <returns>Returns the average as a double</returns>
        public static double AvgOfElementsAtSpecificIndex(IList<List<double>> listOfLists, int indexNr)
        {
            return ExtractElementsAtIndex(listOfLists, indexNr).Average();
        }

        /// <summary>
        ///     This function will compute the average for elements at the same index within several lists.
        ///     It will do so for each index at a time.
        /// </summary>
        /// <param name="listOfLists">The lists of lists to work on</param>
        /// <returns></returns>
        public static List<double> AvgOfElementsAtSameIndexes(IList<List<double>> listOfLists)
        {
            var avgOfElementsAtSameIndexes = new double[listOfLists[0].Count];
            for (var i = 0; i < listOfLists[0].Count; i++)
                avgOfElementsAtSameIndexes[i] = AvgOfElementsAtSpecificIndex(listOfLists, i);
            return avgOfElementsAtSameIndexes.ToList();
        }

        /// <summary>
        ///     The function should compute the percentile of elements at the same index for a 2D array of doubles
        /// </summary>
        /// <param name="listOfLists"></param>
        /// <param name="percentile"></param>
        /// <returns></returns>
        public static List<double> PercElementsAtTheSameIndex(IList<List<double>> listOfLists, double percentile)
        {
            var percElementsAtTheSameIndex = new double[listOfLists[0].Count];
            for (var i = 0; i < listOfLists[0].Count; i++)
            {
                var percPerRow = Percentile(ExtractElementsAtIndex(listOfLists, i), percentile);
                percElementsAtTheSameIndex[i] = percPerRow;
            }
            return percElementsAtTheSameIndex.ToList();
        }

        /// <summary>
        ///     This function should compute the avereage and 2 specified percentiles of elements at the same index of a 2D list
        ///     In other words, avg and percentiles for every row in a 2D array
        /// </summary>
        /// <param name="listOfLists"></param>
        /// <param name="firstPerc"></param>
        /// <param name="secondPerc"></param>
        /// <returns></returns>
        public static List<List<double>> AvgAndPercElementsAtSameIndex(IList<List<double>> listOfLists, double firstPerc,
            double secondPerc)
        {
            var avgElementsSameIndex = AvgOfElementsAtSameIndexes(listOfLists);
            var firstPercElSameIndex = PercElementsAtTheSameIndex(listOfLists, firstPerc);
            var secondPercElSameIndex = PercElementsAtTheSameIndex(listOfLists, secondPerc);
            //initialize the list to be returned with the required outputs
            var avgAndPercElSameIndex = new List<List<double>>
            {
                firstPercElSameIndex,
                avgElementsSameIndex,
                secondPercElSameIndex
            };
            return avgAndPercElSameIndex;
        }

        /// <summary>
        ///     This function  computes the specific percentile level of a series
        /// </summary>
        /// <param name="sequance"></param>
        /// <param name="percentile"></param>
        /// <returns>Retusn the first element under the required percentile level</returns>
        public static double Percentile(IEnumerable<double> sequance, double percentile)
        {
            var sequanceArr = sequance.ToArray();
            Array.Sort(sequanceArr);
            // ReSharper disable once InconsistentNaming
            var N = sequanceArr.Length;
            var n = (N - 1) * percentile + 1;
            // Another method: double n = (N + 1) * percentile;
            if (n == 1d)
                return sequanceArr[0];
            if (n == N)
            {
                return sequanceArr[N - 1];
            }
            var k = (int)n;
            var d = n - k;
            return sequanceArr[k - 1] + d * (sequanceArr[k] - sequanceArr[k - 1]);
        }

        /// <summary>
        ///     The function returns 3 statistics about a given series
        ///     The statistics returned are First Quartile(25%), Average and Third Quartile(75%)
        /// </summary>
        /// <param name="inputArray"></param>
        /// <param name="firstPercentile"></param>
        /// <param name="secondPercentile"></param>
        /// <returns></returns>
        public static List<double> AvgAndPerc(IList<double> inputArray, double firstPercentile, double secondPercentile)
        {
            //generate the output list and compute the elements within it
            var avgAndPercByRowsList = new List<double>
            {
                Percentile(inputArray, firstPercentile),
                inputArray.Average(),
                Percentile(inputArray, secondPercentile)
            };

            return avgAndPercByRowsList;
        }

        /// <summary>
        ///     The function returns the log returns for a time series
        /// </summary>
        /// <param name="timeSeries"></param>
        /// <returns></returns>
        public static List<double> LogRetunsOfSeries(IList<double> timeSeries)
        {
            var logReturnsSeries = new List<double>();
            for (var i = 0; i < timeSeries.Count - 1; i++)
                logReturnsSeries.Add(Math.Log(timeSeries[i + 1] / timeSeries[i]));
            return logReturnsSeries;
        }

        /// <summary>
        ///     The function applies exponential weighting to a given time series
        /// </summary>
        /// <param name="timeSeries">The time series</param>
        /// <param name="lambda">The exponential weighting factor. Preferably between 0.9 and 1</param>
        /// <returns></returns>
        public static List<double> EwmaSeries(IList<double> timeSeries, double lambda)
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            var ewmaSeries = new List<double>();
            //first value will just be the actual first value
            ewmaSeries.Add(timeSeries[0]);
            //Compute EWMA by using the last element in the time series and the last element in the EWMA series
            //The formula is (lambda*ewma_t-1)+(1-lambda)*timeseries_t
            for (var i = 1; i < timeSeries.Count; i++)
                ewmaSeries.Add(lambda * ewmaSeries[i - 1] + (1 - lambda) * timeSeries[i]);
            return ewmaSeries;
        }

        /// <summary>
        ///     The function returns the daily variance of a time series
        ///     as squared returns, by assuming that the average return is 0
        /// </summary>
        /// <param name="priceSeries"></param>
        /// <returns></returns>
        public static List<double> VarDailyReturns(IList<double> priceSeries)
        {
            var dailyLogRetuns = LogRetunsOfSeries(priceSeries);
            //double avgLogReturn = dailyLogRetuns.Average();
            double avgLogReturn = 0;
            var dailyVarianceOfReturns = new List<double>();

            for (var i = 0; i < dailyLogRetuns.Count; i++)
                dailyVarianceOfReturns.Add(Math.Pow(dailyLogRetuns[i] - avgLogReturn, 2));
            return dailyVarianceOfReturns;
        }

        /// <summary>
        ///     The function computes the daily STDEV for the returns of a time series.
        /// </summary>
        /// <param name="priceSeries"></param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public static List<double> DailyStdevOfTimeSeriesEwma(IList<double> priceSeries, double lambda)
        {
            var dailyVarSeries = VarDailyReturns(priceSeries);
            var ewmaVarianceSeries = EwmaSeries(dailyVarSeries, lambda);
            var ewmaStdevSeries = new List<double>();
            for (var i = 0; i < ewmaVarianceSeries.Count; i++)
                ewmaStdevSeries.Add(Math.Sqrt(ewmaVarianceSeries[i]));
            return ewmaStdevSeries;
        }

        /// <summary>
        ///     The function computes the daily covariance as squared returns between two time series
        ///     The average return is assumed to be 0, that
        /// </summary>
        /// <param name="timeSeries1"></param>
        /// <param name="timeSeries2"></param>
        /// <returns></returns>
        public static List<double> CovDailyReturns(IList<double> timeSeries1, IList<double> timeSeries2)
        {
            var dailyCovOfReturns = new List<double>();
            var logRetunsOfSeries1 = LogRetunsOfSeries(timeSeries1);
            var logRetunsOfSeries2 = LogRetunsOfSeries(timeSeries2);
            for (var i = 0; i < logRetunsOfSeries1.Count; i++)
                dailyCovOfReturns.Add(logRetunsOfSeries1[i] * logRetunsOfSeries2[i]);
            return dailyCovOfReturns;
        }

        /// <summary>
        ///     The function will compute the daily cross sectional covariance of multiple pairs of time series of prices.
        /// </summary>
        /// <param name="multiplePairsOfPrices">
        ///     A container of multiple pairs of time series of prices:
        ///     {([P1Series1],[P1Series2]...[])([P2Series1],[P2Series2]...[])}
        /// </param>
        /// <returns>Retuns a list of lists of daily covariances</returns>
        public static List<List<double>> DailyCovarOfMultipleReturns(IList<List<List<double>>> multiplePairsOfPrices)
        {
            var dailyCovarOfMultipleReturns = new List<List<double>>();
            //for each pair of prices
            for (var pairIndex = 0; pairIndex < multiplePairsOfPrices[0].Count; pairIndex++)
            {
                var covListForEachPair = CovDailyReturns(multiplePairsOfPrices[0][pairIndex],
                    multiplePairsOfPrices[1][pairIndex]);
                dailyCovarOfMultipleReturns.Add(covListForEachPair);
            }
            return dailyCovarOfMultipleReturns;
        }

        /// <summary>
        ///     The function will first compute the daily cross sectional covariance of multiple pairs of series of prices.
        ///     After that it will apply exponential weighting to the obtained time series of average covariances
        /// </summary>
        /// <param name="multiplePairsOfPrices">
        ///     A container of multiple pairs of time series of prices:
        ///     {([P1Series1],[P1Series2]...[])([P2Series1],[P2Series2]...[])}
        /// </param>
        /// <param name="lambda">The exponential weighting factor. Preferably between 0.9 and 1</param>
        /// <returns>Retuns a list of lists of daily covariances to which an exponential wighting has been applied</returns>
        public static List<List<double>> DailyCovarOfMultipleReturnsEWMA(IList<List<List<double>>> multiplePairsOfPrices,
            double lambda)
        {
            var dailyCovarOfMultipleReturnsEWMA = new List<List<double>>();

            var dailyCovarOfMultipleReturns = DailyCovarOfMultipleReturns(multiplePairsOfPrices);
            foreach (var dailyCovarList in dailyCovarOfMultipleReturns)
            {
                var dailyCovarListEwma = EwmaSeries(dailyCovarList, lambda);
                dailyCovarOfMultipleReturnsEWMA.Add(dailyCovarListEwma);
            }
            return dailyCovarOfMultipleReturnsEWMA;
        }

        /// <summary>
        ///     The function will first compute the daily cross sectional AVERAGE covariance of multiple pairs of series of prices.
        ///     After that it will apply exponential weighting to the obtained time series of average covariances
        /// </summary>
        /// <param name="multipleSeriesOfPrices">
        ///     A container of multiple pairs of time series of prices:
        ///     {([P1Series1],[P1Series2]...[])([P2Series1],[P2Series2]...[])}
        /// </param>
        /// <param name="lambda">The exponential weighting factor. Preferably between 0.9 and 1</param>
        /// <returns>Retuns a list of average daily covariances to which an exponential wighting has been applied</returns>
        public static List<double> AvgDailyCovarOfMultipleReturnsEWMA(IList<List<List<double>>> multipleSeriesOfPrices,
            double lambda)
        {
            var dailyCovarOfMultipleReturnsEWMA = DailyCovarOfMultipleReturnsEWMA(multipleSeriesOfPrices, lambda);

            return AvgOfElementsAtSameIndexes(dailyCovarOfMultipleReturnsEWMA);
        }

        /// <summary>
        ///     This function will compute the daily exponentially weighted correlation between the retuns for two time series
        ///     it will first compute the var/cov by using EWMA and then apply the correlation formula
        /// </summary>
        /// <param name="timeSeries1"></param>
        /// <param name="timeSeries2"></param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public static List<double> CorrDailyReturnsEwma(IList<double> timeSeries1, IList<double> timeSeries2, double lambda)
        {
            //compute the daily variance and covariance between the two time series
            var varDailyReturnsEwmaLst1 = EwmaSeries(VarDailyReturns(timeSeries1), lambda);
            var varDailyReturnsEwmaLst2 = EwmaSeries(VarDailyReturns(timeSeries2), lambda);
            var covDailyReturnsEwmaLst = EwmaSeries(CovDailyReturns(timeSeries1, timeSeries2), lambda);
            var corrDailyReturnsEwmaLst = new List<double>();
            //apply the correlation formula: corr = [cov(a,b)]/[var(a)*var(b)]
            for (var i = 0; i < covDailyReturnsEwmaLst.Count; i++)
                corrDailyReturnsEwmaLst.Add(
                    covDailyReturnsEwmaLst[i] / Math.Sqrt(varDailyReturnsEwmaLst1[i] * varDailyReturnsEwmaLst2[i]));
            return corrDailyReturnsEwmaLst;
        }

        /// <summary>
        ///     This function will compute multiple series of daily variances for multiple time series of prices
        /// </summary>
        /// <param name="multipleSeriesOfPrices">A list of multiple time series of prices</param>
        /// <returns>
        ///     Returns a list of lists of variances. Each list within the big list coresponds to a different time series of
        ///     prices
        /// </returns>
        public static List<List<double>> DailyVarOfMultipleReturns(IList<List<double>> multipleSeriesOfPrices)
        {
            var dailyVarMultipleSeries = new List<List<double>>();
            foreach (var pricesSeries in multipleSeriesOfPrices)
                dailyVarMultipleSeries.Add(VarDailyReturns(pricesSeries));
            return dailyVarMultipleSeries;
        }

        /// <summary>
        ///     This function will compute the average of variances of multiple time series of prices.
        ///     It will be a cross-sectional average at every time step.
        ///     The result will be an avegare of variance at every time step of multiple series of prices.
        /// </summary>
        /// <param name="multipleSeriesOfPrices">A container of multiple time series of prices</param>
        /// <returns>Returns a list of cross sectional varinace averages at every time step for multiple series of prices</returns>
        public static List<double> AvgDailyVarOfMultipleReturns(IList<List<double>> multipleSeriesOfPrices)
        {
            var dailyVariancesOfMultipleReturns = DailyVarOfMultipleReturns(multipleSeriesOfPrices);
            //count -1 as there are n-1 returns for n prices
            var avgDailyVariancesOfMultipleReturns = new double[multipleSeriesOfPrices[0].Count - 1];
            for (var i = 0; i < avgDailyVariancesOfMultipleReturns.Length; i++)
                avgDailyVariancesOfMultipleReturns[i] =
                    AvgOfElementsAtSpecificIndex(dailyVariancesOfMultipleReturns, i);
            return avgDailyVariancesOfMultipleReturns.ToList();
        }

        /// <summary>
        ///     The function will first compute the daily cross sectional variance of multiple time series of prices.
        ///     After that it will apply exponential weighting to the obtained time series of average variances
        /// </summary>
        /// <param name="multipleSeriesOfPrices">A container of multiple time series of prices</param>
        /// <param name="lambda">The exponential weighting factor. Preferably between 0.9 and 1</param>
        /// <returns>Retuns a list of daily variances to which an exponential wighting has been applied</returns>
        public static List<double> AvgDailyVarOfMultipleReturnsEwma(IList<List<double>> multipleSeriesOfPrices,
            double lambda)
        {
            return EwmaSeries(AvgDailyVarOfMultipleReturns(multipleSeriesOfPrices), lambda);
        }

        /// <summary>
        ///     This function should compute the average correlation among several pairs of time series of prices
        ///     The correlation will be also exponentially weighted
        /// </summary>
        /// <param name="multiplePairsOfPrices">
        ///     The pairs of prices are devided in two different lists
        ///     In breif {[(Pair1Product1), (Pair2Product1), (...) ],[(Pair1Product2), (Pair2Product2), (...)]}
        /// </param>
        /// <param name="lambda">The exponential weigting factor. Preferably between 0.9 and 1</param>
        /// <returns>Returns a list of average daily correlations for all pairs of prices at each point in time</returns>
        public static List<double> AvgDailyCorrOfMultipleReturnsEwma(IList<List<List<double>>> multiplePairsOfPrices,
            double lambda)
        {
            var varListProduct1 = AvgDailyVarOfMultipleReturnsEwma(multiplePairsOfPrices[0], lambda);
            var varListProduct2 = AvgDailyVarOfMultipleReturnsEwma(multiplePairsOfPrices[1], lambda);
            var covList = AvgDailyCovarOfMultipleReturnsEWMA(multiplePairsOfPrices, lambda);

            var avgDailyCorrOfMultipleReturnsEwma = new double[varListProduct1.Count];
            //compute thea avg daily correlation for every time step
            for (var i = 0; i < varListProduct1.Count; i++)
            {
                var correl = covList[i] / Math.Sqrt(varListProduct1[i] * varListProduct2[i]);
                avgDailyCorrOfMultipleReturnsEwma[i] = correl;
            }
            //average the correlation cross sectionally at the same index nr
            return avgDailyCorrOfMultipleReturnsEwma.ToList();
        }
        public static List<double> NormalizeListAgainstMedian(IList<double> listOfDoubles)
        {
            double median = Percentile(listOfDoubles, 0.5);
            List<double> normalizedList = listOfDoubles.Select(element => element - median).ToList();
            return normalizedList;
        }
        /// <summary>
        /// A function that calculates the number of business date within a period with a start and end date
        /// Including the first and the last dates!
        /// </summary>
        /// <param name="firstDay"></param>
        /// <param name="lastDay"></param>
        /// <returns></returns>
        public static int BusinessDaysUntil(this DateTime firstDay, DateTime lastDay)
        //, params DateTime[] bankHolidays
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;

            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = (int)firstDay.DayOfWeek;
                int lastDayOfWeek = (int)lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            //foreach (DateTime bankHoliday in bankHolidays)
            //{
            //    DateTime bh = bankHoliday.Date;
            //    if (firstDay <= bh && bh <= lastDay)
            //        --businessDays;
            //}

            return businessDays;
        }
        public static double CDF(double z)
        {
            double p = 0.3275911;
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;

            int sign;
            if (z < 0.0)
                sign = -1;
            else
                sign = 1;

            double x = Math.Abs(z) / Math.Sqrt(2.0);
            double t = 1.0 / (1.0 + p * x);
            double erf = 1.0 - (((((a5 * t + a4) * t) + a3)
                                 * t + a2) * t + a1) * t * Math.Exp(-x * x);
            return 0.5 * (1.0 + sign * erf);
        }
        public static double ConvertToContinuous(double rate)
        {
            return Math.Log(1 + rate);
        }



        //public Type GetInstrumentType(object financialInstrument)
        //{
        //    Type result = null;
        //    var objectType = financialInstrument.GetType().FullName;
        //    if (objectType == typeof(Option).FullName)
        //    {
        //        result = (Type)Activator.CreateInstance(objectType.GetType());
        //    }
        //    else if (objectType == typeof(Future).FullName || objectType == typeof(Swap).FullName)
        //    {
        //        result = (Type)Activator.CreateInstance(objectType.GetType());
        //    }
        //    else
        //    {
        //        Log("Please, provide a valid argument! " + financialInstrument);
        //        throw new Exception("Please, provide a valid argument! " + financialInstrument);
        //    }
        //    return result;
        //}
    }
}
