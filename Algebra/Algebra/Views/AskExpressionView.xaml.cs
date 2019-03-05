using Algebra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: AskView(typeof(Algebra.Views.AskExpressionView), EMenu.PrimeP)]

namespace Algebra.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AskExpressionView : ContentView
    {
        public AskExpressionView()
        {
            InitializeComponent();
        }
    }
}