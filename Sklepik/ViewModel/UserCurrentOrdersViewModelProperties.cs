using Sklepik.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sklepik.ViewModel
{
    public partial class UserCurrentOrdersViewModel
    {
        private List<UserOrderHeaderModel> _userOredersList = new List<UserOrderHeaderModel>();

        public List<UserOrderHeaderModel> OrdersList
        {
            get { return _userOredersList; }
            set
            {
                _userOredersList = value;
                NotifyPropertyChanged(nameof(OrdersList));
            }
        }

    }
}
