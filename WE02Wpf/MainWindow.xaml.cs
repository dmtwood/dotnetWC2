using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WE02Library;

namespace WE02Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Account> Accounts { get; set; }
        public Account ActiveAccount { get; set; }

        public List<Client> Clients { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Clients = new List<Client>
            {
                new Client("Johan", "Janssens"),
                new Client("Els", "Leys"),
                new Client("Chris", "Van den Abeele"),
                new Client("Fatma", "Taspinar")
            };

            Accounts = new List<Account> {
                new RegularAccount("01-xxx-xx", 1000m, DateTime.Now, 0.5m, Clients[0], new List<string>{"001-xxx-xx", "001-xxx-xx"}),
                new RegularAccount("02-xxx-xx", 1000m, DateTime.Now, 0.5m, Clients[1], new List<string>{"001-xxx-xx", "001-xxx-xx"}),
                new SavingsAccount("03-xxx-xx", 1000m, DateTime.Now, 0.5m, Clients[2], 2m),
                new SavingsAccount("04-xxx-xx", 1000m, DateTime.Now, 0.5m, Clients[3], 2m)
            };

            ActiveAccount = Accounts.First();
            UpdateGUI();
        }
        private void UpdateGUI()
        {
            LblBalance.Content = ActiveAccount.Balance.ToString("C"); // C = format as currency
            LblAccountType.Content = ActiveAccount.AccountType;
            LblCreationDate.Content = ActiveAccount.CreationDate.ToString("dd MMM yyyy");
            LblInterest.Content = ActiveAccount.Interest;
            LblIBAN.Content = ActiveAccount.IBAN;

            // accounts uit menu halen
            MenuAccounts.Items.Clear();

            // IBAN van elke account tonen in Accounts Menu
            foreach (Account account in Accounts)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Header = account.IBAN;
                // functie AccountMenuItem_Click() toevoegen aan Click delegate van menuItem, m.a.w. als er op één van de accounts word geklikt, wordt deze functie uitgevoerd
                menuItem.Click += AccountMenuItem_Click;
                // nieuw item toevoegen aan Accounts menu
                MenuAccounts.Items.Add(menuItem);
            }

            // Clients tonen in Clients Menu
            MenuClients.Items.Clear();
            foreach (Client Client in Clients)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Header = Client.ToString();
                menuItem.Click += ClientMenuItem_Click;
                MenuClients.Items.Add(menuItem);
            }



        }

        private void AccountMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // check of sender object wel degelijk een MenuItem is en indien ja, casten en opslaan
            if (sender is MenuItem menuItem)
            {
                // Accounts lijst aflopen tot we een account tegenkomen waarvan het IBAN overeenkomt met de Header van het aangeklikte MenuItem
                foreach (var account in Accounts)
                {
                    if (menuItem.Header.ToString() == account.IBAN)
                    {
                        ActiveAccount = account;
                        UpdateGUI();
                        return;
                    }
                }
            }
        }

        private void ClientMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                foreach (var Client in Clients)
                {
                    if (menuItem.Header.ToString() == Client.ToString())
                    {
                        var createEditClientWindow = new CreateEditClientWindow(Client);

                        if (createEditClientWindow.ShowDialog().GetValueOrDefault())
                        {
                            UpdateGUI();
                        }

                        return;
                    }
                }
            }
        }

        private void BtnDeposit_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(TxtAmount.Text, out decimal bedrag))
            {
                try
                {
                    ActiveAccount.Deposit(bedrag);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                UpdateGUI();
            }
            else
            {
                MessageBox.Show("Invalid amount entered.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                TxtAmount.Focus();
                TxtAmount.Select(0, TxtAmount.Text.Length);
            }
        }

        private void BtnWithdraw_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(TxtAmount.Text, out decimal bedrag))
            {
                try
                {
                    ActiveAccount.Withdraw(bedrag);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                UpdateGUI();
            }
            else
            {
                MessageBox.Show("Invalid amount entered.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                TxtAmount.Focus();
                TxtAmount.Select(0, TxtAmount.Text.Length);
            }
        }

        private void FileMenuNewAccount_Click(object sender, RoutedEventArgs e)
        {
            CreateAccountWindow createAccountWindow = new CreateAccountWindow(Clients);
            if (createAccountWindow.ShowDialog().GetValueOrDefault())
            {
                Accounts.Add(createAccountWindow.NewAccount);
                UpdateGUI();
            }
        }

        private void FileMenuNewClient_Click(object sender, RoutedEventArgs e)
        {
            CreateEditClientWindow createEditClientWindow = new CreateEditClientWindow();
            if (createEditClientWindow.ShowDialog().GetValueOrDefault())
            {
                Clients.Add(createEditClientWindow.Client);
                UpdateGUI();
            }
        }

        private void FileMenuExit_Click(object sender, RoutedEventArgs e)
        {
            Close(); // sluit het huidige venster (indien laatste venster == einde app)
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            //OnClosing wordt aangeroepen net voor het venster wordt gesloten
            var result = MessageBox.Show("Are you sure you want to quit?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true; // annuleer de "close" operatie
            }
        }


    }
}
