using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Algebra.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AskPage : ContentPage
	{
		public AskPage ()
		{
			InitializeComponent ();
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}