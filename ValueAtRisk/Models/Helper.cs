using System;
using System.Collections.Generic;
using System.Text;
using Utilities;
using ValueAtRisk.Models.Inputs;
using ValueAtRisk.Models.Instruments;

namespace ValueAtRisk.Models
{
   public class Helper:Logger
    {
        static Dictionary<Type, int> typeDict = new Dictionary<Type, int>
        {
            {typeof(Future),0},
            {typeof(Swap),1},
            {typeof(Option),2},
            {typeof(AsianOptionInput),3},
            {typeof(EuropeanOptionInput),4},
            {typeof(FutureInput),5},
            {typeof(SwapInput),6},
        };
        public static Object GetInstrumentType(object financialInstrument)
        {
            var objectType = financialInstrument.GetType();
            Logger log=new Logger();
            switch (typeDict[objectType])
            {
                case 0:
                    var test = (Future)Activator.CreateInstance(objectType);
                    return test;
                case 1:
                    return (Swap)Activator.CreateInstance(objectType); 
                case 2:
                    return (Option)Activator.CreateInstance(objectType);
                case 3:
                    return (AsianOptionInput)Activator.CreateInstance(objectType);
                case 4:
                    return (EuropeanOptionInput)Activator.CreateInstance(objectType);
                case 5:
                    return (FutureInput)Activator.CreateInstance(objectType);
                case 6:
                    return (SwapInput)Activator.CreateInstance(objectType);
                default:
                    log.Log("Please, provide a valid argument! " + financialInstrument);
                    throw new Exception("Please, provide a valid argument! " + financialInstrument);
            }
        }
    }
}
