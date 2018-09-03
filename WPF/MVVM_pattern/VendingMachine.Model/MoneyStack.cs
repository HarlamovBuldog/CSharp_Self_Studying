using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Model
{
    public class MoneyStack
    {
        public MoneyStack(Banknote banknote, int amount)
        {
            Banknote = banknote;
            Amount = amount;
        }
        public Banknote Banknote { get; }
        public int Amount { get; }
    }
}
