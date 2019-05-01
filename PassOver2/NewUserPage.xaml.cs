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
    /// Interaction logic for NewUserPage.xaml
    /// </summary>
    public partial class NewUserPage : Window
    {
        DAO_Class daoInstance = new DAO_Class();
        public NewUserPage()
        {
            InitializeComponent();
          
        }
        
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveUserBtn_Click(object sender, RoutedEventArgs e)
        {
            var red = new SolidColorBrush(Colors.Red);
            var black= new SolidColorBrush(Colors.Black);
            bool isValid = true;
            string firstName = firstNameBox.Text;
            string lastName = lastNameBox.Text;
            string userName = custUserNameBox.Text;
            string password = usrPassBox.Text;
            string credNumber = creditBox.Text;
            //User Input Check
            if (string.IsNullOrWhiteSpace(firstName))
            {
                isValid = false;
                firstNameLbl.Foreground = red;
            }
            else
            {
                firstNameLbl.Foreground = black;
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                isValid = false;
                lastNameLbl.Foreground = red;
            }
            else
            {
                lastNameLbl.Foreground = black;
            }

            if(string.IsNullOrWhiteSpace(userName))
            {
                isValid = false;
                custUserNmaeLbl.Foreground = red;
            }
            else
            {
                custUserNmaeLbl.Foreground = black;
            }

            if(string.IsNullOrWhiteSpace(password))
            {
                isValid = false;
                UsrPasswordLbl.Foreground = red;
            }
            else
            {
                UsrPasswordLbl.Foreground = black;
            }
            //Valid credit card number is 16 digits 
            if ((credNumber.Length == 16)&&(credNumber.All(c => c >= '0' && c <= '9')))
            {
                
                CredLbl.Foreground = black;

            }
            else
            {
                isValid = false;
                CredLbl.Foreground = red;
            }
            //if everythin is valid the user name is being checked
            if(isValid)
            {
                if (!daoInstance.CheckUsrName(userName))
                {
                    Customer c = new Customer
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        UserName = userName,
                        UserPass = password,
                        CardNumber = credNumber
                    };
                    try
                    {
                        daoInstance.CreateNewCustomer(c);
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
                    MessageBox.Show("User Name Already Exist","Error",MessageBoxButton.OK,MessageBoxImage.Warning);
                }
            }
        }
    }
}
