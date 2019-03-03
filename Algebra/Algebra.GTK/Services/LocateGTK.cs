using Algebra.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(Algebra.GTK.Services.LocateGTK))]

namespace Algebra.GTK.Services
{
    class LocateGTK : LocateBase, ILocate
    {
    }
}
