using Algebra.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.ViewModels
{
    public class AddViewModel : BaseViewModel
    {
        private Menu mMenu;

        public AddViewModel()
        {
            Menu = FactoryMenu.Menu;
        }

        public Menu Menu { get => mMenu; set { SetProperty(ref mMenu, value); } }
    }
}
