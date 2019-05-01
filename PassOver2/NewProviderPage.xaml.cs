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
    /// Interaction logic for NewProviderPage.xaml
    /// </summary>
    public partial class NewProviderPage : Window
    {
        DAO_Class daoInstance = new DAO_Class();
        public NewProviderPage()
        {
            InitializeComponent();
        }

        private void NewProviderBtn_Click(object sender, RoutedEventArgs e)
        {
            var red = new SolidColorBrush(Colors.Red);
            var black = new SolidColorBrush(Colors.Black);
            bool isValid = true;
            string userName = pUserNameBox.Text;
            string password = pPassBox.Text;
            string companyName = companyBox.Text;
            //User Input Check
            if (string.IsNullOrWhiteSpace(userName))
            {
                isValid = false;
                pUsrNameLbl.Foreground = red;
            }
            else
            {
                pUsrNameLbl.Foreground = black;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                isValid = false;
                pPassLbl.Foreground = red;
            }
            else
            {
                pPassLbl.Foreground = black;
            }
            if (string.IsNullOrWhiteSpace(companyName))
            {
                isValid = false;
                companyLbl.Foreground = red;
            }
            else
            {
                companyLbl.Foreground = black;
            }
            //if everythin is valid the user name is being checked
            if (isValid)
            {
                if (!daoInstance.CheckProvName(userName))
                {
                    Provider p = new Provider
                    {
                        UserName = userName,
                        ProviderPass = password,
                        Company = companyName
                    };
                    try
                    {
                        daoInstance.CreateNewProvider(p);
                        MessageBox.Show("Success!");
                        Close();
                    }
                    catch (System.Data.SqlClient.SqlException)
                    {
                        MessageBox.Show("An error occured registration might not be complete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                    finally
                    {
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("User Name Already Exist", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

