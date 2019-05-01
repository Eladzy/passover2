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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;

namespace PassOver2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string connectionStr = ConfigurationManager.ConnectionStrings["PassOver2.Connect"].ConnectionString;
        }

        private void MainExit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Exit Application?", "Exit",MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }

        }

        private void UserLogIn_Click(object sender, RoutedEventArgs e)
        {
            USER_LOG uSER_LOG = new USER_LOG();
            uSER_LOG.Show();
        }

        private void ProviderLogIn_Click(object sender, RoutedEventArgs e)
        {
            ProviderLogIn providerLog = new ProviderLogIn();
            providerLog.Show();
        }

        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            NewUserPage newUser=new NewUserPage();
            newUser.Show();
          
        }

        private void NewProvider_Click(object sender, RoutedEventArgs e)
        {
            NewProviderPage newProviderPage = new NewProviderPage();
            newProviderPage.Show();
        }
    }
}
