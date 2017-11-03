using System;
using System.Collections.Generic;
using System.Text;

namespace ValueAtRisk.Models
{
   public class MtmValueOneTransaction
   {
        private Tuple<DateTime, double> _mtmOneTrans;
        public Tuple<DateTime, double> MtmOneTrans  { get { return _mtmOneTrans; } set { _mtmOneTrans = value; } }
        public MtmValueOneTransaction(Tuple<DateTime, double> mtmOneTransaction)
        {
            MtmOneTrans = mtmOneTransaction;
        }
   }
}
