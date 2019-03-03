using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algebra.Services;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(Algebra.iOS.Services.LocateiOS))]

namespace Algebra.iOS.Services
{
    class LocateiOS : LocateBase, ILocate
    {
    }
}