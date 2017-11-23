using System;
using ValueAtRisk.Interfaces;

namespace ValueAtRisk.Models.Instruments
{
    public class Option:IInstrument
    {
        public OptionStyle ChosenOptionStyle;
        public CallPutType ChosenCallPutType;
        public double StrikePrice { get; set; }
        public string Commodity { get; set; }
        public string FondsCode { get; set; }
        public DateTime EndPricingPeriod { get; set; }
        public enum OptionStyle
        {
            Asian,
            European
        }

        public enum CallPutType
        {
            Call,
            Put
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionStyle"></param>
        /// <param name="callPutType"></param>
        /// <param name="strikePrice"></param>
        public Option(OptionStyle optionStyle, CallPutType callPutType, double strikePrice, string commodity,string fondsCode, DateTime endPricingPeriod)
        {
            ChosenOptionStyle = optionStyle;
            ChosenCallPutType = callPutType;
            StrikePrice = strikePrice;
            Commodity = commodity;
            FondsCode = fondsCode;
            EndPricingPeriod = endPricingPeriod;
        }


    }
}
