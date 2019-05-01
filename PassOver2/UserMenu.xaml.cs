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
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace PassOver2
{
    /// <summary>
    /// Interaction logic for UserMenue.xaml
    /// </summary>
    public partial class UserMenu : Window
    {
        internal Customer C { get; set; }
        internal DAO_Class daoClass = new DAO_Class();
        static SqlConnection connection;
        public UserMenu(Customer c)
        {
            this.C = c;
            InitializeComponent();
            string connectionStr = ConfigurationManager.ConnectionStrings["PassOver2.Connect"].ConnectionString;
            connection = new SqlConnection(connectionStr);
            ShowProducts();
            ShowCart();
        }
        
        private void ShowProducts()
        {
            string query = "SELECT * FROM PRODUCT";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
            using (dataAdapter)
            {
                DataTable productTable = new DataTable();
                dataAdapter.Fill(productTable);
                productBOX.DisplayMemberPath = "PROD_NAME";
                productBOX.SelectedValuePath = "PROD_ID";
                productBOX.ItemsSource = productTable.DefaultView;
            }
        }
        private void ShowCart()
        {
            string query = "SHOW_CART";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@custId", C.Id.ToString());
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
               using (dataAdapter)
               {
                   DataTable orderTable = new DataTable();
                   dataAdapter.Fill(orderTable);
                   cartData.DisplayMemberPath = "PROD_NAME";
                   cartData.DisplayMemberPath = "PROD_PROD_PRICE";
                   cartData.DisplayMemberPath = "O_QUANTITY";
                   cartData.DisplayMemberPath = "O_TOTALPRICE";
                   cartData.SelectedValuePath = "O_ID";
                   cartData.ItemsSource = orderTable.DefaultView;
                
               }
               
        }
        
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ProductBOX_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            quantityCombo.Items.Clear();
            quantityCombo.SelectedIndex = 0;
            int prodID=0;
            try
            {
                string query = "SELECT PROD_QUANTITY FROM PRODUCT WHERE PROD_ID=@id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", productBOX.SelectedValue.ToString());
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prodID = (int)reader["PROD_QUANTITY"];
                }

                if (prodID > 0)
                {
                    quantityCombo.Items.Clear();
                    PurchaseBtn.IsEnabled = true;
                    quantityCombo.IsEnabled = true;
                    for (int i = 1; i <= prodID; i++)
                    {
                        quantityCombo.Items.Add(i);
                    }
                }
                else
                {
                    quantityCombo.Items.Clear();
                    PurchaseBtn.IsEnabled = false;
                    quantityCombo.IsEnabled = false;
                    quantityCombo.Items.Add(0);
                }
                reader.Close();
                connection.Close();
            }
            catch (NullReferenceException)
            {

            }
        }

        private void PurchaseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (quantityCombo.SelectedItem != null)
            {
                int amount = (int)quantityCombo.SelectedItem;
                int prodId = (int)productBOX.SelectedValue;
                daoClass.AddNewOrder(amount, C.Id, prodId);
            }
           try
           {
                quantityCombo.Items.Refresh();
                quantityCombo.SelectedIndex = 0;
                ShowCart();
                ShowProducts();
                
                
           }
            catch (Exception)
           {
                
           }


        }
    }
}
