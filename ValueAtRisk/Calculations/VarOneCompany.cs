using System;
using System.Collections.Generic;
using System.Text;
using ValueAtRisk.Models;

namespace ValueAtRisk.Calculations
{
    class VarOneCompany
    {
        //Have an sql query here to fill a list of Transactions for whcih VAR must be computed for
        internal List<Transaction> TransactionsForVar = new List<Transaction>();
        //This list will contain the mtm distribution of all individual transactions
        internal List<List<double>> CollectedMtmDistributionList = new List<List<double>>();
    }
}
