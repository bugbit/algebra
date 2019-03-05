using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Algebra.Models;

[assembly: AskView(typeof(Algebra.ViewModels.AskExpression), EMenu.PrimeP)]

namespace Algebra.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AskExpression : ContentPage
    {
        public AskExpression()
        {
            InitializeComponent();
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}