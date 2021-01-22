using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Xml.Serialization;
using WE02Library;

namespace WE02Console
{
    class Program
    {
        private static List<Account> _accounts;
        private static Account _currentAccount;

        private static List<Client> _clients;

        static void Main(string[] args)
        {

            _clients = new List<Client>();
            _clients.Add(new Client("Wim", "Hambrouck"));
            _clients.Add(new Client("Johan", "Janssens"));
            _clients.Add(new Client("Els", "Leys"));

            var creditCards = new List<string> { "123-456-789", "987-654-321" };


            _accounts = new List<Account>();
            _accounts.Add(new RegularAccount("BE99 1234 5678 0123", 1000M, DateTime.Now, 0.02M, _clients[0], creditCards));
            _accounts.Add(new SavingsAccount("BE00 9876 5432 1098", 2000M, DateTime.Now, 0.05M, _clients[0], 0.02M));

            _currentAccount = _accounts[0];

            bool doorgaan = true;

            while (doorgaan)
            {
                Console.Clear();
                ConsoleHelper.TekenKader("MyBank© management system V1.0", true);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"-= CURRENT ACCOUNT =-\n{_currentAccount}");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Gray;

                ConsoleHelper.TekenKader("Main menu");
                var opties = new List<string> { "Withdraw", "Deposit", "New bank account", "Switch account", "Client management" };
                switch (ConsoleHelper.Menu(opties, "Quit"))
                {
                    case 0:
                        Console.WriteLine("Bye!");
                        doorgaan = false;
                        break;
                    case 1:
                        Withdraw(_currentAccount);
                        break;
                    case 2:
                        Deposit(_currentAccount);
                        break;
                    case 3:
                        CreateAccount();
                        break;
                    case 4:
                        SwitchAccount();
                        break;
                    case 5:
                        ManageClients();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private static void ManageClients()
        {
            Console.Clear();
            ConsoleHelper.TekenKader("Client management");

            var opties = new List<string> { "Add client", "Edit existing client" };
            switch (ConsoleHelper.Menu(opties, "Cancel"))
            {
                case 0:
                    break;
                case 1:
                    _clients.Add(NewClient());
                    break;
                case 2:
                    EditClient();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static Client NewClient()
        {
            var voornaam = ConsoleHelper.ReadString("First name: ");
            var achternaam = ConsoleHelper.ReadString("Last name: ");

            return new Client(voornaam, achternaam);
        }

        private static void EditClient()
        {
            ConsoleHelper.TekenKader("Edit client");

            ShowClients();
            Console.WriteLine("0 - Cancel");

            int keuze = ConsoleHelper.ReadInt("Choose client to edit: ", 0, _clients.Count);

            if (keuze != 0)
            {
                Console.Clear();
                ConsoleHelper.TekenKader("Edit client: " + _clients[keuze - 1]);
                _clients[keuze - 1] = NewClient();
            }
        }

        private static void ShowClients()
        {
            int i = 1;
            foreach (var client in _clients)
            {
                Console.WriteLine($"{i++} - {client}");
            }
        }

        private static void SwitchAccount()
        {
            Console.Clear();

            ConsoleHelper.TekenKader("Switch account");

            int i = 1;
            foreach (var account in _accounts)
            {
                Console.WriteLine($"{i++} - {account.IBAN} ({account.AccountType})");
            }
            Console.WriteLine("0 - Cancel");

            int keuze = ConsoleHelper.ReadInt("Choose active account: ", 0, _accounts.Count);

            if (keuze != 0)
            {
                _currentAccount = _accounts[keuze - 1];
            }
        }

        private static void CreateAccount()
        {
            Console.Clear();
            ConsoleHelper.TekenKader("New bank account");
            var opties = new List<string> { "Regular account", "Savings account" };
            switch (ConsoleHelper.Menu(opties, "Cancel"))
            {
                case 0:
                    break;
                case 1:
                    _accounts.Add(NewRegularAccount());
                    break;
                case 2:
                    _accounts.Add(NewSavingsAccount());
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static RegularAccount NewRegularAccount()
        {
            var client = ChooseClient();
            var iban = ConsoleHelper.ReadString("IBAN: ");

            Console.WriteLine("Credit cards (q to stop input):");
            var creditCards = new List<string>();

            string invoer = ConsoleHelper.ReadString("");
            while (invoer != "q")
            {
                creditCards.Add(invoer);
                invoer = ConsoleHelper.ReadString("");
            }

            return new RegularAccount(iban, 0, DateTime.Now, 0.2m, client, creditCards);
        }

        private static Client ChooseClient()
        {
            Console.WriteLine();
            Console.WriteLine("For which client should the account be created?");
            ShowClients();
            int keuze = ConsoleHelper.ReadInt("Choose client: ", 1, _clients.Count);

            return _clients[keuze - 1];
        }

        private static SavingsAccount NewSavingsAccount()
        {
            var client = ChooseClient();
            var iban = ConsoleHelper.ReadString("IBAN: ");

            return new SavingsAccount(iban, 0, DateTime.Now, 0.5m, client, 0.2M);
        }

        private static void Deposit(Account currentAccount)
        {
            bool doorgaan = true;

            while (doorgaan)
            {
                Console.WriteLine();
                Console.Write("Amount to deposit: ");

                string input = Console.ReadLine();
                decimal amount;

                while (!decimal.TryParse(input, out amount))
                {
                    ConsoleHelper.ShowError("Input must be a number.");
                    Console.Write("Amount to deposit: ");
                    input = Console.ReadLine();
                }

                try
                {
                    currentAccount.Deposit(amount);
                    doorgaan = false;
                }
                catch (ArgumentException ex) // gegooid als bedrag < 0
                {
                    ConsoleHelper.ShowError(ex.Message);
                }
            }
        }

        private static void Withdraw(Account currentAccount)
        {
            bool doorgaan = true;

            while (doorgaan)
            {
                Console.WriteLine();
                Console.Write("Amount to withdraw: ");

                string input = Console.ReadLine();
                decimal amount;

                while (!decimal.TryParse(input, out amount))
                {
                    ConsoleHelper.ShowError("Input must be a number.");
                    Console.Write("Amount to withdraw: ");
                    input = Console.ReadLine();
                }

                try
                {
                    currentAccount.Withdraw(amount);
                    doorgaan = false;
                }
                catch (ArgumentException ex) // gegooid als bedrag < 0
                {
                    ConsoleHelper.ShowError(ex.Message);
                }
                catch (InvalidOperationException ex) // gegooid als balance < -1000 zou worden door afhaling
                {
                    ConsoleHelper.ShowError(ex.Message, ConsoleColor.Yellow);
                    Console.Write("Press any key to continue...");
                    Console.ReadKey(true);
                    doorgaan = false;
                }
            }
        }
    }
}
