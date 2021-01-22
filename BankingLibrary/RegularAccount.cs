using System;
using System.Collections.Generic;
using System.Text;

namespace WE02Library
{
    public class RegularAccount : Account
    {
        public List<string> CreditCards { get; set; }

        public override string AccountType => "Regular account";

        public RegularAccount(string iban, decimal balance, DateTime creationDate, decimal interest, Client client, List<string> creditCards = null) : base(iban, balance, creationDate, interest, client)
        {
            if(creditCards == null)
            {
                CreditCards = new List<string>();
            }
            else
            {
                CreditCards = creditCards;
            }
        }

        public override string ToString()
        {
            return base.ToString() + Environment.NewLine +
                "Credit cards: " + string.Join(", ", CreditCards);
        }
    }
}
