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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CAS = Algebra.ExpressionCAS;

namespace Algebra
{
    public partial class FrmAlgebraPad : Form
    {
        private int mNumNonameExpression = 0;
        private CancellationTokenSource mCancelToken = new CancellationTokenSource();

        public FrmAlgebraPad()
        {
            InitializeComponent();
            InitFrmName();
            InitPrecisions();
        }

        public PadControl NewPad(string argName = null)
        {
            var pName = argName ?? string.Format(Properties.Resources.NonameExpression, Interlocked.Increment(ref mNumNonameExpression));
            var pPage = new TabPage();
            var pPad = new PadControl(mCancelToken) { NameExpression = pName };

            pPad.ChangeIsEvaluating += Pad_ChangeIsEvaluating;
            pPage.Controls.Add(pPad);
            tabPads.TabPages.Add(pPage);

            return pPad;
        }

        public async Task Evaluate()
        {
            var pPad = PadActive;

            if (pPad == null)
                return;

            var pTypePrecision = PrecisionInfoActive;

            if (pTypePrecision == null)
                return;

            await pPad.Evaluate(pTypePrecision);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            mCancelToken.Cancel();
        }

        private PadControl PadActive => tabPads.SelectedTab?.Controls.OfType<PadControl>().FirstOrDefault();
        private CAS.Precisions.Info PrecisionInfoActive
        {
            get
            {
                var pItem = precisionToolStripMenuItem.DropDownItems.OfType<ToolStripMenuItem>().FirstOrDefault(i => i.Checked);

                if (pItem == null)
                    return null;

                return (CAS.Precisions.Info)pItem.Tag;
            }
        }

        private void InitFrmName()
        {
            var pAssembly = Assembly.GetExecutingAssembly();
            var pName = pAssembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
            var pVersion = pAssembly.GetName().Version.ToString();

            Text = $"{pName} {pVersion}";
        }

        private void InitPrecisions()
        {
            precisionToolStripMenuItem.DropDownItems.AddRange((from p in CAS.Precisions.PrecicionsInfo.Values select new ToolStripMenuItem { Text = p.Name, Tag = p }).ToArray());
            ((ToolStripMenuItem)precisionToolStripMenuItem.DropDownItems[0]).Checked = true;
        }

        private void Pad_ChangeIsEvaluating(PadControl argPad, bool argIsValidate)
        {
            if (argPad == PadActive)
            {
                evaluateToolStripMenuItem.Enabled = !argIsValidate;
                cancelToolStripMenuItem.Enabled = argIsValidate;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FrmAlgebraPad_Load(object sender, EventArgs e)
        {
            PadActive?.FocusExpression();
        }

        private async void evaluateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Evaluate();
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PadActive?.CancelEvaluate();
        }

        private void tabPads_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pad_ChangeIsEvaluating(PadActive, PadActive.IsEvaluating);
        }
    }
}
