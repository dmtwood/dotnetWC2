using System;

namespace WE02Library
{
    public abstract class Account
    {
        private const int _minimumBalance = -1000;

        public string IBAN { get; set; }
        public decimal Balance { get; private set; }
        public DateTime CreationDate { get; set; }
        public decimal Interest { get; set; }
        public abstract string AccountType { get; }
        public Client Client { get; }


        public Account(string iban, decimal balance, DateTime creationDate, decimal interest, Client client)
        {
            IBAN = iban;
            Balance = balance;
            CreationDate = creationDate;
            Interest = interest;
            Client = client;
        }

        /// <summary>
        /// Adds an amount to the balance of the account.
        /// </summary>
        /// <param name="amount">The amount to be added.</param>
        /// <exception cref="ArgumentException">Thrown when amount is 0 or negative.</exception>
        public void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                Balance += amount;
            }
            else
            {
                throw new ArgumentException("Deposited amount must be a number higher than 0!");
            }
        }

        /// <summary>
        /// Subtracts an amount from the balance of the account.
        /// </summary>
        /// <param name="amount">The amount to subtract.</param>
        /// <exception cref="ArgumentException">Thrown when amount is 0 or negative.</exception>
        /// <exception cref="InvalidOperationException">Thrown if subtraction of amount would result in account to be overdrawn.</exception>"
        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Withdrawn amount must be a positive number!");
            }

            if (Balance - amount >= _minimumBalance)
            {
                Balance -= amount;
            }
            else
            {
                throw new InvalidOperationException($"Withdrawal would exceed minimum balance of {_minimumBalance}!");
            }
        }

        public override string ToString()
        {
            return $"Client: {Client}" + Environment.NewLine +
                $"IBAN: {IBAN}, Balance: {Balance:C}, Created on: {CreationDate}, Interest: {Interest}";
        }
     }
}
