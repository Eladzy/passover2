using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Windows;

namespace PassOver2
{
   public class DAO_Class:IDao
    {
         readonly static string connectionString = ConfigurationManager.ConnectionStrings["PassOver2.Connect"].ConnectionString;
        static DAO_Class()
        {
           
          

        }

        public void AddNewOrder(int amount,int userId,int productId)
        {
            int totalPrice=0,price=0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT PROD_PRICE FROM PRODUCT WHERE PROD_ID=@id",connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@id", productId.ToString());
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            price = (int)reader["PROD_PRICE"];
                        }
                    }
                }
            }
            totalPrice = price * amount;
            AcceptOrder(userId, amount, productId, totalPrice);
        }
        private void AcceptOrder(int userId, int amount, int productId, int totalPrice)
        {
            string query = "INSERT_O";
            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                using (SqlCommand cmd=new SqlCommand(query, connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId",userId.ToString());
                    cmd.Parameters.AddWithValue("@prodId",productId.ToString());
                    cmd.Parameters.AddWithValue("@quantity",amount.ToString());
                    cmd.Parameters.AddWithValue("@totalPrice", totalPrice.ToString());
                    cmd.ExecuteNonQuery();
                }
            }
            SubtractProduct(amount,productId);
        }
        private void SubtractProduct(int amount,int prodId)
        {
            string query = "REMOVE_PROD";
            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query,connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@amount", amount.ToString());
                    cmd.Parameters.AddWithValue("@id", prodId.ToString());
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void AddToProduct(string prodName,int quantity)
        {
            string query = "UPDATE PRODUCT SET PROD_QUANTITY+=@quantity WHERE PROD_NAME=@prodName";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@prodName", prodName.ToString());
                    cmd.Parameters.AddWithValue("@quantity", quantity.ToString());
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
        }   }
        public bool CheckProductExist(string prodName,int providerId,int price,int quantity)
        {
            string query = "SELECT * FROM PRODUCT WHERE PROD_NAME=@prodName";
            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                using (SqlCommand cmd=new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@prodName", prodName.ToString());
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.HasRows)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            AddNewProduct(prodName,providerId,price,quantity);
                return false;
        }
        private void AddNewProduct(string prodName, int providerId, int price, int quantity)
        {        
            string query = "INSERT_PROD";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using(SqlCommand cmd=new SqlCommand(query, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    cmd.Parameters.AddWithValue("@prodName", prodName.ToString());
                    cmd.Parameters.AddWithValue("@providerNum", providerId.ToString());
                    cmd.Parameters.AddWithValue("@prodPrice", price.ToString());
                    cmd.Parameters.AddWithValue("@prodQuantity", quantity.ToString());
                    
                    cmd.ExecuteNonQuery();
                }
            }

        }  
        public bool CheckProvName(string userName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM PROVIDER WHERE( P_USERNAME=@user)", connection))
                {
                    cmd.Parameters.AddWithValue("@user", userName.ToString());
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

            };
        }
        public bool CheckProvPass()
        {
            throw new NotImplementedException();
        }
        public bool CheckUsrName(string userName)
        {
            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM CUSTOMER WHERE( C_USERNAME=@user)", connection))
                {
                    cmd.Parameters.AddWithValue("@user", userName.ToString());
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    
                }
               
            } 
        }
        public bool LogUser(string userName,string password)
        {
            if (!CheckUsrName(userName))
                return false;
            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                using (SqlCommand cmd=new SqlCommand("SELECT * FROM CUSTOMER WHERE C_USERNAME=@user AND C_PASSWORD=@password", connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@user", userName.ToString());
                    cmd.Parameters.AddWithValue("@password", password.ToString());
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        UserObjecter(userName);
                        return true;
                    }
                    
                }
            }
            return false;
        }
        public bool LogProvider(string userName,string password)
        {
            if (!CheckProvName(userName))
                return false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM PROVIDER WHERE P_USERNAME=@user AND P_PASSWORD=@password", connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@user", userName.ToString());
                    cmd.Parameters.AddWithValue("@password", password.ToString());
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        ProviderObjecter(userName);
                        return true;
                    }

                }
            }
            return false;
        }
        public void CreateNewCustomer(Customer c)
        {
            string query = "INSERT_C";
            using (SqlConnection connection = new SqlConnection("Data Source=laptop-7lva2ppq;Initial Catalog=PassOver2;Integrated Security=True"))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    
                    
                    cmd.Parameters.AddWithValue("@name", c.FirstName.ToString());
                    cmd.Parameters.AddWithValue("@lastName", c.LastName.ToString());
                    cmd.Parameters.AddWithValue("@card", c.CardNumber.ToString());
                    cmd.Parameters.AddWithValue("@user", c.UserName.ToString());
                    cmd.Parameters.AddWithValue("@password", c.UserPass.ToString());
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                SqlCommand cmd1 = new SqlCommand("SELECT * FROM CUSTOMER WHERE C_USERNAME=@user", connection);
                cmd1.Parameters.AddWithValue("@user", c.UserName.ToString());
                connection.Open();
                using (SqlDataReader reader = cmd1.ExecuteReader())
                {              
                    
                    while (reader.Read())
                    {
                        c.Id = (int)reader["C_ID"];
                        Debug.WriteLine(c.Id);
                    }
                    
                }
            }
        }
        public void CreateNewProvider(Provider p)
         {
            string query = "INSERT_P";
            using (SqlConnection connection = new SqlConnection("Data Source=laptop-7lva2ppq;Initial Catalog=PassOver2;Integrated Security=True"))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.AddWithValue("@user", p.UserName.ToString());
                    cmd.Parameters.AddWithValue("@password", p.ProviderPass.ToString());
                    cmd.Parameters.AddWithValue("@company", p.Company.ToString());
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                using (SqlCommand cmd1 = new SqlCommand($"SELECT * FROM PROVIDER WHERE (P_USERNAME=@user)", connection))
                {
                    connection.Open();
                    cmd1.Parameters.AddWithValue("@user", p.UserName.ToString());
                    using (SqlDataReader reader = cmd1.ExecuteReader(CommandBehavior.Default))
                    {

                        while (reader.Read())
                        {
                            p.Id = (int)reader["P_ID"];
                            Debug.WriteLine(p.Id);
                        }
                    }
                }
            }
        }
        private void ProviderObjecter(string userName)
        {
            Provider p=null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM PROVIDER WHERE P_USERNAME=@user", connection))
                {
                    cmd.Parameters.AddWithValue("@user", userName.ToString());
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        p = new Provider
                        {
                            Id = (int)reader["P_ID"],
                            UserName = userName,
                            ProviderPass = (string)reader["P_PASSWORD"],
                            Company=(string)reader["P_COMPANY"]
                            
                        };
                        ProviderInterface provI = new ProviderInterface(p);
                        provI.Show();
                    }
                    reader.Close();
                }
        }   }   
        private void UserObjecter(string userName)
        {
            Customer c=null;
            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                using (SqlCommand cmd=new SqlCommand("SELECT * FROM CUSTOMER WHERE C_USERNAME=@user",connection))
                {
                    cmd.Parameters.AddWithValue("@user", userName.ToString());
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);
                    while (reader.Read())
                    {
                        c = new Customer
                        {
                            UserName = userName,
                            Id=(int)reader["C_ID"],
                            FirstName=(string)reader["C_FIRST_NAME"],
                            LastName = (string)reader["C_LAST_NAME"],
                            UserPass=(string)reader["C_PASSWORD"],
                            CardNumber=(string)reader["C_CardNUM"]

                        };
                        UserMenu userMenu = new UserMenu(c);
                        userMenu.Show();
                    }
                    reader.Close();
                }
            }
        }
   }
}
