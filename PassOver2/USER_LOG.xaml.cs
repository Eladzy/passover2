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
    /// Interaction logic for USER_LOG.xaml
    /// </summary>
    public partial class USER_LOG : Window
    {
        DAO_Class daoClass = new DAO_Class();
        public USER_LOG()
        {
            InitializeComponent();
        }

        private void CloseUsrLog_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UsrLogBtn_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            string userName = UsrBox.Text;
            var password = UsrPass.Password;
            if (string.IsNullOrWhiteSpace(userName))
            {
                isValid = false;
                UsrLbl.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                UsrLbl.Foreground = new SolidColorBrush(Colors.Black);
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                isValid = false;
                UsrPassLbl.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                UsrPassLbl.Foreground = new SolidColorBrush(Colors.Black);
            }
            if (isValid)
            {
                if (daoClass.LogUser(userName, password))
                {
                    Close();
                }
                else
                {
                    MessageBox.Show("Credentials Error Please Try Again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
