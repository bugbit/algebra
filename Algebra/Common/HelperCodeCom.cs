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
