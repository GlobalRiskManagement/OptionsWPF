using System;
using System.Collections.Generic;
using System.Text;
using ValueAtRisk.Models;
using ValueAtRisk.Models.Inputs;
using ValueAtRisk.Models.Instruments;

namespace ValueAtRisk.Calculations.MTM
{
    public class MTMOneTradeCalculator
    {
            OptionCalculator oc = new OptionCalculator();
            LinearPayoffCalc lpc = new LinearPayoffCalc();
            /// <summary>
            /// any kind of transaction(swaps, options...)
            /// </summary>
            /// <param name="transaction"></param>
            /// <param name="simulatedMarketPrice"></param>
            /// <param name="fromWhosPerspective"></param>
            public MarkToMarketLevel MTMCalculateOneTradeOnePrice(Transaction transaction, Price simulatedMarketPrice, object input)
            {
                MarkToMarketLevel mtm = null;
                var inputType = input.GetType().FullName;
                var transactionType = Helper.GetInstrumentType(transaction.FinancialInstrument);
                if (transactionType is Option)
                {

                }

                return mtm;
            }
        //public List<MarkToMarketLevel> MtmCalculateOneTradeListOfPrices(object transaction, List<Price> simulatedPrices,
        //    string fromWhosePerspective, OptionInput optionInput = null, OptionInputAsian optionInputAsian = null)
        //{
        //    List<MarkToMarketLevel> listOfMTMs = new List<MarkToMarketLevel>();
        //    foreach (var price in simulatedPrices)
        //    {
        //        var objectType = transaction.GetType().FullName;
        //        if (objectType == typeof(Option).FullName)
        //        {
        //            var instance = (Option)Activator.CreateInstance(objectType.GetType());
        //            if (instance.ChosenOptionStyle == Option.OptionStyle.Asian)
        //            {
        //                var result = MTMCalculateOneTradeOnePrice(transaction, price, fromWhosePerspective, optionInputAsian);
        //                listOfMTMs.Add(result);
        //            }
        //            else
        //            {
        //                var result = MTMCalculateOneTradeOnePrice(transaction, price, fromWhosePerspective, optionInput);
        //                listOfMTMs.Add(result);
        //            }
        //        }
        //        else
        //        {
        //            var result = MTMCalculateOneTradeOnePrice(transaction, price, fromWhosePerspective);
        //            listOfMTMs.Add(result);
        //        }
        //    }
        //    return listOfMTMs;
        //}
        //public Type GetOptionInputType(object optionInput)
        //{
        //    Type result = null;
        //    var objectType = optionInput.GetType().FullName;
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
        //        Log("Please, provide a valid argument! " + optionInput);
        //        throw new Exception("Please, provide a valid argument! " + optionInput);
        //    }
        //    return result;
        //}
    }
}
