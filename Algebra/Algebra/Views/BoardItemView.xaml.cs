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
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Algebra.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BoardItemView : ContentView
    {
        private MathPainter mEnunciadoPainter;

        public BoardItemView()
        {
            InitializeComponent();
            mEnunciadoPainter = new MathPainter(12) { TextColor = SKColors.White, LaTeX = $@"\text{{{ViewModel?.Numero}. {EnunciadoStr}}}" };

            var pM = mEnunciadoPainter.Measure;

            if (pM.HasValue)
            {
                Enunciado.WidthRequest = pM.Value.Width;
                Enunciado.HeightRequest = pM.Value.Height;
            }

            Enunciado.OnPaintSurface += OnEnunciadoPaintSurface;
        }

        public ViewModels.BoardItemViewModel ViewModel => BindingContext as ViewModels.BoardItemViewModel;
        public object AskViewModel => ViewModel?.AskViewModel;

        virtual protected void OnEnunciadoPaintSurface(SKSurface s, SKImageInfo i)
        {
            s.Canvas.Clear(SKColors.Green);

            if (ViewModel == null)
                return;

            var p = new MathPainter(12) { TextColor = SKColors.White, LaTeX = $@"\text{{{ViewModel?.Numero}. {EnunciadoStr}}}" };

            p.Draw(s.Canvas, alignment: CSharpMath.Rendering.TextAlignment.Left);
        }

        virtual protected string EnunciadoStr => string.Empty;
    }
}