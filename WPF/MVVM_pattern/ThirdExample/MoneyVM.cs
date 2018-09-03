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

namespace ThirdExample
{
    public class MoneyVM
    {
        public Visibility IsInsertVisible { get; }
        public DelegateCommand InsertCommand { get; }
        public string Icon { get; }
        public string Name { get; }
        public int Amount { get; }
    }
}
