using System;
using System.Collections.Generic;
using System.Text;

namespace ValueAtRisk.Interfaces
{
    interface IInstrument
    {
        string Commodity { get; set; }
        string FondsCode { get; set; }
        DateTime EndPricingPeriod { get; set; }
    }
}
