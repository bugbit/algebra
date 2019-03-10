using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.ViewModels
{
    public class AskViewModel : BaseViewModel
    {
        private Core.Session mSession;
        private Models.Menu mMenu;

        public Core.Session Session { get => mSession; set => SetProperty(ref mSession, value); }
        public Models.Menu Menu { get => mMenu; set => SetProperty(ref mMenu, value); }
    }
}
