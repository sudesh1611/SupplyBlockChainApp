using System;
using System.Collections.Generic;
using System.Text;

namespace SupplyBlockChainApp
{
    public class Transaction
    {
        public string ProductID { get; set; }
        public string ProcessDone { get; set; }
        public string ProcessDoneBy { get; set; }
        public double CostOfProcess { get; set; }
        public string TransactionTimeStamp { get; set; }

        public Transaction(string productID, string processDone, string processDoneBy, double costOfProcess = 0)
        {
            if (string.IsNullOrEmpty(productID) || string.IsNullOrEmpty(processDone) || string.IsNullOrEmpty(processDoneBy))
            {
                //console.log()
                //Console.WriteLine("One or more fields of transaction are empty. Transaction Aborted!!");
                return;
            }
            if (CostOfProcess < 0)
            {
                //console.log()
                //Console.WriteLine("Cost of process can't be negative. Transaction Aborted!!");
                return;
            }
            ProductID = productID;
            ProcessDone = processDone;
            ProcessDoneBy = processDoneBy;
            CostOfProcess = costOfProcess;
            TransactionTimeStamp = DateTime.UtcNow.ToLongTimeString();
            return;
        }


        public override string ToString()
        {
            return (ProductID.Trim() + ProcessDone.Trim() + ProcessDoneBy.Trim() + CostOfProcess.ToString().Trim() + TransactionTimeStamp.Trim());
        }
    }
}
