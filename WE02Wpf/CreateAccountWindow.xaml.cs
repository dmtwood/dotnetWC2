using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WE02Library;

namespace WE02Wpf
{
    /// <summary>
    /// Interaction logic for CreateAccountWindow.xaml
    /// </summary>
    public partial class CreateAccountWindow : Window
    {
        public Account NewAccount { get; set; }

        public CreateAccountWindow(List<Client> clients)
        {
            InitializeComponent();
            // comboBox opvullen met Clients zodat klant kan worden gekozen bij aanmaken rekening
            foreach (Client Client in clients)
            {
                ComboClients.Items.Add(Client);
            }
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (CheckInputs())
            {
                if (RadioNormalAccount.IsChecked.Value)
                {
                    // indien gekozen voor "Normal account", nieuwe zichtrekening aanmaken
                    NewAccount = new RegularAccount(TxtIBAN.Text, 0m, DateTime.Now, 0.2m, ComboClients.SelectedItem as Client, LstCreditCards.Items.OfType<string>().ToList());
                }
                else
                {
                    // bolletje "Savings account" is aangeduid, dus nieuwe spaarrekening
                    NewAccount = new SavingsAccount(TxtIBAN.Text, 0m, DateTime.Now, 1.5m, ComboClients.SelectedItem as Client, 0.5m);
                }
                DialogResult = true; // hoofdprogramma krijgt DialogResult als return waarde van ShowDialog()
                Close(); // venster sluiten
            }
            else
            {
                MessageBox.Show("Please choose an account type, fill in an IBAN and select a Client.", "Invalid input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Checks if the necessary form fields have been filled in.
        /// </summary>
        /// <returns>True if input exists. False if one of the fields is missing input.</returns>
        private bool CheckInputs()
        {
            return (RadioNormalAccount.IsChecked.GetValueOrDefault() || RadioSavingsAccount.IsChecked.GetValueOrDefault()) && !string.IsNullOrEmpty(TxtIBAN.Text) && ComboClients.SelectedIndex != -1;
        }

        private void BtnAddCreditCard_Click(object sender, RoutedEventArgs e)
        {
            // eerst checken of er wel degelijk invoer is en het Credit Card veld. Indien lege string: foutboodschap tonen, indien ingevuld: invoer toevoegen aan de ListBox
            string newCreditCard = TxtCreditCard.Text;
            if (string.IsNullOrWhiteSpace(newCreditCard))
            {
                MessageBox.Show("Please fill in the number of the card you wish to add.", "Invalid input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                TxtCreditCard.Focus();
            }
            else
            {
                LstCreditCards.Items.Add(newCreditCard);
                TxtCreditCard.Text = string.Empty;
            }
        }

        private void RadioSavingsAccount_Checked(object sender, RoutedEventArgs e)
        {
            // "Savings account" werd geselecteerd, dus gedeelte om kredietkaarten toe te voegen in de GUI uitschakelen
            GrpCreditCards.IsEnabled = false;
        }

        private void RadioSavingsAccount_Unchecked(object sender, RoutedEventArgs e)
        {
            // "Savings account" werd gedeselecteerd, dus gedeelte om kredietkaarten toe te voegen in de GUI inschakelen
            GrpCreditCards.IsEnabled = true;
        }
    }
}
