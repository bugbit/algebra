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
    public partial class AddPage : ContentPage
    {
        public AddPage()
        {
            InitializeComponent();
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if ((e.SelectedItem is Models.MenuItem pMenuItem) && pMenuItem.AskViewType != null)
            {
                var pPage = new AskPage { Title = pMenuItem.Title };
                var pViewObj = Activator.CreateInstance(pMenuItem.AskViewType);

                if (pViewObj is ContentView pView)
                    pPage.View.Children.Add(pView);
                await Navigation.PushModalAsync(pPage);
            }
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}