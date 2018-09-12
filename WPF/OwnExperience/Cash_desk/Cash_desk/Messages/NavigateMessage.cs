using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Cash_desk
{
    class NavigateMessage
    {
        public Type ViewType { get; set; }
        public Type ViewModelType { get; set; }
        public UserControl View { get; set; }
    }
}
