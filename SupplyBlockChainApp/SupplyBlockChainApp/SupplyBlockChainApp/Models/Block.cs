using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SupplyBlockChainApp
{
    public class Block
    {
        //Stores the current hash of the block after mining
        public string CurrentHash { get; set; }

        //Stores the hash of the previous block
        public string PreviousHash { get; set; }

        //This is to get proof of work
        public ulong Nounce { get; set; }

        //This Contains all the transactions that are added to this block
        public List<Transaction> Transactions { get; set; }

        //Time and date when this block is created
        public string BlockAddedTimeStamp { get; set; }


        public Block(string previousHash, List<Transaction> transactions, int Difficulty)
        {
            CurrentHash = String.Empty;
            PreviousHash = previousHash;
            Transactions = transactions;
            var now = DateTime.Now;
            BlockAddedTimeStamp = now.ToLongDateString() + " " + now.ToLongTimeString();
            Nounce = 0;
        }

        //Mining of block
        public async Task MineBlock(int Difficulty)
        {
            var tempString = String.Empty;
            await Task.Run(() =>
            {
                foreach (var item in Transactions)
                {
                    tempString += item.ToString();
                }
                CurrentHash = CalculateHash(tempString);

                //Proof of work i.e. first five characters of hash should be 0
                while (CurrentHash.Substring(0, Difficulty) != new string('0', Difficulty))
                {
                    Nounce++;
                    CurrentHash = CalculateHash(tempString);
                }
            });
        }

        //Function to calculate hash of block
        private string CalculateHash(string transactionString)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(PreviousHash + transactionString + Nounce.ToString() + BlockAddedTimeStamp.Trim());
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
