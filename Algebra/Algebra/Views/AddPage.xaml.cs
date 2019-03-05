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
                var pView = Activator.CreateInstance(pMenuItem.AskViewType);

                if (pView is Page pPage)
                {
                    pPage.Title = pMenuItem.Title;
                    await Navigation.PushModalAsync(pPage);
                }
            }
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}