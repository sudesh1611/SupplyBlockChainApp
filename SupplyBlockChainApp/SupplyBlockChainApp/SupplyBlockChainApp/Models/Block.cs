using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SupplyBlockChainApp
{
    public class Block
    {
        public string CurrentHash { get; set; }
        public string PreviousHash { get; set; }
        public ulong Nounce { get; set; }
        public List<Transaction> Transactions { get; set; }
        public string BlockAddedTimeStamp { get; set; }

        public Block(string previousHash, List<Transaction> transactions, int Difficulty)
        {
            CurrentHash = String.Empty;
            PreviousHash = previousHash;
            Transactions = transactions;
            BlockAddedTimeStamp = DateTime.UtcNow.ToLongTimeString();
            Nounce = 0;
            //MineBlock(Difficulty);
        }

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
                while (CurrentHash.Substring(0, Difficulty) != new string('0', Difficulty))
                {
                    Nounce++;
                    CurrentHash = CalculateHash(tempString);
                }
            });
            //Console.WriteLine("Block successfully mined : " + CurrentHash);
        }

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
