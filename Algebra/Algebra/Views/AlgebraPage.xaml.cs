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

using Algebra.Services;
using Algebra.ViewModels;
using CSharpMath.SkiaSharp;
using SkiaForms;
using SkiaSharp;
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
    public partial class AlgebraPage : ContentPage, IBoard
    {
        public AlgebraPage()
        {
            InitializeComponent();
        }

        public async Task AddInBoard(View v)
        {
            Board.Children.Add(v);
            await Scroll.ScrollToAsync(v, ScrollToPosition.MakeVisible, true);
        }

        public async Task<BoardItemView> AddInBoard(AskViewModel argViewModel)
        {
            var pItemVM = new BoardItemViewModel { Numero = NuevoNumeroItem(), AskViewModel = argViewModel };
            var pItemViewObj = Activator.CreateInstance(argViewModel.MenuItem.ViewerType);

            if (!(pItemViewObj is BoardItemView pItemView))
                return null;

            pItemView.BindingContext = pItemVM;

            await AddInBoard(pItemView);

            return pItemView;
        }

        Task IBoard.AddInBoard(AskViewModel argViewModel) => AddInBoard(argViewModel);

        public async Task Start()
        {
            var s = new StackLayout { Spacing = 0 };

            s.Children.Add(new Label { Text = $"{vm.Name} Version {vm.Version}", TextColor = Color.White });
            s.Children.Add(new Label { Text = vm.Description, TextColor = Color.White });

            var l = new Label { Text = vm.Website, TextColor = Color.Wheat, TextDecorations = TextDecorations.Underline };

            l.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => Device.OpenUri(new Uri(vm.Website)))
            });

            s.Children.Add(l);
            s.Children.Add(new Label());
            s.Children.Add(new Label { Text = vm.License, TextColor = Color.White });
            s.Children.Add(new Label { Text = "GNU GENERAL PUBLIC LICENSE", TextColor = Color.White });
            await AddInBoard(s);

            //MathPainter p = new MathPainter(40) { LaTeX = @"\raisebox{25mu}{\text{\kern.7222emC\#}\\Math}"

            //};

            //var skiaView = new SkiaView()
            //{
            //    WidthRequest = 200,
            //    HeightRequest = 100,
            //    BackgroundColor=Color.Green
            //};

            //skiaView.OnPaintSurface = (surface, imageInfo) =>
            //{
            //    surface.Canvas.Clear(SKColors.Green);
            //    p.Draw(surface.Canvas);
            //};

            //await AddInBoard(skiaView);

            //var pSession = ((AlgebraViewModel)BindingContext).Session;
            //var pAskVM = new PrimePViewModel()
            //{
            //    Session = pSession,
            //    ExprStr = "11"
            //};

            //pAskVM.Expr = await pSession.Alg.Parse("11", System.Threading.CancellationToken.None);
            //await pAskVM.Calculate(System.Threading.CancellationToken.None);

            //var pItemVM = new BoardItemViewModel { Numero = NuevoNumeroItem(), AskViewModel = pAskVM, /*Result = await pSession.Alg.PrimeP(pAskVM.Expr, Core.Math.EAlgorithmPrimeP.Default, System.Threading.CancellationToken.None) */};
            //var pItemView = new PrimePView();

            //pItemView.BindingContext = pItemVM;

            //await AddInBoard(pItemView);


            //var pAddBtn = new Button { Text = Core.Algebra_Resources.AddBtn_Text };

            //pAddBtn.Clicked += AddItem_Clicked;

            //await AddInBoard(pAddBtn);

            /*

            var skiaView = new SkiaView()
            {
                WidthRequest = 40,
                HeightRequest = 40
            };

            skiaView.OnPaintSurface = (surface, imageInfo) =>
            {
                var centerX = 20;
                var centerY = 20;

                var canvas = surface.Canvas;
                canvas.Clear();

                var fill = new SKPaint()
                {
                    IsAntialias = true,
                    Style = SKPaintStyle.Fill,
                    Color = SKColor.Parse("#ff0000")
                };

                canvas.DrawCircle(centerX, centerY, 20, fill);
            };

            AddInBoard(skiaView);

            MathPainter p = new MathPainter(40) { LaTeX = @"\raisebox{25mu}{\text{\kern.7222emC\#}\\Math}" };

            skiaView = new SkiaView()
            {
                WidthRequest = 200,
                HeightRequest = 100
            };

            skiaView.OnPaintSurface = (surface, imageInfo) =>
            {
                p.Draw(surface.Canvas);
            };

            AddInBoard(skiaView);

    */
        }

        private int NuevoNumeroItem()
        {
            lock (Board)
                return (from v in Board.Children.OfType<BoardItemView>() let n = v.ViewModel.Numero orderby n descending select n).FirstOrDefault() + 1;
        }

        private async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddPage(vm.Session, this));
        }
    }
}