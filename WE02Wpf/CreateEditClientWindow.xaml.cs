using System;
using System.Collections.Generic;
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
    /// Interaction logic for CreateEditClientWindow.xaml
    /// </summary>
    public partial class CreateEditClientWindow : Window
    {
        public Client Client { get; private set; }

        public CreateEditClientWindow(Client editClient = null)
        {
            InitializeComponent();
            if (editClient == null)
            {
                Client = new Client();
            }
            else
            {
                Title = "Edit Client";
                BtnCreate.Content = "_Save";
                Client = editClient;
                TxtFirstName.Text = editClient.FirstName;
                TxtLastName.Text = editClient.LastName;
            }
        }


        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (CheckInputs())
            {
                Client.FirstName = TxtFirstName.Text;
                Client.LastName = TxtLastName.Text;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please fill in both fields.", "Invalid input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private bool CheckInputs()
        {
            return !(string.IsNullOrWhiteSpace(TxtFirstName.Text) || string.IsNullOrWhiteSpace(TxtLastName.Text));
        }
    }
}
