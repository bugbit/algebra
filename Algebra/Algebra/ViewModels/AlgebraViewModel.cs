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
using System.Reflection;
using System.Text;

namespace Algebra.ViewModels
{
    public class AlgebraViewModel : BaseViewModel
    {
        private string _Name, _Version, _Description, _License, _Website;
        private Core.Session mSession;

        public AlgebraViewModel()
        {
            var pAssembly = Assembly.GetExecutingAssembly();

            Name = pAssembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
            Version = pAssembly.GetName().Version.ToString();
            Description = pAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
            License = pAssembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
            Website = "https://github.com/bugbit/algebra";
            Session = new Core.Session();
        }

        public string Name { get => _Name; set => SetProperty(ref _Name, value); }
        public string Version { get => _Version; set => SetProperty(ref _Version, value); }
        public string Description { get => _Description; set => SetProperty(ref _Description, value); }
        public string License { get => _License; set => SetProperty(ref _License, value); }
        public string Website { get => _Website; set => SetProperty(ref _Website, value); }
        public Core.Session Session { get => mSession; set => SetProperty(ref mSession, value); }
    }
}
