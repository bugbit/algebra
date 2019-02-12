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

using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.Common
{
    class HelperCodeCom
    {
        public static Lazy<CSharpCodeProvider> CodeProvider { get; } = new Lazy<CSharpCodeProvider>(() =>
        {
            var csc = new CSharpCodeProvider();
            var settings = csc
             .GetType()
             .GetField("_compilerSettings", BindingFlags.Instance | BindingFlags.NonPublic)
             .GetValue(csc);

            var path = settings
             .GetType()
             .GetField("_compilerFullPath", BindingFlags.Instance | BindingFlags.NonPublic);

            path.SetValue(settings, ((string)path.GetValue(settings)).Replace(@"bin\roslyn\", @"roslyn\"));

            return csc;
        });
    }
}
