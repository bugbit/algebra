using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algebra.Services;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

[assembly: Dependency(typeof(Algebra.Droid.Services.LocateDroid))]

namespace Algebra.Droid.Services
{
    class LocateDroid : LocateBase, ILocate
    {
    }
}