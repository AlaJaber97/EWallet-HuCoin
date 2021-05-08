using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Miner.Models;

namespace Miner.Services
{
    public class CryptoCurrency
    {
        private readonly List<Transaction> CurrentTransactions;
        private readonly List<Node> Nodes;
        private List<Block> Chain;

        private static int BlockCount;
        private static decimal Reward;
        private static Credential MinerCredential;
        private Block LastBlock => Chain.Last();

        public CryptoCurrency()
        {
            CurrentTransactions = new();
            Chain = new();
            Nodes = new();

            MinerCredential = Utils.RSA.KeyGenerate();
            Reward = 50;
        }
        public void RegisterNodes(string[] listAddress) =>
            Array.ForEach(listAddress, address => RegisterNode(address));
        public void RegisterNode(string address) =>
            Nodes.Add(new Node { Address = new Uri(address) });
        private Block CreateNewBlock(int prevProof, string prevHash = null)
        {
            var block = new Block
            {
                Index = Chain.Count,
                Timestemp = DateTime.UtcNow,
                Transactions = CurrentTransactions,
                Proof = prevProof,
                PreviousHash = prevHash ?? GetHash(Chain.Last()),
            };

            CurrentTransactions.Clear();
            Chain.Add(block);
            return block;
        }
        private string GetHash(Block block)
        {
            var blockText = System.Text.Json.JsonSerializer.Serialize(block);
            return GetSha256(blockText);
        }
        private string GetSha256(string data)
        {
            var sha256 = new SHA256Managed();
            var hashBuilder = new StringBuilder();
            var bytes = Encoding.Unicode.GetBytes(data);
            var hash = sha256.ComputeHash(bytes);

            Array.ForEach(hash, byt => hashBuilder.Append($"{byt:x2}"));
            return hashBuilder.ToString();
        }
        private int ProofGenerate(string prevHash)
        {
            int proof = 0;
            while (!IsVaildProof(CurrentTransactions, proof, prevHash))
                proof++;
            if(BlockCount == 10)
            {
                BlockCount = 0;
                Reward /= 2;
            }
            CurrentTransactions.Add(new Transaction
            {
                Sender = "0",
                Recipient = MinerCredential.PublicKey,
                Amount = Reward,
            });
            BlockCount++;
            return proof;
        }
        private bool IsVaildProof(List<Transaction> transactions, int proof, string prevHash)
        {
            var signatures = transactions.Select(item => item.Signature).ToArray();
            var guess = $"{signatures}{proof}{prevHash}";
            var result = GetSha256(guess);
            return result.StartsWith("00");
        }
        public bool VerifyTransactionSignature(Transaction transaction, string signature, string publicKey)
        {
            var originalMessage = transaction.ToString();
            bool isVerified = Utils.RSA.Verify(publicKey, originalMessage, signature);
            return isVerified;
        }
        public List<Transaction> GetTransactionsByAddress(string ownerAddress)
        {
            return Chain.OrderByDescending(c => c.Index)
                    .SelectMany(item => item.Transactions)
                    .Where(item => item.Sender.Equals(ownerAddress) || item.Recipient.Equals(ownerAddress))
                    .ToList();
        }
        public decimal GetBalance(string ownerAddress)
        {
            var transactions = GetTransactionsByAddress(ownerAddress);
            var Deposit = transactions.Where(item => item.Recipient == ownerAddress).Sum(item => item.Amount);
            var Withdraw = transactions.Where(item=> item.Sender == ownerAddress).Sum(item=> item.Amount);
            return Deposit - Withdraw;
        }
        private bool HasBalance(Transaction transaction) =>
            GetBalance(transaction.Sender) >= transaction.Amount;
        public bool CreateTransaction(Transaction transaction)
        {
            var isVerified = VerifyTransactionSignature(transaction, transaction.Signature, transaction.Sender);
            if (!isVerified || transaction.Sender == transaction.Recipient) return false;
            if (!HasBalance(transaction)) return false;

            AddTransaction(transaction);
            return true;
        }
        private void AddTransaction(Transaction transaction)
        {
            CurrentTransactions.Add(transaction);
            if(transaction.Sender != MinerCredential.PublicKey)
            {
                CurrentTransactions.Add(new Transaction
                {
                    Sender = transaction.Sender,
                    Recipient = MinerCredential.PublicKey,
                    Amount = transaction.Amount,
                });
            }
        }
        public Block Mine()
        {
            int proof = ProofGenerate(LastBlock.PreviousHash);
            return CreateNewBlock(proof);
        }
        public string GetFullChain() => System.Text.Json.JsonSerializer.Serialize(new Blockchain(Chain));
        

        //How to connect with other miner (Consensus)
        //And solve any confilct happen
        
        private bool IsValidChain(List<Block> chain)
        {
            var lastBlock = chain.First();
            foreach (var block in chain)
            {
                //check on sequance
                if (block.PreviousHash != GetHash(lastBlock)) return false;
                //check validation block
                if (!IsVaildProof(block.Transactions, block.Proof, block.PreviousHash)) return false;

                lastBlock = block;
            }
            return true;
        }

        public List<Transaction> GetTransactions() => CurrentTransactions;
        public List<Block> GetBlocks() => Chain;
        public List<Node> GetNodes() => Nodes;
        public Credential GetCredential() => MinerCredential;
    }
}
