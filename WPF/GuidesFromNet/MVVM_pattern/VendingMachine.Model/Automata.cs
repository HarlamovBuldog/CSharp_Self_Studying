using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Model
{
    public class Automata : BindableBase
    {
        public Automata()
        {
            //деньгохранилище автомата
            _automataBank = new ObservableCollection<MoneyStack>
                (Banknote.Banknotes.Select(b => new MoneyStack(b, 100)));
            AutomataBank = new ReadOnlyObservableCollection<MoneyStack>(_automataBank);
            //продукты автомата
            _productsInAutomata =
                new ObservableCollection<ProductStack>(Product.Products.Select(p => new ProductStack(p, 100)));
            ProductsInAutomata = new ReadOnlyObservableCollection<ProductStack>(_productsInAutomata);
        }

        public ReadOnlyObservableCollection<MoneyStack> AutomataBank { get; }
        private readonly ObservableCollection<MoneyStack> _automataBank;
        public ReadOnlyObservableCollection<ProductStack> ProductsInAutomata { get; }
        private readonly ObservableCollection<ProductStack> _productsInAutomata;

        //поместить купюру в отделение для соответственной купюры
        internal void InsertBanknote(Banknote banknote)
        {
            _automataBank.First(ms => ms.Banknote.Equals(banknote)).PushOne();
            Credit += banknote.Nominal;
        }

        //кредит
        private int credit;
        public int Credit
        {
            get { return credit; }
            set { SetProperty(ref credit, value); }
        }

        internal bool BuyProduct(Product product)
        {
            if (Credit >= product.Price && _productsInAutomata.First(p => p.Product.Equals(product)).PullOne())
            {
                Credit -= product.Price;
                return true;
            }
            return false;
        }

        internal bool GetChange(out IEnumerable<MoneyStack> change)
        {
            change = new List<MoneyStack>();
            if (Credit == 0) return false;

            var creditToReturn = Credit;
            var toReturn = new List<MoneyStack>();
            foreach (var ms in _automataBank.OrderByDescending(m => m.Banknote.Nominal))
            {
                if (creditToReturn >= ms.Banknote.Nominal)
                {
                    toReturn.Add(new MoneyStack(ms.Banknote, creditToReturn / ms.Banknote.Nominal));
                    creditToReturn -= (creditToReturn / ms.Banknote.Nominal) * ms.Banknote.Nominal;
                }
            }
            if (creditToReturn != 0) return false; //денег не набирается, ничего не возвращаем

            foreach (var ms in toReturn) //возвращаем
                for (int i = 0; i < ms.Amount; ++i)  //по одной монетке правда
                    _automataBank.First(m => Equals(m.Banknote, ms.Banknote)).PullOne();
            change = toReturn;
            Credit = 0;
            return true;
        }
    }
}
