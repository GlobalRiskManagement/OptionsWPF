using System;
using System.Collections.Generic;
using System.Text;
using Accord.Statistics.Distributions.Univariate;
using ValueAtRisk.Models;

namespace ValueAtRisk.Calculations
{
    public class OptionCalculator
    {
        /// <summary>
        /// calculate Asian BS formula
        /// </summary>
        /// <param name="option"></param>
        /// <param name="optionInput"></param>
        /// <returns></returns>
        /// 
        public double DiscreteAsianHHM(Option option, OptionInputAsian optionInput)
        {
            double d1, d2, h, EA, EA2, vA, OptionValue;
            double result = 0;
            var normal = new NormalDistribution();

            if (option.ChosenOptionStyle == Option.OptionStyle.Asian)
            {
                h = (optionInput.TtoMaturity - optionInput.TtoNextAverage) / (optionInput.NoOfFixings - 1);
                if (optionInput.CostOfCarry == 0)
                    EA = optionInput.AssetPrice;
                else
                    EA = optionInput.AssetPrice / optionInput.NoOfFixings *
                         Math.Exp(optionInput.CostOfCarry * optionInput.TtoNextAverage) *
                         (1 - Math.Exp(optionInput.CostOfCarry * h * optionInput.NoOfFixings)) /
                         (1 - Math.Exp(optionInput.CostOfCarry * h));

                if (optionInput.NoOfFixingsFixed > 0)
                {
                    if (optionInput.AverageSoFar > optionInput.NoOfFixings / optionInput.NoOfFixingsFixed *
                        option.StrikePrice)
                    {
                        if (option.ChosenCallPutType == Option.CallPutType.Put)
                            return result = 0;
                        if (option.ChosenCallPutType == Option.CallPutType.Call)
                        {
                            optionInput.AverageSoFar =
                                optionInput.AverageSoFar * optionInput.NoOfFixingsFixed /
                                optionInput.NoOfFixings +
                                EA * (optionInput.NoOfFixings - optionInput.NoOfFixingsFixed) /
                                optionInput.NoOfFixings;
                            return result = (optionInput.AverageSoFar - option.StrikePrice) *
                                            (Math.Exp(-optionInput.RiskFreeRate * optionInput.TtoMaturity));
                        }
                    }
                }
                if (optionInput.NoOfFixingsFixed == optionInput.NoOfFixings - 1)
                {
                    option.StrikePrice = optionInput.NoOfFixings * option.StrikePrice -
                                         (optionInput.NoOfFixings - 1 * optionInput.AverageSoFar);
                    return result = DiscreteEuropeanHHM(option, optionInput) * 1 / optionInput.NoOfFixings;
                }
                if (optionInput.CostOfCarry == 0)
                {
                    EA2 = optionInput.AssetPrice * optionInput.AssetPrice *
                          Math.Exp(optionInput.Volatility * optionInput.Volatility *
                                   optionInput.TtoNextAverage) /
                          (optionInput.NoOfFixings * optionInput.NoOfFixings)
                          * ((1 - Math.Exp(optionInput.Volatility * optionInput.Volatility * h *
                                           optionInput.NoOfFixings)) /
                             (1 - Math.Exp(optionInput.Volatility * optionInput.Volatility * h))
                             + 2 / (1 - Math.Exp(optionInput.Volatility * optionInput.Volatility * h)) *
                             (optionInput.NoOfFixings -
                              (1 - Math.Exp(optionInput.Volatility * optionInput.Volatility * h *
                                            optionInput.NoOfFixings)) /
                              (1 - Math.Exp(optionInput.Volatility * optionInput.Volatility * h))));
                }
                else
                {
                    EA2 = optionInput.AssetPrice * optionInput.AssetPrice *
                          Math.Exp((2 * optionInput.CostOfCarry +
                                    optionInput.Volatility * optionInput.Volatility) *
                                   optionInput.TtoNextAverage) /
                          (optionInput.NoOfFixings * optionInput.NoOfFixings)
                          * ((1 - Math.Exp(
                                  (2 * optionInput.CostOfCarry +
                                   optionInput.Volatility * optionInput.Volatility) * h *
                                  optionInput.NoOfFixings)) /
                             (1 - Math.Exp((2 * optionInput.CostOfCarry +
                                            optionInput.Volatility * optionInput.Volatility) * h))
                             + 2 / (1 - Math.Exp((optionInput.CostOfCarry +
                                                  optionInput.Volatility * optionInput.Volatility) * h)) *
                             ((1 - Math.Exp(optionInput.CostOfCarry * h * optionInput.NoOfFixings)) /
                              (1 - Math.Exp(optionInput.CostOfCarry * h))
                              - (1 - Math.Exp(
                                     (2 * optionInput.CostOfCarry +
                                      optionInput.Volatility * optionInput.Volatility) * h *
                                     optionInput.NoOfFixings))
                              / (1 - Math.Exp((2 * optionInput.CostOfCarry +
                                               optionInput.Volatility * optionInput.Volatility) * h))));
                }
                vA = Math.Sqrt((Math.Log(EA2) - 2 * Math.Log(EA)) / optionInput.TtoMaturity);
                OptionValue = 0;
                if (optionInput.NoOfFixingsFixed > 0)
                {
                    option.StrikePrice =
                        optionInput.NoOfFixings / (optionInput.NoOfFixings - optionInput.NoOfFixingsFixed) *
                        option.StrikePrice - optionInput.NoOfFixingsFixed /
                        (optionInput.NoOfFixings - optionInput.NoOfFixingsFixed) * optionInput.AverageSoFar;
                }
                d1 = (Math.Log(EA / option.StrikePrice) + Math.Pow(vA, 2) / 2 * optionInput.TtoMaturity) /
                     (vA * Math.Sqrt(optionInput.TtoMaturity));
                d2 = d1 - vA * Math.Sqrt(optionInput.TtoMaturity);

                if (option.ChosenCallPutType == Option.CallPutType.Call)
                {
                    OptionValue = Math.Exp(-optionInput.RiskFreeRate * optionInput.TtoMaturity) *
                                  (EA * normal.DistributionFunction(d1) -
                                   option.StrikePrice * normal.DistributionFunction(d2));
                }
                if (option.ChosenCallPutType == Option.CallPutType.Put)
                {
                    OptionValue = Math.Exp(-optionInput.RiskFreeRate * optionInput.TtoMaturity) *
                                  (option.StrikePrice * normal.DistributionFunction(-d2) -
                                   EA * normal.DistributionFunction(-d1));
                }
                result = OptionValue * (optionInput.NoOfFixings - optionInput.NoOfFixingsFixed) /
                         optionInput.NoOfFixings;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="optionInput"></param>
        /// <returns></returns>
        /// 
        public double AsianCurranApprox(Option option, OptionInputAsian optionInput)
        {
            double dt, my, myi, vxi, vi, vx, Km, sum1, sum2, ti, EA, z;
            double result = 0;
            var normal = new NormalDistribution();

            z = 1;

            if (option.ChosenCallPutType == Option.CallPutType.Put)
                z = -1;
            dt = (optionInput.TtoMaturity - optionInput.TtoNextAverage) / (optionInput.NoOfFixings - 1);
            if (optionInput.CostOfCarry == 0)
                EA = optionInput.AssetPrice;
            else
            {
                EA = optionInput.AssetPrice / optionInput.NoOfFixings *
                     Math.Exp(optionInput.CostOfCarry * optionInput.TtoNextAverage) *
                     (1 - Math.Exp(optionInput.CostOfCarry * dt * optionInput.NoOfFixings)) /
                     (1 - Math.Exp(optionInput.CostOfCarry * dt));
            }
            if (optionInput.NoOfFixingsFixed > 0)
            {
                if (optionInput.AverageSoFar >
                    optionInput.NoOfFixings / optionInput.NoOfFixingsFixed * option.StrikePrice)
                {
                    if (option.ChosenCallPutType == Option.CallPutType.Put)
                    {
                        return 0;
                    }
                    if (option.ChosenCallPutType == Option.CallPutType.Call)
                    {
                        optionInput.AverageSoFar =
                            optionInput.AverageSoFar * optionInput.NoOfFixingsFixed /
                            optionInput.NoOfFixings +
                            EA * (optionInput.NoOfFixings - optionInput.NoOfFixingsFixed) /
                            optionInput.NoOfFixings;
                        result = (optionInput.AverageSoFar - option.StrikePrice) *
                                 Math.Exp(-optionInput.RiskFreeRate * optionInput.TtoMaturity);
                        return result;
                    }
                }
            }
            if (optionInput.NoOfFixingsFixed == optionInput.NoOfFixings - 1)
            {
                option.StrikePrice = optionInput.NoOfFixings * option.StrikePrice -
                                     (optionInput.NoOfFixings - 1) * optionInput.AverageSoFar;
                result = DiscreteEuropeanHHM(option, optionInput) * 1 / optionInput.NoOfFixings;
                return result;
            }
            if (optionInput.NoOfFixingsFixed > 0)
            {
                option.StrikePrice =
                    optionInput.NoOfFixings / (optionInput.NoOfFixings - optionInput.NoOfFixingsFixed) *
                    option.StrikePrice - optionInput.NoOfFixingsFixed /
                    (optionInput.NoOfFixings - optionInput.NoOfFixingsFixed) * optionInput.AverageSoFar;
            }

            vx = optionInput.Volatility * Math.Sqrt(optionInput.TtoNextAverage +
                                                        dt * (optionInput.NoOfFixings - 1) *
                                                        (2 * optionInput.NoOfFixings - 1) /
                                                        (6 * optionInput.NoOfFixings));
            my = Math.Log(optionInput.AssetPrice) +
                 (optionInput.CostOfCarry - optionInput.Volatility * optionInput.Volatility * 0.5) *
                 (optionInput.TtoNextAverage + (optionInput.NoOfFixings - 1) * dt / 2);
            sum1 = 0;

            for (int i = 1; i < optionInput.NoOfFixings + 1; i++)
            {
                ti = dt * i + optionInput.TtoNextAverage - dt;
                vi = optionInput.Volatility * Math.Sqrt(optionInput.TtoNextAverage + (i - 1) * dt);
                vxi = optionInput.Volatility * optionInput.Volatility *
                      (optionInput.TtoNextAverage +
                       dt * ((i - 1) - i * (i - 1) / (2 * optionInput.NoOfFixings)));
                myi = Math.Log(optionInput.AssetPrice) +
                      (optionInput.CostOfCarry - optionInput.Volatility * optionInput.Volatility * 0.5) *
                      ti;
                sum1 = sum1 + Math.Exp(myi + vxi / (vx * vx) *
                                       (Math.Log(option.StrikePrice) - my) + (vi * vi - vxi * vxi / (vx * vx)) * 0.5);
            }
            Km = 2 * option.StrikePrice - 1 / optionInput.NoOfFixings * sum1;
            sum2 = 0;

            for (int i = 1; i < optionInput.NoOfFixings + 1; i++)
            {
                ti = dt * i + optionInput.TtoNextAverage - dt;
                vi = optionInput.Volatility * Math.Sqrt(optionInput.TtoNextAverage + (i - 1) * dt);
                vxi = optionInput.Volatility * optionInput.Volatility *
                      (optionInput.TtoNextAverage +
                       dt * ((i - 1) - i * (i - 1) / (2 * optionInput.NoOfFixings)));
                myi = Math.Log(optionInput.AssetPrice) +
                      (optionInput.CostOfCarry - optionInput.Volatility * optionInput.Volatility * 0.5) *
                      ti;
                sum2 = sum2 + Math.Exp(myi + vi * vi * 0.5) *
                       normal.DistributionFunction(z * ((my - Math.Log(Km)) / vx + vxi / vx));
            }
            result = Math.Exp(-optionInput.RiskFreeRate * optionInput.TtoMaturity) * z *
                     (1 / optionInput.NoOfFixings * sum2 -
                      option.StrikePrice * normal.DistributionFunction(z * (my - Math.Log(Km)) / vx)) *
                     (optionInput.NoOfFixings - optionInput.NoOfFixingsFixed) / optionInput.NoOfFixings;
            return result;
        }

        /// <summary>
        /// Computes European style option by Black Scholes and Merton
        /// </summary>
        /// <param name="option"></param>
        /// <param name="optionInput"></param>
        /// <returns></returns>
        /// 
        public double DiscreteEuropeanHHM(Option option, OptionInput optionInput)
        {
            double d1, d2, result = 0;
            var normal = new NormalDistribution();
            d1 = (Math.Log(optionInput.AssetPrice / option.StrikePrice) +
                  (optionInput.CostOfCarry + Math.Pow(optionInput.Volatility, 2) / 2) *
                  optionInput.TtoMaturity) / (optionInput.Volatility * Math.Sqrt(optionInput.TtoMaturity));
            d2 = d1 - optionInput.Volatility * Math.Sqrt(optionInput.TtoMaturity);
            if (option.ChosenCallPutType == Option.CallPutType.Call)
            {
                result = optionInput.AssetPrice *
                         Math.Exp((optionInput.CostOfCarry - optionInput.RiskFreeRate) *
                                  optionInput.TtoMaturity) * normal.DistributionFunction(d1) - option.StrikePrice *
                         Math.Exp(-optionInput.RiskFreeRate * optionInput.TtoMaturity) *
                         normal.DistributionFunction(d2);
                return result;
            }
            else
            {
                result =
                    option.StrikePrice * Math.Exp(-optionInput.RiskFreeRate * optionInput.TtoMaturity) *
                    normal.DistributionFunction(-d2) - optionInput.AssetPrice *
                    Math.Exp((optionInput.CostOfCarry - optionInput.RiskFreeRate) *
                             optionInput.TtoMaturity) * normal.DistributionFunction(-d1);
                return result;
            }
        }
    }

}
