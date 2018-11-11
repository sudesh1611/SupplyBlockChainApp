using System;
using System.Collections.Generic;
using System.Text;

namespace SupplyBlockChainApp
{
    public class Transaction
    {
        //Unique hashID of a product
        public string ProductID { get; set; }

        //What process is done on the product
        public string ProcessDone { get; set; }

        //Who did this process
        public string ProcessDoneBy { get; set; }

        //Cost of processing 
        public double CostOfProcess { get; set; }

        //Time and date when this transaction created
        public string TransactionTimeStamp { get; set; }

        //Location details of product at the time of transaction
        public string Latitude { get; set; }
        public string Longitude { get; set; }


        public Transaction(string productID, string processDone, string processDoneBy, string latitude, string longitude, double costOfProcess = 0)
        {
            if (string.IsNullOrEmpty(productID) || string.IsNullOrEmpty(processDone) || string.IsNullOrEmpty(processDoneBy))
            {
                //Console.WriteLine("One or more fields of transaction are empty. Transaction Aborted!!");
                return;
            }
            if (CostOfProcess < 0)
            {
                //Console.WriteLine("Cost of process can't be negative. Transaction Aborted!!");
                return;
            }
            ProductID = productID;
            ProcessDone = processDone;
            ProcessDoneBy = processDoneBy;
            CostOfProcess = costOfProcess;
            Latitude = latitude;
            Longitude = longitude;
            var now = DateTime.Now;
            TransactionTimeStamp = now.ToLongDateString() + " " + now.ToLongTimeString();
            return;
        }


        public override string ToString()
        {
            return (ProductID.Trim() + ProcessDone.Trim() + ProcessDoneBy.Trim() + CostOfProcess.ToString().Trim() + Latitude.ToString() + Longitude.ToString() + TransactionTimeStamp.Trim());
        }
    }
}
