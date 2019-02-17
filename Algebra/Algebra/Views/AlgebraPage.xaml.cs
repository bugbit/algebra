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

            var s = new StackLayout { Spacing = 0 };

            s.Children.Add(new Label { Text = $"{vm.Name} Version {vm.Version}" });
            s.Children.Add(new Label { Text = vm.Description });

            var l = new Label { Text = vm.Website, TextColor = Color.Blue, TextDecorations = TextDecorations.Underline };

            l.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => Device.OpenUri(new Uri(vm.Website)))
            });

            s.Children.Add(l);
            s.Children.Add(new Label());
            s.Children.Add(new Label { Text = vm.License });
            s.Children.Add(new Label { Text = "GNU GENERAL PUBLIC LICENSE" });
            AddInBoard(s);

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

        public void AddInBoard(View v)
        {
            Board.Children.Add(v);
        }
    }
}