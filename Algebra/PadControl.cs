﻿#region LICENSE
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

namespace Algebra
{
    public partial class PadControl : UserControl
    {
        private string mNameExpression;
        private CAS.PadContext mExprCASContext = new CAS.PadContext();

        public PadControl()
        {
            InitializeComponent();
            AddOutputHeader();
        }

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

        public void AddOutput(string argText)
        {
            txtOutput.AppendText(argText);
        }

        public void FocusExpression()
        {
            Select();
            txtExpression.Focus();
        }

        public void Evaluate(Type argTypePrecision)
        {
            var pExpr = CAS.UserExpression.Evaluate(argTypePrecision, mExprCASContext, ExpressionString);

            if (pExpr == null)
                return;
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
    }
}
