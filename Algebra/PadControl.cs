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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using CAS = Algebra.ExpressionCAS;
using System.Threading;

namespace Algebra
{
    public delegate void ChangeIsEvaluateHandler(PadControl argPad, bool argIsValidate);

    public partial class PadControl : UserControl
    {
        private string mNameExpression;
        private CAS.PadContext mExprCASContext = new CAS.PadContext();
        private IProgress<PadProgress> mPadProgress;
        private CancellationTokenSource mFrmAlgebraCancel;
        private CancellationTokenSource mPadCancel = null;

        public PadControl(CancellationTokenSource argFrmAlgebraCancel = null)
        {
            mFrmAlgebraCancel = argFrmAlgebraCancel;
            InitializeComponent();
            tlProcesoName.Text = Properties.Resources.Ready;
            mPadProgress = new Progress<PadProgress>(ProgressChanged);
            AddOutputHeader();
        }

        public event ChangeIsEvaluateHandler ChangeIsEvaluate;

        public string NameExpression
        {
            get => mNameExpression;
            set
            {
                mNameExpression = value;
                UpdateNameExpressionInParent();
            }
        }

        public string ExpressionString
        {
            get => txtExpression.Text;
            set => txtExpression.Text = value;
        }

        public IList<string> ExpressionLines
        {
            get => txtExpression.Lines;
        }

        public void AddOutput(string argText)
        {
            txtOutput.AppendText(argText);
        }

        public void FocusExpression()
        {
            Select();
            txtExpression.Focus();
        }

        public async Task Evaluate(CAS.Precisions.Info argPrecisionInfo)
        {
            // Controlar isevaluate
            ChangeIsEvaluate?.Invoke(this, true);
            try
            {
                mPadCancel?.Dispose();
                mPadCancel = new CancellationTokenSource();

                using (var pCancel = CancellationTokenSource.CreateLinkedTokenSource(mFrmAlgebraCancel.Token, mPadCancel.Token))
                {
                    var pEvaluator = new CAS.Evaluate.Evaluator(mPadProgress, pCancel.Token, argPrecisionInfo, mExprCASContext);
                    var pExpr = await pEvaluator.Evaluate(ExpressionLines);

                    if (pExpr == null)
                        return;
                }
            }
            finally
            {
                ChangeIsEvaluate?.Invoke(this, true);
                mPadCancel?.Dispose();
                mPadCancel = null;
            }
        }
        public void CancelEvaluate()
        {
            mPadCancel?.Cancel();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            UpdateNameExpressionInParent();
        }

        private void UpdateNameExpressionInParent()
        {
            if (Parent != null)
                Parent.Text = mNameExpression;
        }

        private void AddOutputHeader()
        {
            var pAssembly = Assembly.GetExecutingAssembly();
            var pName = pAssembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
            var pVersion = pAssembly.GetName().Version.ToString();
            var pDescription = pAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
            var pLicense = pAssembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;

            AddOutput
            (
                $@"/*
    {pName} Version {pVersion}
    {pDescription}
    https://github.com/bugbit/algebra

    {pLicense}
    GNU GENERAL PUBLIC LICENSE
*/
                "
            );
        }

        private void ProgressChanged(PadProgress argProgress)
        {
            if (!string.Equals(tlProcesoName.Text, argProgress.Name))
                tlProcesoName.Text = argProgress.Name;
            if (tgProcess.Visible != argProgress.Visible)
                tgProcess.Visible = argProgress.Visible;
            if (tgProcess.Minimum != argProgress.Minimum)
                tgProcess.Minimum = argProgress.Minimum;
            if (tgProcess.Maximum != argProgress.Maximum)
                tgProcess.Maximum = argProgress.Maximum;
            if (tgProcess.Value != argProgress.Progress)
                tgProcess.Value = argProgress.Progress;
        }
    }
}
