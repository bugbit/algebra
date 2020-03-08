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

//using CSharpMath.Rendering;
using CSharpMath.SkiaSharp;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Algebra.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BoardItemView : ContentView
    {
        public BoardItemView()
        {
            InitializeComponent();

            Enunciado.OnPaintSurface += OnEnunciadoPaintSurface;
            Resultado.OnPaintSurface += OnResultadoPaintSurface;
        }

        public ViewModels.BoardItemViewModel ViewModel => BindingContext as ViewModels.BoardItemViewModel;
        public object AskViewModel => ViewModel?.AskViewModel;
        public object Result => ViewModel?.Result;

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (IsPropertyToInvalidateView(propertyName))
                Invalidate();
        }

        virtual protected void OnEnunciadoPaintSurface(SKSurface s, SKImageInfo i)
        {
            s.Canvas.Clear(SKColors.Green);

            if (ViewModel == null)
                return;

            var p = new MathPainter(12) { TextColor = SKColors.White };

            p.LaTeX = $@"\text{{{ViewModel?.Numero}. {EnunciadoStr}}}";

            var pM = p.Measure;

            if (pM.HasValue)
            {
                Enunciado.WidthRequest = pM.Value.Width;
                Enunciado.HeightRequest = pM.Value.Height;
            }

            p.Draw(s.Canvas, alignment: CSharpMath.Rendering.TextAlignment.Left);
        }

        virtual protected void OnResultadoPaintSurface(SKSurface s, SKImageInfo i)
        {
            s.Canvas.Clear(SKColors.Green);
        }

        virtual protected string EnunciadoStr => string.Empty;

        virtual protected bool IsPropertyToInvalidateView(string p) => p == nameof(ViewModel.Numero) || p == nameof(ViewModel.Result);

        protected void Invalidate()
        {
            Enunciado?.Invalidate();
            Resultado?.Invalidate();
        }
    }

    public class BoardItemView<A, R> : BoardItemView
    {
        new public A AskViewModel => (base.AskViewModel is A pVM) ? pVM : default(A);
        new public R Result => (base.Result is R pVM) ? pVM : default(R);
    }
}