using System;
using ValueAtRisk.Interfaces;

namespace ValueAtRisk.Models.Instruments
{
    public class Swap:IInstrument
    {
        public string Commodity { get; set; }
        public string FondsCode { get; set; }
        public DateTime EndPricingPeriod { get; set; }
        public DateTime StartPricingPeriod { get; set; }
        public Swap(string commodity, string fondsCode, DateTime startPricingPeriod, DateTime endPricingPeriod)
        {
            Commodity = commodity;
            FondsCode = fondsCode;
            StartPricingPeriod = startPricingPeriod;
            EndPricingPeriod = endPricingPeriod;
        }
    }
}
