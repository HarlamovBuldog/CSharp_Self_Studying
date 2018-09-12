using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Support;

namespace Cash_desk
{
    public class GroupVM : VMBase
    {

        public Groups TheGroup { get; set; }

        public GroupVM()
        {
            // Initialise the entity or inserts will fail
            //Сюда можно подкинуть инициализацию полей
            TheGroup = new Groups();
        }
        
    }
}
