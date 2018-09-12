using Prism;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VendingMachine.Model;

namespace ThirdExample
{
    public class MoneyVM : BindableBase
    {
        public MoneyStack MoneyStack { get; }

        public MoneyVM(MoneyStack moneyStack, PurchaseManager manager = null)
        {
            MoneyStack = moneyStack;
            moneyStack.PropertyChanged += (s, a) => { RaisePropertyChanged(nameof(Amount)); };

            if (manager != null) //по умолчанию Null, если же нет, то тогда задаем DelegateCommand
                InsertCommand = new DelegateCommand(() => {
                    manager.InsertMoney(MoneyStack.Banknote);
                });

           
        }

        public Visibility IsInsertVisible => InsertCommand == null ? Visibility.Collapsed : Visibility.Visible;
        public DelegateCommand InsertCommand { get; }
        public string Icon => MoneyStack.Banknote.IsCoin ? "..\\Images\\coin.jpg" : "..\\Images\\banknote.png";
        public string Name => MoneyStack.Banknote.Name;
        public int Amount => MoneyStack.Amount;
    }
}
