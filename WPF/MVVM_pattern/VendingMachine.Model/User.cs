using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Model
{
    public class User
    {
        public User()
        {
            //кошелек пользователя
            _userWallet = new ObservableCollection<MoneyStack>
               (Banknote.Banknotes.Select(b => new MoneyStack(b, 10)));
            UserWallet = new ReadOnlyObservableCollection<MoneyStack>(_userWallet);
        }

        public ReadOnlyObservableCollection<MoneyStack> UserWallet { get; }
        private readonly ObservableCollection<MoneyStack> _userWallet;

        public int UserSumm { get; }
    }
}
