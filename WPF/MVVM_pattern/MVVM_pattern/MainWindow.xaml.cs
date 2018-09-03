using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MVVM_pattern
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _number1;
        public int Number1
        {
            get { return _number1; }
            set
            {
                _number1 = value;
                OnPropertyChanged("Number3"); // уведомление View о том, что изменилась сумма
            }
        }

        private int _number2;
        public int Number2
        {
            get { return _number2; }
            set { _number2 = value; OnPropertyChanged("Number3"); }
        }

        //свойство только для чтения, оно считывается View каждый раз, когда обновляется Number1 или Number2
        public int Number3
        {
            get
            {
                return MathFuncs.GetSumOf(Number1, Number2);
            }
        }
    }

    public static class MathFuncs
    {
        public static int GetSumOf(int a, int b)
        {
            return a + b;
        }
    }
}
