using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace PassOver2
{
    /// <summary>
    /// Interaction logic for ProviderLogIn.xaml
    /// </summary>
    public partial class ProviderLogIn : Window
    {
        DAO_Class daoClass = new DAO_Class();
        public ProviderLogIn()
        {
            InitializeComponent();
        }

        private void ProviderLogClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ProviderLogBtn_Click(object sender, RoutedEventArgs e)
        {
            string userName = providerNameBox.Text;
            var password = ProviderPass.Password.ToString();
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(userName))
            {
                isValid = false;
                providerNameLbl.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                providerNameLbl.Foreground = new SolidColorBrush(Colors.Black);
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                isValid = false;
                providerPassLbl.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                providerPassLbl.Foreground = new SolidColorBrush(Colors.Black);
            }
            if (isValid == true)
            {
                if (!daoClass.LogProvider(userName,password))
                {
                    MessageBox.Show("Credentials Error Please Try Again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Close(); ;
                }
            }
        }
    }
}
