using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SupplyBlockChainApp
{
    public class BlockChain
    {
        private int Difficulty { get; set; } = 5;
        public List<Block> Chain { get; set; }
        public List<Transaction> PendingTransactions { get; set; }

        public BlockChain()
        {
            Chain = new List<Block>
            {
                new Block("",new List<Transaction>(),Difficulty)
            };
            PendingTransactions = new List<Transaction>();
        }

        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        public async Task MineTransactions()
        {
            Block newBlock = new Block(GetLatestBlock().CurrentHash, PendingTransactions, Difficulty);
            await newBlock.MineBlock(Difficulty);
            Chain.Add(newBlock);
            PendingTransactions = new List<Transaction>();
        }

        public void CreateTransaction(Transaction transaction)
        {
            PendingTransactions.Add(transaction);
        }

        public bool IsChainValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                var currentBlock = Chain[i];
                var previousBlock = Chain[i - 1];
                if(currentBlock.CurrentHash != CalculteHashOfBlock(currentBlock))
                {
                    return false;
                }
                if(currentBlock.PreviousHash != previousBlock.CurrentHash)
                {
                    return false;
                }
            }
            return true;
        }

        //public void GetProductTransactionsDetails(string productID)
        //{
        //    Console.WriteLine();
        //    Console.WriteLine("Transactions on Product ID : " + productID);

        //    double processingCost = 0;

        //    foreach (var block in this.Chain)
        //    {
        //        foreach (var transaction in block.Transactions)
        //        {
        //            if (transaction.ProductID == productID)
        //            {
        //                Console.WriteLine("\t Transaction Time : " + transaction.TransactionTimeStamp);
        //                Console.WriteLine("\t Work Done : " + transaction.ProcessDone);
        //                Console.WriteLine("\t Done By: " + transaction.ProcessDoneBy);
        //                Console.WriteLine("\t Processing Cost: " + transaction.CostOfProcess);
        //                Console.WriteLine();
        //                processingCost += transaction.CostOfProcess;
        //            }
        //        }
        //    }

        //    Console.WriteLine("\tTotal Processing Cost Till Now : " + processingCost);
        //    Console.WriteLine();
        //    Console.WriteLine();
        //}

        private string CalculteHashOfBlock(Block block)
        {
            var transactionString = String.Empty;
            foreach(var item in block.Transactions)
            {
                transactionString += item.ToString();
            }
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(block.PreviousHash + transactionString + block.Nounce.ToString() + block.BlockAddedTimeStamp.Trim());
            byte[] hash = sha256.ComputeHash(bytes);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

    }
}
