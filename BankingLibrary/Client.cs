using System;
using System.Collections.Generic;
using System.Text;

namespace WE02Library
{
    public class Client
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Client(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public Client()
        {
        }

        public override string ToString()
        {
            return $"{LastName.ToUpperInvariant()} {FirstName}";
        }
    }
}
