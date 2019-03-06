using Algebra.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.ViewModels
{
    public class AddViewModel : BaseViewModel
    {
        private Menu mMenu;
        private Core.Session mSession;

        public AddViewModel()
        {
            Menu = FactoryMenu.Menu;
        }

        public Menu Menu { get => mMenu; set { SetProperty(ref mMenu, value); } }
        public Core.Session Session { get => mSession; set => SetProperty(ref mSession, value); }
    }
}
