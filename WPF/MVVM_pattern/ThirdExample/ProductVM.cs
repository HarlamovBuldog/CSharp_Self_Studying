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
    public class ProductVM
    {
        public Visibility IsBuyVisible { get; }
        public DelegateCommand BuyCommand { get; }
        public string Name { get; }
        public string Price { get; }
        public int Amount { get; }
    }
}
