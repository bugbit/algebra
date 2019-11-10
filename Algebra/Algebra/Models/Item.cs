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
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;

namespace Algebra.Models
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class PageItemAttribute : Attribute
    {
        public EItemId Id { get; set; }
        public Type ViewType { get; set; }

        public PageItemAttribute() { }
        public PageItemAttribute(EItemId argId, Type argViewType)
        {
            Id = argId;
            ViewType = argViewType;
        }
    }

    public enum EItemId
    {
        Grupo = 100,
        NumberTheory = Grupo,
        PrimeP = NumberTheory + 10
    }

    public sealed class Item
    {
        public EItemId Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string TitleKey => $"{Id}_Title";
    }

    public sealed class Group : ObservableCollection<Item>
    {
        public EItemId Id { get; set; }
        public string Text { get; set; }
        public string TitleKey => $"{Id}_Title";
    }

    public sealed class Groups : ObservableCollection<Group>
    {
        private static readonly Lazy<Groups> mInstance = new Lazy<Groups>(Create);

        private Groups()
        {
            DependencyService.Get<Services.ILocate>().CultureChanged += OnCultureChanged;
        }

        public ResourceManager ResourceManager { get; set; } = Core.Algebra_Resources.ResourceManager;

        private void OnCultureChanged(object sender, Services.LocateCultureChanged e)
        {
            UpdateIR(e.Culture);
        }

        private void UpdateIR(CultureInfo argCulture)
        {
            foreach (var g in this)
            {
                g.Text = ResourceManager.GetString(g.TitleKey, argCulture);

                foreach (var i in g)
                {
                    i.Text = ResourceManager.GetString(i.TitleKey, argCulture);
                }
            }
        }

        private static Groups Create()
        {
            var pGroups = new Groups();
            var pDictViews = typeof(Groups).Assembly.GetCustomAttributes<PageItemAttribute>().ToDictionary(t => t.Id, t => t.ViewType);
            var pIdType = Enum.GetUnderlyingType(typeof(EItemId));
            var pMenus =
                from f in typeof(EItemId).GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public)
                let v = (EItemId)f.GetValue(null)
                let g = (int)v / (int)EItemId.Grupo
                let i = (int)v % (int)EItemId.Grupo
                select new { f, g, i, v };
            var pQuery =
                from m in pMenus
                group m by m.g into g
                select new { g = g.Key /*,i=g.OrderBy(ii=>ii.f.)*/ };

            return pGroups;
        }
    }
}