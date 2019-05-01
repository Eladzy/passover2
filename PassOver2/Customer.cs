using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassOver2
{
   public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CardNumber {get; set;}
        public string UserName { get; set; }
        public string UserPass { get; set; }
        
        public Customer()
        {
            
        }
        public static bool operator ==(Customer c1, Customer c2)
        {
            if (ReferenceEquals(c1, null) && ReferenceEquals(c2, null))
                return true;
            if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null))
                return false;

            return (c1.UserName == c2.UserName);
        }
        public static bool operator !=(Customer c1, Customer c2)
        {
            return !(c1 == c2);
        }
        public override bool Equals(object ob)
        {
            if (ReferenceEquals(ob, null))
                return false;
            Customer c = ob as Customer;
            if (ReferenceEquals(c, null))
                return false;

            return this.UserName == c.UserName;
        }
        public override int GetHashCode()
        {
            return Id;
        }

    }
}
