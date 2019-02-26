using Algebra.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.ViewModels
{
    public class AddViewModel : BaseViewModel
    {
        private Grupos mMenu;

        public AddViewModel()
        {
            Menu = FactoryMenu.Menu;
        }

        public Grupos Menu { get => mMenu; set { SetProperty(ref mMenu, value); } }
    }
}
