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

using Algebra.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using Xamarin.Forms;

namespace Algebra.Models
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MenuOfAttribute : Attribute
    {
        public EMenu Grupo { get; set; }

        public MenuOfAttribute(EMenu grupo)
        {
            Grupo = grupo;
        }
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    public class AskViewAttribute : Attribute
    {
        public EMenu[] Ids { get; set; }
        public Type ViewType { get; set; }

        public AskViewAttribute(Type argType, params EMenu[] argIds)
        {
            Ids = argIds;
            ViewType = argType;
        }
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    public class MenuItemViewerAttribute : Attribute
    {
        public Type ViewerType { get; set; }
        public EMenu Id { get; set; }
        public MenuItemViewerAttribute(Type argType, EMenu argId)
        {
            ViewerType = argType;
            Id = argId;
        }
    }

    public enum EMenu
    {
        NumberTheory,
        [MenuOf(NumberTheory)]
        PrimeP
    }

    public class MenuItem : IEquatable<MenuItem>
    {
        public EMenu Id { get; set; }
        public string TitleKey => $"{Id}_Title";
        public string Title { get; set; }
        public Type AskViewType { get; set; }
        public Type ViewerType { get; set; }

        public bool Equals(MenuItem other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj) => (obj is MenuItem m) && Equals(m);

        public override int GetHashCode() => Id.GetHashCode();
    }

    public class Grupo : ObservableCollection<MenuItem>, IEquatable<Grupo>
    {
        public EMenu Id { get; set; }
        public string TitleKey => $"{Id}_Title";
        public string Title { get; set; }

        public bool Equals(Grupo other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj) => (obj is Grupo m) && Equals(m);

        public override int GetHashCode() => Id.GetHashCode();
    }

    public sealed class Menu : ObservableCollection<Grupo>, IDisposable
    {
        public Menu()
        {
            DependencyService.Get<Services.ILocate>().CultureChanged += OnCultureChanged;
        }

        private void OnCultureChanged(object sender, Services.LocateCultureChanged e)
        {
            UpdateIR(e.Culture);
        }

        public ResourceManager ResourceManager { get; set; }

        public void UpdateIR(CultureInfo argCulture)
        {
            foreach (var g in this)
            {
                g.Title = ResourceManager.GetString(g.TitleKey, argCulture);
                foreach (var m in g)
                    m.Title = ResourceManager.GetString(m.TitleKey, argCulture);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar llamadas redundantes

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: elimine el estado administrado (objetos administrados).
                    DependencyService.Get<Services.ILocate>().CultureChanged -= OnCultureChanged;
                    ClearItems();
                }

                // TODO: libere los recursos no administrados (objetos no administrados) y reemplace el siguiente finalizador.
                // TODO: configure los campos grandes en nulos.

                ResourceManager = null;
                disposedValue = true;
            }
        }

        // TODO: reemplace un finalizador solo si el anterior Dispose(bool disposing) tiene código para liberar los recursos no administrados.
        // ~Menu() {
        //   // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
        //   Dispose(false);
        // }

        // Este código se agrega para implementar correctamente el patrón descartable.
        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
            Dispose(true);
            // TODO: quite la marca de comentario de la siguiente línea si el finalizador se ha reemplazado antes.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
