using System;
using System.Collections.Generic;
using System.Text;

namespace ValueAtRisk.Models
{
    public class Option:Transaction
    {
        public OptionStyle ChosenOptionStyle;
        public CallPutType ChosenCallPutType;
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
        public double StrikePrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionStyle"></param>
        /// <param name="callPutType"></param>
        /// <param name="strikePrice"></param>
        public Option(OptionStyle optionStyle, CallPutType callPutType, double strikePrice)
        {
            ChosenOptionStyle = optionStyle;
            ChosenCallPutType = callPutType;
            StrikePrice = strikePrice;
        }
    }
}
