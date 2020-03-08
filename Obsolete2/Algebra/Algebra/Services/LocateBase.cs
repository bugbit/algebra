using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Algebra.Services
{
    public class LocateBase : ILocate
    {
        private CultureInfo mCulture;

        public CultureInfo Culture
        {
            get => mCulture;
            set
            {
                mCulture = value;
                Core.Algebra_Resources.Culture = value;
                RaiseCultureChanged();
            }
        }

        public event EventHandler<LocateCultureChanged> CultureChanged;

        protected virtual void RaiseCultureChanged()
        {
            CultureChanged?.Invoke(this, new LocateCultureChanged(mCulture));
        }
    }
}
