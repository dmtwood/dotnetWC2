using System;
using System.Collections.Generic;
using System.Text;

namespace WE02Library
{
    public class SavingsAccount : Account
    {
        public decimal LoyaltyBonus { get; set; }

        public override string AccountType => "Savings account";

        public SavingsAccount(string iban, decimal balance, DateTime creationDate, decimal interest, Client client, decimal loyaltyBonus) : base(iban, balance, creationDate, interest, client)
        {
            LoyaltyBonus = loyaltyBonus;
        }

        public override string ToString()
        {
            return base.ToString() + $", Loyalty bonus: {LoyaltyBonus:C}";
        }
    }
}
