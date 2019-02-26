using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;

namespace Algebra.Core
{
    public sealed class ResString
    {
        public ResString() { }
        public ResString(Func<CultureInfo> getCulture, Func<ResourceManager> getResourceManage, string key)
        {
            GetCulture = getCulture;
            GetResourceManager = getResourceManage;
        }

        public Func<CultureInfo> GetCulture { get; set; }
        public Func<ResourceManager> GetResourceManager { get; set; }
        public string Key { get; set; }

        public override string ToString() => GetResourceManager.Invoke().GetString(Key, GetCulture.Invoke());
    }
}
