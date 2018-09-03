using Prism;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Model;

namespace ThirdExample
{
    public class MainViewVM : BindableBase
    {
        public MainViewVM()
        {
            _user = new User();
        }

        public int UserSumm => _user.UserSumm;
        public ObservableCollection<MoneyVM> UserWallet { get; }
        public ObservableCollection<ProductVM> UserBuyings { get; }
        public DelegateCommand GetChange { get; }
        public int Credit { get; }
        public ReadOnlyObservableCollection<MoneyVM> AutomataBank { get; }
        public ReadOnlyObservableCollection<ProductVM> ProductsInAutomata { get; }

        private User _user;
    }
}
