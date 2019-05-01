using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

namespace PassOver2
{
    /// <summary>
    /// Interaction logic for ProviderInterface.xaml
    /// </summary>
    public partial class ProviderInterface : Window
    {
        private DAO_Class dao_Class = new DAO_Class();
        private Provider P { get; set; }
        private SqlConnection connection;
        public ProviderInterface(Provider p)
        {
            InitializeComponent();
            this.P =p;
            string connectionStr = ConfigurationManager.ConnectionStrings["PassOver2.Connect"].ConnectionString;
            connection = new SqlConnection(connectionStr);
            ShowGrid();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddNewBtn_Click(object sender, RoutedEventArgs e)
        {
            int pPrice=0,pQuantity=0;
            string name = nameBox.Text;
            string price = priceBox.Text;
            string quantity = quantityBox.Text;
            bool isValid = true;
            SolidColorBrush red = new SolidColorBrush(Colors.Red);
            SolidColorBrush black = new SolidColorBrush(Colors.Black);
            if (string.IsNullOrWhiteSpace(name))
            {
                isValid = false;
                pNameLbl.Foreground = red;
            }
            else
            {
                pNameLbl.Foreground = black;
            }
            if (!string.IsNullOrWhiteSpace(price))
            {
              if(Int32.TryParse(price,out pPrice))
              {
                    if (pPrice >= 0)
                    {
                        priceLbl.Foreground = black;
                    }
                    else
                    {
                        isValid = false;
                        priceLbl.Foreground = red;
                    }
              }
                else
                {
                    isValid = false;
                    priceLbl.Foreground = red;
                }

            }
            else
            {
                isValid = false;
                priceLbl.Foreground = red;
            }
            if (!string.IsNullOrWhiteSpace(quantity))
            {
                if (Int32.TryParse(quantity, out pQuantity))
                {
                    if (pQuantity >= 0)
                    {
                        quantityLbl.Foreground = black;
                    }
                    else
                    {
                        isValid = false;
                        quantityLbl.Foreground = red;
                    }

                }
                else
                {
                    isValid = false;
                    quantityLbl.Foreground = red;
                }
            }
            else
            {
                isValid = false;
                quantityLbl.Foreground = red;
            }

            if (isValid)
            {
                if (dao_Class.CheckProductExist(name,P.Id,pPrice,pQuantity))
                {
                    MessageBox.Show("Product Already Exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Success", "Product Added", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                        inventoryGrid.Items.Refresh();
                        ShowGrid();
                     
                }
            }
        }
        private void ShowGrid()
        {
            string query = "SHOW_INVENTORY";
            using (SqlCommand cmd=new SqlCommand(query, connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", P.Id.ToString());
                connection.Open();
                using (SqlDataAdapter dataAdapter=new SqlDataAdapter(cmd))
                {
                    
                    DataTable inventoryTable = new DataTable();
                    dataAdapter.Fill(inventoryTable);
                    inventoryGrid.DisplayMemberPath = "Name";
                    inventoryGrid.DisplayMemberPath = "Price";
                    inventoryGrid.DisplayMemberPath = "Quantity";
                    inventoryGrid.SelectedValuePath = Name;
                    inventoryGrid.ItemsSource = inventoryTable.DefaultView;

                }
                
                connection.Close();
            }
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            int num;
            DataRowView row = inventoryGrid.SelectedItem as DataRowView;
            if(Int32.TryParse(updateBox.Text, out num))
            {
                try
                {
                    if(num>=0)
                    dao_Class.AddToProduct(row["Name"].ToString(), num);
                }
                catch (Exception)
                {

                }
                try
                {
                    ShowGrid();
                }
                catch (Exception)
                {

                    
                }
            }
        }

        private void InventoryGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateBtn.IsEnabled = true;
           
            
        }
    }
}
