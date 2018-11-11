using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SupplyBlockChainApp
{
    public class BlockChain
    {
        //Main Chain containing all the mined blocks
        public List<Block> Chain { get; set; }

        //This is how many zeroes should be in front of hash
        public int Difficulty { get; set; } = 5;

        //Transactions which are to be mined in next round
        public List<Transaction> PendingTransactions { get; set; }

        //Transactions which are being mined in this round
        public List<Transaction> MiningTransactions { get; set; }

        public BlockChain()
        {
            Chain = new List<Block>
            {
                //Genesis Block
                new Block("",new List<Transaction>(),Difficulty)
            };
            PendingTransactions = new List<Transaction>();
            MiningTransactions = new List<Transaction>();
        }

        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        //Adds mined block to chain
        public void AddBlock(Block MinedBlock)
        {
            Chain.Add(MinedBlock);
            MiningTransactions = new List<Transaction>();
        }

        //Creates a new transaction
        public void CreateTransaction(Transaction transaction)
        {
            PendingTransactions.Add(transaction);
        }

        //Checks if blockchain is valid or not
        public bool IsChainValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                var currentBlock = Chain[i];
                var previousBlock = Chain[i - 1];
                if (currentBlock.CurrentHash != CalculteHashOfBlock(currentBlock))
                {
                    return false;
                }
                if (currentBlock.PreviousHash != previousBlock.CurrentHash)
                {
                    return false;
                }
            }
            return true;
        }

        //Calculates hash of a block
        private string CalculteHashOfBlock(Block block)
        {
            var transactionString = String.Empty;
            foreach (var item in block.Transactions)
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
