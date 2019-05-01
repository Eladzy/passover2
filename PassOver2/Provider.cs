using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassOver2
{
    public class Provider
    {
        public string UserName { get; set; }
        public string ProviderPass { get; set; }
        public string Company { get; set; }
        public int Id { get; set; }
        public Provider()
        {
           
        }
        public static bool operator ==(Provider p1,Provider p2)
        {
            if (ReferenceEquals(p1, null) && ReferenceEquals(p2, null))
                return true;
            if (ReferenceEquals(p1, null) || ReferenceEquals(p2, null))
                return false;

            return (p1.UserName == p2.UserName);
        }
        public static bool operator !=(Provider p1, Provider p2)
        {
            return !(p1 == p2);
        }
        public override bool Equals(object ob)
        {
            if (ReferenceEquals(ob, null))
                return false;
            Provider p = ob as Provider;
            if (ReferenceEquals(p, null))
                return false;

            return this.UserName == p.UserName;
        }
        public override int GetHashCode()
        {
            return Id;
        }
    }
}
