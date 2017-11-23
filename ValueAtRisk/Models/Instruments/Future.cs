using System;
using ValueAtRisk.Interfaces;

namespace ValueAtRisk.Models.Instruments
{
    public class Future:IInstrument
    {
        public string Commodity { get; set; }
        public string FondsCode { get; set; }
        public DateTime EndPricingPeriod { get; set; }
        public Future(string commodity, string fondsCode, DateTime endPricingPeriod)
        {
            Commodity = commodity;
            FondsCode = fondsCode;
            EndPricingPeriod = endPricingPeriod;
        }
    }
}
