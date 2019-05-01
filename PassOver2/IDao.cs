using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassOver2
{
    interface IDao
    {
        void AddNewOrder(int amount, int userId, int productId);
        void AddToProduct(string prodName, int quantity);
        bool CheckProductExist(string prodName, int providerId, int price, int quantity);
        bool CheckProvName(string userName);
        bool CheckProvPass();
        bool CheckUsrName(string userName);
        void CreateNewCustomer(Customer c);
        void CreateNewProvider(Provider p);
        bool LogProvider(string userName, string password);
        bool LogUser(string userName, string password);
    }
}
