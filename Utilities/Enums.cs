using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public class Enums
    {
        //The type of derivative: swap, future, option, fpa
        public enum DerivativeTypeEnum
        {
            Future,
            Swap,
            BulletSwap,
            NominationSwap,
            Fpa,
            Option
        }

        public enum AsianFormulaType
        {
            DiscreteAsianHHM,
            AsianCurranApprox
        }
    }
}
