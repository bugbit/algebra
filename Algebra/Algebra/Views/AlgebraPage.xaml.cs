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
    public partial class AlgebraPage : ContentPage
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

            var pAddBtn = new Button { Text = Core.Algebra_Resources.AddBtn_Text };

            pAddBtn.Clicked += AddItem_Clicked;

            await AddInBoard(pAddBtn);

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

        private async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddPage());
        }
    }
}