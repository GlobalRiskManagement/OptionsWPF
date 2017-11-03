using System;
using System.Collections.Generic;
using System.Text;

namespace ValueAtRisk.Models
{
   public class MtmDistribOneTransaction
   {
       private List<MtmValueOneTransaction> _mtmList;
       public List<MtmValueOneTransaction> MtmDistribList { get { return _mtmList; } set { _mtmList = value; } }

        public MtmDistribOneTransaction(List<MtmValueOneTransaction> mtmList)
       {
           MtmDistribList = mtmList;
       }
   }
}
