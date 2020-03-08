
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Algebra.Models;
using CSharpMath.SkiaSharp;
using SkiaSharp;

[assembly: MenuItemViewer(typeof(Algebra.Views.PrimePViewOld), EMenu.PrimeP)]

namespace Algebra.Views
{
    public class PrimePViewOld : BoardItemView<ViewModels.PrimePViewModel, bool>
    {
        protected override bool IsPropertyToInvalidateView(string p) => base.IsPropertyToInvalidateView(p) || p == nameof(AskViewModel.Expr);

        protected override string EnunciadoStr => $"Averigua si el número {Core.Exprs.NodeExprLaTexBuilderVisitor.ToString(AskViewModel.Expr)} es primo";

        protected override void OnResultadoPaintSurface(SKSurface s, SKImageInfo i)
        {
            base.OnResultadoPaintSurface(s, i);

            if (ViewModel == null)
                return;

            var p = new MathPainter(12) { TextColor = SKColors.White };

            p.LaTeX = (Result) ? @"\text{Es un número primo}" : @"\text{No es un número primo}";

            var pM = p.Measure;

            if (pM.HasValue)
            {
                Resultado.WidthRequest = pM.Value.Width;
                Resultado.HeightRequest = pM.Value.Height;
            }

            p.Draw(s.Canvas, alignment: CSharpMath.Rendering.TextAlignment.Left);
        }
    }
}
