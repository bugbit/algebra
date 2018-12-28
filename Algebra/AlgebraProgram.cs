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

using Algebra.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Algebra
{
    class AlgebraProgram
    {
        private string[] mArgs { get; set; }
        private bool mShowHelp;

        public AlgebraProgram(string[] argArgs)
        {
            mArgs = argArgs;
        }

        [STAThread]
        static int Main(string[] args)
        {
            PrepareConsoleForLocalization();

            return new AlgebraProgram(args).Run();
        }

        public int Run()
        {
            ParseCommandLine();
            if (mShowHelp)
                return DisplayUsage();

            FrmAlgebraPad pFrm = new FrmAlgebraPad();

            pFrm.NewPad();
            Application.Run(pFrm);

            return 0;
        }

        /// <summary>
        /// Prepares the console for localization.
        /// </summary>
        private static void PrepareConsoleForLocalization()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CurrentUICulture.GetConsoleFallbackUICulture();
            if ((Console.OutputEncoding.CodePage != Encoding.UTF8.CodePage) &&
                (Console.OutputEncoding.CodePage != Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage) &&
                (Console.OutputEncoding.CodePage != Thread.CurrentThread.CurrentUICulture.TextInfo.ANSICodePage))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            }
        }

        private void ParseCommandLine()
        {
            for (int i = 0; i < mArgs.Length; i++)
            {
                var pOption = mArgs[i];

                if (pOption == "/?" || pOption.Equals("-help", StringComparison.InvariantCultureIgnoreCase) || pOption.Equals("--h", StringComparison.InvariantCultureIgnoreCase))
                    mShowHelp = true;
            }
        }

        private int DisplayUsage()
        {
            MessageBox.Show(Properties.Resources.UsageText.ToParam(), "Algebra");

            return 0;
        }
    }
}
