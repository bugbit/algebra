using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Algebra.Core.Math;
using Algebra.Models;

[assembly: AskViewModel(typeof(Algebra.ViewModels.PrimePViewModel), EMenu.PrimeP)]

namespace Algebra.ViewModels
{
    public class PrimePViewModel : AskExpressionViewModel<bool>
    {
        public override async Task Calculate()
        {
            Result = await Session.Alg.PrimeP(Expr, EAlgorithmPrimeP.Default, CancellationToken.None);
        }
    }
}
