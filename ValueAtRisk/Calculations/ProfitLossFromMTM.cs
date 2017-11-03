using System;
using System.Collections.Generic;
using System.Text;

namespace ValueAtRisk.Calculations
{
    public class ProfitLossFromMTM
    {
        public double ProfitLoss { get; set; }
        //TODO previousmtm, currentmtm, buyer/seller
        public ProfitLossFromMTM(double previousMTM, double currentMTM, string buyerSeller="")
        {
            ProfitLoss = currentMTM - previousMTM;
        }
    }
}
