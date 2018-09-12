using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Support;
namespace wpf_EntityFramework
{
    public class CustomerVM : VMBase
    {
        public Customers TheCustomer { get; set; }
        public CustomerVM()
        {
            // Initialise the entity or inserts will fail
            TheCustomer = new Customers();
            TheCustomer.CreditLimit = 5000;
            TheCustomer.Outstanding = 0;
        }

    }
}
