using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Algebra.Services
{
    public class LocateCultureChanged : EventArgs
    {
        public CultureInfo Culture { get; }

        public LocateCultureChanged(CultureInfo argCulture)
        {
            Culture = argCulture;
        }
    }
    public interface ILocate
    {
        CultureInfo Culture { get; set; }

        event EventHandler<LocateCultureChanged> CultureChanged;
    }
}
