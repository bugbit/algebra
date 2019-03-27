#region LICENSE
/*
    Algebra Software free CAS
    Copyright © 2018 Óscar Hernández Bañó
    This file is part of Algebra.
    Algebra is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

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
        public AddPage(Core.Session argSession)
        {
            InitializeComponent();
            vm.Session = argSession;
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if ((e.SelectedItem is Models.MenuItem pMenuItem) && pMenuItem.AskViewType != null)
            {
                var pPage = new AskPage { Title = pMenuItem.Title };
                var pViewObj = Activator.CreateInstance(pMenuItem.AskViewType);
                var pViewModel = Activator.CreateInstance(pMenuItem.AskViewModelType);

                if (pViewModel is ViewModels.AskViewModel pAskViewModel)
                {
                    pAskViewModel.Menu = vm.Menu;
                    pAskViewModel.Session = vm.Session;
                }

                pPage.BindingContext = pViewModel;

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